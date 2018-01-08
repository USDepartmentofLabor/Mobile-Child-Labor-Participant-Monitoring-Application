using System;
using System.Collections.Generic;
using System.Linq;
using MDPMS.Database.Data.Models;
using MDPMS.Database.Data.Models.Base;
using MDPMS.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MDPMS.Shared.Workers
{
    public static class SyncWorker
    {
        /*
             SYNC STRATEGY
             =============
             * SUMMARY => Examine record existence and last update age
                * For now, mobile can not edit households after it is synced to parent
                * Use integer id from parent to identify records
                * Mobile will have internal and external (parent DPMS) ids
                * Record age will be determined by updated_at/LastUpdatedAt datetimes
                              
               Mobile                                               Parent_DPMS
               ======                                               ===========
             * GetRecordFromParent <------------------------------- Exists on parent but not on mobile
             * Exists on Mobile but not on parent ----------------> GetRecordFromMobile (if not previously synced, otherwise soft/hard delete from mobile)
             * Older on mobile than parent <--UpdateMobileRecord--- Newer on parent than mobile
             * Newer on mobile than parent ---NotYetSupported-----> Older on parent than mobile             (Currently invalid, would be supported by updating parent record)
                            
             REASONING =>

             Exists
             ======
             Mobile | Parent
             ---------------
             0      |   0       * N/A
             0      |   1       * Exists only on parent, mobile get from parent
             1      |   0       * Exists only on mobile, send to parent         !!! NOTE check for internal id since it could have been deleted from parent
             1      |   1       * Exists on both, see Record_Age table ==>
 
             Exists_Special_Case
             ===================
             1      |   0       * If no external id, send to parent
             1      |   0       * If has external id, delete or soft delete internally

             Record_Age
             ==========
             Mobile | Parent
             ---------------
             0      |   0       * Same age, no action
             0      |   1       * Newer on parent, update mobile
             1      |   0       * Newer on mobile, update parent    (NOT_YET_SUPPORTED)
             1      |   1       * Same age, no action
 
            NOTE on newer on mobile:
            For now we can send those to the parent and mark as NEEDS_TO_BE_MANUALLY_RESOLVED

            ALSO, deleted from parent, if already synced but no longer on parent delete on mobile, use soft delete?
            this would require the parent to support deleting households
             */
             
        public static Tuple<bool, string> Sync(ApplicationInstanceData applicationInstanceData,
            bool allowAlreadySyncedUpdateToParent)
        {
            try
            {
                // Status Customization Look Ups
                var statusCustomizationHazardousConditionsResult = SyncObject(
                    applicationInstanceData,
                    allowAlreadySyncedUpdateToParent,
                    @"/api/v1/status_customization_hazardous_conditions",
                    applicationInstanceData.Data.StatusCustomizationHazardousConditions);
                if (!statusCustomizationHazardousConditionsResult.Item1)
                {
                    return new Tuple<bool, string>(false, @"Sync error");
                }

                var statusCustomizationHouseholdTasksResult = SyncObject(
                    applicationInstanceData,
                    allowAlreadySyncedUpdateToParent,
                    @"/api/v1/status_customization_household_tasks",
                    applicationInstanceData.Data.StatusCustomizationHouseholdTasks);
                if (!statusCustomizationHouseholdTasksResult.Item1)
                {
                    return new Tuple<bool, string>(false, @"Sync error");
                }

                var statusCustomizationWorkActivitiesResult = SyncObject(
                    applicationInstanceData,
                    allowAlreadySyncedUpdateToParent,
                    @"/api/v1/status_customization_work_activities",
                    applicationInstanceData.Data.StatusCustomizationWorkActivities);
                if (!statusCustomizationWorkActivitiesResult.Item1)
                {
                    return new Tuple<bool, string>(false, @"Sync error");
                }

                // Data
                var householdsResult = SyncObject(
                    applicationInstanceData,
                    allowAlreadySyncedUpdateToParent,
                    @"/api/v1/households",
                    applicationInstanceData.Data.Households);
                if (!householdsResult.Item1)
                {
                    return new Tuple<bool, string>(false, @"Sync error");
                }

                var householdsNewResult = SyncNewParentObjects(
                    applicationInstanceData,
                    @"/api/v1/households",
                    applicationInstanceData.Data.Households);
                if (!householdsNewResult.Item1)
                {
                    return new Tuple<bool, string>(false, @"Sync error");
                }
                
                //< IncomeSource, Household >
                var incomeSourcesResult = SyncChildObject(                     
                    applicationInstanceData,
                    allowAlreadySyncedUpdateToParent,
                    @"/api/v1/income_sources",
                    applicationInstanceData.Data.IncomeSources,
                    @"household_id",
                    applicationInstanceData.Data.Households);
                if (!incomeSourcesResult.Item1)
                {
                    return new Tuple<bool, string>(false, @"Sync error");
                }

                var incomeSourcesNewResult = SyncNewChildObjects(
                    applicationInstanceData,
                    @"/api/v1/income_sources",
                    applicationInstanceData.Data.IncomeSources,
                    @"household_id",
                    applicationInstanceData.Data.Households);
                if (!incomeSourcesNewResult.Item1)
                {
                    return new Tuple<bool, string>(false, @"Sync error");
                }
            }
            catch
            {
                // TODO: error log
                return new Tuple<bool, string>(false, @"Sync error");
            }
            return new Tuple<bool, string>(true, @"");
        }
        
        public static Tuple<bool, string> SyncObject<T>(ApplicationInstanceData applicationInstanceData, bool allowAlreadySyncedUpdateToParent, string apiPath, DbSet<T> data) where T : class, ISyncable<T>, new()
        {
            // parent objects
            try
            {
                // get existing objects from parent
                var existingObjectsJson = Helper.Rest.RestHelper.PerformRestGetRequestWithApiKey(
                    applicationInstanceData.SerializedApplicationInstanceData.Url,
                    apiPath,
                    applicationInstanceData.SerializedApplicationInstanceData.ApiKey);

                // deserialize and sync Households
                var idsInParent = new List<int>();
                dynamic objectParse = JsonConvert.DeserializeObject(existingObjectsJson);
                foreach (var objectInstance in objectParse)
                {
                    T newObject = new T().GetObjectFromJson(objectInstance);
                    idsInParent.Add((int)newObject.GetExternalId());                    
                    var query = data.Where(a => a.GetExternalId().Equals(newObject.GetExternalId()));
                    if (query.Any())
                    {
                        // sync non new records
                        var record = query.First();

                        // the parent is newer so update the local
                        if (record.GetLastUpdatedAt() < newObject.GetLastUpdatedAt()) record.UpdateObject(newObject);
                        else if (record.GetLastUpdatedAt() > newObject.GetLastUpdatedAt())
                        {
                            if (allowAlreadySyncedUpdateToParent)
                            {
                                // IF_SUPPORTED: the local is newer so update the parent with new info
                                if (record.GetObjectNeedsUpate(newObject))
                                {
                                    var putSuccess = Helper.Rest.RestHelper.PerformRestPutRequestWithApiKeyAndId(
                                        applicationInstanceData.SerializedApplicationInstanceData.Url,
                                        apiPath,
                                        applicationInstanceData.SerializedApplicationInstanceData.ApiKey,
                                        newObject.GenerateUpdateJsonFromObject(record),
                                        record.GetExternalId().ToString());
                                    if (putSuccess.Item1)
                                    {
                                        // set last updated at from JSON response
                                        dynamic jsonResponseParse = JsonConvert.DeserializeObject(putSuccess.Item2);
                                        if (jsonResponseParse.status.Value.ToString().Equals(@"success"))
                                        {
                                            objectInstance.LastUpdatedAt = ((DateTime)DateTime.Parse(jsonResponseParse.updated_at.Value)).ToUniversalTime();
                                        }
                                        else
                                        {
                                            // TODO: error log    
                                            return new Tuple<bool, string>(false, @"Update error");
                                        }
                                    }
                                    else
                                    {
                                        // TODO: error log    
                                        return new Tuple<bool, string>(false, @"Update error");
                                    }
                                }
                                else
                                {
                                    record.SetLastUpdatedAt(newObject.GetLastUpdatedAt());
                                }
                            }
                            else
                            {
                                // NOT_YET_SUPPORTED: the local is newer so update the parent with new info
                                // TODO: error log
                                return new Tuple<bool, string>(false, @"Update error, update from mobile not allowed");
                            }
                        }
                    }
                    else
                    {
                        // add it locally, it is new to mobile
                        data.Add(newObject);
                    }
                }
                applicationInstanceData.Data.SaveChanges();

                // Delete local households not on parent, i.e. not in GET but has external id, requires parent suppoorting delete
                var idQuery = data.Where(a => !a.GetExternalId().Equals(null)).Select(a => (int)a.GetExternalId()).ToList();
                var toDeleteIds = idQuery.Except(idsInParent).ToList();
                foreach (var id in toDeleteIds)
                {
                    // TODO: generic delete children first if contains relationship
                    if (typeof(T) == typeof(Household))
                    {
                        // delete income sources related to it
                        var children =
                            applicationInstanceData.Data.IncomeSources.Where(a => a.GetExternalParentId().Equals(id));
                        foreach (var incomeSource in children)
                        {
                            var childRecord = applicationInstanceData.Data.IncomeSources.First(a => a.InternalId.Equals(incomeSource.InternalId));
                            applicationInstanceData.Data.IncomeSources.Remove(childRecord);
                        }
                        applicationInstanceData.Data.SaveChanges();                        
                    }

                    var rec = data.Where(a => a.GetExternalId().Equals(id));
                    data.Remove(rec.First());
                }
                applicationInstanceData.Data.SaveChanges();                                
            }
            catch
            {
                // TODO: error log
                return new Tuple<bool, string>(false, @"Sync error");
            }
            return new Tuple<bool, string>(true, @"");
        }

        public static Tuple<bool, string> SyncNewParentObjects<T>(
            ApplicationInstanceData applicationInstanceData,
            string apiPath,
            DbSet<T> data) where T : class, ISyncable<T>, new()
        {
            try
            {
                // post new households last
                foreach (var objectInstance in data.Where(a => a.GetExternalId().HasValue == false))
                {
                    var objectJson = objectInstance.GetJsonFromObject();
                    // TODO: if child update parent id here before post
                    var postSuccess = Helper.Rest.RestHelper.PerformRestPostRequestWithApiKey(
                        applicationInstanceData.SerializedApplicationInstanceData.Url,
                        apiPath,
                        applicationInstanceData.SerializedApplicationInstanceData.ApiKey,
                        objectJson);

                    if (postSuccess.Item1)
                    {
                        // set last updated at from JSON response
                        dynamic jsonResponseParse = JsonConvert.DeserializeObject(postSuccess.Item2);
                        if (jsonResponseParse.status.Value.ToString().Equals(@"success"))
                        {
                            objectInstance.SetLastUpdatedAt(((DateTime)DateTime.Parse(jsonResponseParse.updated_at.Value)).ToUniversalTime());
                            objectInstance.SetExternalId((int?)jsonResponseParse.id);
                        }
                        else
                        {
                            // TODO: error log    
                            return new Tuple<bool, string>(false, @"Add error");
                        }
                    }
                    else
                    {
                        // TODO: error log    
                        return new Tuple<bool, string>(false, @"Add error");
                    }
                }
                applicationInstanceData.Data.SaveChanges();
            }
            catch
            {
                // TODO: error log
                return new Tuple<bool, string>(false, @"Sync error");
            }
            return new Tuple<bool, string>(true, @"");
        }
        
        // T is child
        // T2 is parent
        public static Tuple<bool, string> SyncChildObject<T, T2>(ApplicationInstanceData applicationInstanceData,
            bool allowAlreadySyncedUpdateToParent, string apiPath, DbSet<T> data, string parentIdName, DbSet<T2> parentData)
            where T : class, ISyncableAsChild<T>, new()        
            where T2 : class , ISyncable<T2>, new()
        {
            // similar to parent, check id link and validity of id
            try
            {                
                var incomeSourcesResult = SyncObject(applicationInstanceData, allowAlreadySyncedUpdateToParent, apiPath, data);
                if (!incomeSourcesResult.Item1)
                {
                    return new Tuple<bool, string>(false, @"Sync error");
                }

                // set foreign keys                
                // get data again and set (TODO: do in 1 step for child objects)
                var existingObjectsJson = Helper.Rest.RestHelper.PerformRestGetRequestWithApiKey(
                    applicationInstanceData.SerializedApplicationInstanceData.Url,
                    apiPath,
                    applicationInstanceData.SerializedApplicationInstanceData.ApiKey);
                dynamic objectParse = JsonConvert.DeserializeObject(existingObjectsJson);
                foreach (var objectInstance in objectParse)
                {
                    T newObject = new T().GetObjectFromJson(objectInstance); 
                    // find the existig object to attach
                    var existingObjectQuery = data.Where(a => a.GetExternalId().Equals(newObject.GetExternalId()));
                    if (existingObjectQuery.Count().Equals(1))
                    {
                        newObject = existingObjectQuery.First();
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, @"search error");
                    }
                    
                    // find its parent and attach it
                    var parentQuery = parentData.Where(a => a.GetExternalId().Equals(newObject.GetExternalParentId()));
                    if (parentQuery.Count().Equals(1))
                    {
                        if (typeof(T) == typeof(IncomeSource))
                        {                            
                            var parent = parentQuery.First() as Household;
                            if (parent.IncomeSources == null) parent.IncomeSources = new List<IncomeSource>();
                            parent.IncomeSources.Add(newObject as IncomeSource);                            
                        }
                        else
                        {
                            return new Tuple<bool, string>(false, @"parent child error");
                        }
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, @"parent child error");
                    }                    
                }
                applicationInstanceData.Data.SaveChanges();                
            }
            catch
            {
                // TODO: error log
                return new Tuple<bool, string>(false, @"Sync error");
            }
            return new Tuple<bool, string>(true, @"");
        }
        
        // T is child
        // T2 is parent
        public static Tuple<bool, string> SyncNewChildObjects<T, T2>(
            ApplicationInstanceData applicationInstanceData,
            string apiPath,
            DbSet<T> data,
            string parentIdName,
            DbSet<T2> parentData)
            where T : class, ISyncableAsChild<T>, new()
            where T2 : class, ISyncableWithChildren<T2>, new()
        {
            try
            {
                // TEMP: set external and internal parent ids on the child objects when null
                foreach (var parent in parentData)
                {
                    parent.SetParentIdsInChildObjects();
                }

                applicationInstanceData.Data.SaveChanges();
                
                // post new households last
                foreach (var objectInstance in data.Where(a => a.GetExternalId().HasValue == false))
                {
                    var objectJson = objectInstance.GetJsonFromObject();
                    // find the parent and get its external id
                    var query = parentData.Where(a => a.GetInternalId().Equals(objectInstance.GetInternalParentId()));
                    if (!query.Count().Equals(1))
                    {
                        return new Tuple<bool, string>(false, @"Search error");
                    }
                    objectInstance.SetExternalParentId(query.First().GetExternalId());                    
                    var postSuccess = Helper.Rest.RestHelper.PerformRestPostRequestWithApiKey(
                        applicationInstanceData.SerializedApplicationInstanceData.Url,
                        apiPath,
                        applicationInstanceData.SerializedApplicationInstanceData.ApiKey,
                        objectJson);

                    if (postSuccess.Item1)
                    {
                        // set last updated at from JSON response
                        dynamic jsonResponseParse = JsonConvert.DeserializeObject(postSuccess.Item2);
                        if (jsonResponseParse.status.Value.ToString().Equals(@"success"))
                        {
                            objectInstance.SetLastUpdatedAt(((DateTime)DateTime.Parse(jsonResponseParse.updated_at.Value)).ToUniversalTime());
                            objectInstance.SetExternalId((int?)jsonResponseParse.id);
                        }
                        else
                        {
                            // TODO: error log    
                            return new Tuple<bool, string>(false, @"Add error");
                        }
                    }
                    else
                    {
                        // TODO: error log    
                        return new Tuple<bool, string>(false, @"Add error");
                    }
                }
                applicationInstanceData.Data.SaveChanges();
            }
            catch
            {
                // TODO: error log
                return new Tuple<bool, string>(false, @"Sync error");
            }
            return new Tuple<bool, string>(true, @"");
        }
    }
}
