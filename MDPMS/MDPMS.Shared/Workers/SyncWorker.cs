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
                // Look Ups
                // PersonRelationship
                var personRelationshipResult = SyncObject(
                    applicationInstanceData,
                    false,
                    @"/api/relationships",
                    applicationInstanceData.Data.PersonRelationships);
                if (!personRelationshipResult.Item1)
                {
                    return new Tuple<bool, string>(false, applicationInstanceData.SelectedLocalization.Translations[@"ErrorSyncError"]);
                }

                // Custom Fields
                var customFieldResult = SyncObject(
                    applicationInstanceData,
                    false,
                    @"/api/custom_fields",
                    applicationInstanceData.Data.CustomFields);
                if (!customFieldResult.Item1)
                {
                    return new Tuple<bool, string>(false, applicationInstanceData.SelectedLocalization.Translations[@"ErrorSyncError"]);
                }

                // Status Customization Look Ups
                var statusCustomizationHazardousConditionsResult = SyncObject(
                    applicationInstanceData,
                    allowAlreadySyncedUpdateToParent,
                    @"/api/status_customization_hazardous_conditions",
                    applicationInstanceData.Data.StatusCustomizationHazardousConditions);
                if (!statusCustomizationHazardousConditionsResult.Item1)
                {
                    return new Tuple<bool, string>(false, applicationInstanceData.SelectedLocalization.Translations[@"ErrorSyncError"]);
                }

                var statusCustomizationHouseholdTasksResult = SyncObject(
                    applicationInstanceData,
                    allowAlreadySyncedUpdateToParent,
                    @"/api/status_customization_household_tasks",
                    applicationInstanceData.Data.StatusCustomizationHouseholdTasks);
                if (!statusCustomizationHouseholdTasksResult.Item1)
                {
                    return new Tuple<bool, string>(false, applicationInstanceData.SelectedLocalization.Translations[@"ErrorSyncError"]);
                }

                var statusCustomizationWorkActivitiesResult = SyncObject(
                    applicationInstanceData,
                    allowAlreadySyncedUpdateToParent,
                    @"/api/status_customization_work_activities",
                    applicationInstanceData.Data.StatusCustomizationWorkActivities);
                if (!statusCustomizationWorkActivitiesResult.Item1)
                {
                    return new Tuple<bool, string>(false, applicationInstanceData.SelectedLocalization.Translations[@"ErrorSyncError"]);
                }

                // Service Type Categories
                // TODO: add new method for non datetime based sync for this table
                var serviceTypeCategoriesResult = SyncObject(
                    applicationInstanceData,
                    allowAlreadySyncedUpdateToParent,
                    @"/api/service_type_categories",
                    applicationInstanceData.Data.ServiceTypeCategories);
                if (!serviceTypeCategoriesResult.Item1)
                {
                    return new Tuple<bool, string>(false, applicationInstanceData.SelectedLocalization.Translations[@"ErrorSyncError"]);
                }

                // Service Types
                var serviceTypesResult = SyncChildObject(
                    applicationInstanceData,
                    allowAlreadySyncedUpdateToParent,
                    @"/api/service_types",
                    applicationInstanceData.Data.ServiceTypes,
                    @"service_type_category_id",
                    applicationInstanceData.Data.ServiceTypeCategories);
                if (!serviceTypesResult.Item1)
                {
                    return new Tuple<bool, string>(false, applicationInstanceData.SelectedLocalization.Translations[@"ErrorSyncError"]);
                }

                // set service type parent internal ids since not added through ui
                // TODO: alternate method for server only child records?
                foreach (var serviceTypeCategory in applicationInstanceData.Data.ServiceTypeCategories)
                {
                    serviceTypeCategory.SetParentIdsInChildObjects();
                }
                applicationInstanceData.Data.SaveChanges();

                // Services
                var servicesResult = SyncChildObject(
                    applicationInstanceData,
                    allowAlreadySyncedUpdateToParent,
                    @"/api/services",
                    applicationInstanceData.Data.Services,
                    @"service_type_id",
                    applicationInstanceData.Data.ServiceTypes);
                if (!servicesResult.Item1)
                {
                    return new Tuple<bool, string>(false, applicationInstanceData.SelectedLocalization.Translations[@"ErrorSyncError"]);
                }

                foreach (var serviceType in applicationInstanceData.Data.ServiceTypes)
                {
                    serviceType.SetParentIdsInChildObjects();
                }
                applicationInstanceData.Data.SaveChanges();

                // Data
                var householdsResult = SyncObject(
                    applicationInstanceData,
                    allowAlreadySyncedUpdateToParent,
                    @"/api/households",
                    applicationInstanceData.Data.Households);
                if (!householdsResult.Item1)
                {
                    return new Tuple<bool, string>(false, applicationInstanceData.SelectedLocalization.Translations[@"ErrorSyncError"]);
                }

                var householdsNewResult = SyncNewParentObjects(
                    applicationInstanceData,
                    @"/api/households",
                    applicationInstanceData.Data.Households);
                if (!householdsNewResult.Item1)
                {
                    return new Tuple<bool, string>(false, applicationInstanceData.SelectedLocalization.Translations[@"ErrorSyncError"]);
                }
                
                //< IncomeSource, Household >
                var incomeSourcesResult = SyncChildObject(                     
                    applicationInstanceData,
                    allowAlreadySyncedUpdateToParent,
                    @"/api/income_sources",
                    applicationInstanceData.Data.IncomeSources,
                    @"household_id",
                    applicationInstanceData.Data.Households);
                if (!incomeSourcesResult.Item1)
                {
                    return new Tuple<bool, string>(false, applicationInstanceData.SelectedLocalization.Translations[@"ErrorSyncError"]);
                }

                var incomeSourcesNewResult = SyncNewChildObjects(
                    applicationInstanceData,
                    @"/api/income_sources",
                    applicationInstanceData.Data.IncomeSources,
                    @"household_id",
                    applicationInstanceData.Data.Households);
                if (!incomeSourcesNewResult.Item1)
                {
                    return new Tuple<bool, string>(false, applicationInstanceData.SelectedLocalization.Translations[@"ErrorSyncError"]);
                }

                //<Person, Household>
                var peopleResult = SyncChildObject(
                    applicationInstanceData,
                    allowAlreadySyncedUpdateToParent,
                    @"/api/people",
                    applicationInstanceData.Data.People,
                    @"household_id",
                    applicationInstanceData.Data.Households);
                if (!peopleResult.Item1)
                {
                    return new Tuple<bool, string>(false, applicationInstanceData.SelectedLocalization.Translations[@"ErrorSyncError"]);
                }

                var peopleNewResult = SyncNewChildObjects(
                    applicationInstanceData,
                    @"/api/people",
                    applicationInstanceData.Data.People,
                    @"household_id",
                    applicationInstanceData.Data.Households);
                if (!peopleNewResult.Item1)
                {
                    return new Tuple<bool, string>(false, applicationInstanceData.SelectedLocalization.Translations[@"ErrorSyncError"]);
                }

                // <PersonFollowUp, Person>
                var peopleFollowUpResult = SyncChildObject(
                    applicationInstanceData,
                    allowAlreadySyncedUpdateToParent,
                    @"/api/follow_ups",
                    applicationInstanceData.Data.PersonFollowUps,
                    @"person_id",
                    applicationInstanceData.Data.People);
                if (!peopleFollowUpResult.Item1)
                {
                    return new Tuple<bool, string>(false, applicationInstanceData.SelectedLocalization.Translations[@"ErrorSyncError"]);
                }

                var peopleFollowUpNewResult = SyncNewChildObjects(
                    applicationInstanceData,
                    @"/api/follow_ups",
                    applicationInstanceData.Data.PersonFollowUps,
                    @"person_id",
                    applicationInstanceData.Data.People);
                if (!peopleFollowUpNewResult.Item1)
                {
                    return new Tuple<bool, string>(false, applicationInstanceData.SelectedLocalization.Translations[@"ErrorSyncError"]);
                }

                // Service Instances
                var serviceInstancesResult = SyncChildObject(
                    applicationInstanceData,
                    allowAlreadySyncedUpdateToParent,
                    @"/api/service_instances",
                    applicationInstanceData.Data.ServiceInstances,
                    @"person_id",
                    applicationInstanceData.Data.People);
                if (!serviceInstancesResult.Item1)
                {
                    return new Tuple<bool, string>(false, applicationInstanceData.SelectedLocalization.Translations[@"ErrorSyncError"]);
                }

                foreach (var person in applicationInstanceData.Data.People)
                {
                    person.SetParentIdsInChildObjects();
                }
                applicationInstanceData.Data.SaveChanges();

                // new
                var serviceInstancesNewResult = SyncNewChildObjects(
                    applicationInstanceData,
                    @"/api/service_instances",
                    applicationInstanceData.Data.ServiceInstances,
                    @"person_id",
                    applicationInstanceData.Data.People);
                if (!serviceInstancesNewResult.Item1)
                {
                    return new Tuple<bool, string>(false, applicationInstanceData.SelectedLocalization.Translations[@"ErrorSyncError"]);
                }

                // Custom Household Values
                var customHouseholdValuesResult = SyncCustomValueObject(
                    applicationInstanceData,
                    allowAlreadySyncedUpdateToParent,
                    @"/api/custom_values",
                    applicationInstanceData.Data.CustomHouseholdValues,
                    applicationInstanceData.Data.CustomFields.Where(a => a.ModelType == @"Household").ToList());
                if (!customHouseholdValuesResult.Item1)
                {
                    return new Tuple<bool, string>(false, applicationInstanceData.SelectedLocalization.Translations[@"ErrorSyncError"]);
                }
                var postNewCustomHouseholdValuesResult = PostCustomValues(
                    applicationInstanceData,
                    @"/api/custom_values",
                    applicationInstanceData.Data.CustomHouseholdValues);
                if (!postNewCustomHouseholdValuesResult.Item1)
                {
                    return new Tuple<bool, string>(false, applicationInstanceData.SelectedLocalization.Translations[@"ErrorSyncError"]);
                }

                // Custom Person Values
                var customPersonValuesResult = SyncCustomValueObject(
                    applicationInstanceData,
                    allowAlreadySyncedUpdateToParent,
                    @"/api/custom_values",
                    applicationInstanceData.Data.CustomPersonValues,
                    applicationInstanceData.Data.CustomFields.Where(a => a.ModelType == @"Person").ToList());
                if (!customPersonValuesResult.Item1)
                {
                    return new Tuple<bool, string>(false, applicationInstanceData.SelectedLocalization.Translations[@"ErrorSyncError"]);
                }
                var postNewCustomPersonValuesResult = PostCustomValues(
                    applicationInstanceData,
                    @"/api/custom_values",
                    applicationInstanceData.Data.CustomPersonValues);
                if (!postNewCustomPersonValuesResult.Item1)
                {
                    return new Tuple<bool, string>(false, applicationInstanceData.SelectedLocalization.Translations[@"ErrorSyncError"]);
                }

                // Custom PersonFollowUp Values
                var customPersonFollowUpValuesResult = SyncCustomValueObject(
                    applicationInstanceData,
                    allowAlreadySyncedUpdateToParent,
                    @"/api/custom_values",
                    applicationInstanceData.Data.CustomPersonFollowUpValues,
                    applicationInstanceData.Data.CustomFields.Where(a => a.ModelType == @"FollowUp").ToList());
                if (!customPersonFollowUpValuesResult.Item1)
                {
                    return new Tuple<bool, string>(false, applicationInstanceData.SelectedLocalization.Translations[@"ErrorSyncError"]);
                }
                var postNewCustomPersonFollowUpValuesResult = PostCustomValues(
                    applicationInstanceData,
                    @"/api/custom_values",
                    applicationInstanceData.Data.CustomPersonFollowUpValues);
                if (!postNewCustomPersonFollowUpValuesResult.Item1)
                {
                    return new Tuple<bool, string>(false, applicationInstanceData.SelectedLocalization.Translations[@"ErrorSyncError"]);
                }
            }
            catch
            {
                // TODO: error log
                return new Tuple<bool, string>(false, applicationInstanceData.SelectedLocalization.Translations[@"ErrorSyncError"]);
            }

            // successful sync
            applicationInstanceData.SerializedApplicationInstanceData.LastSync = DateTime.Now;
            applicationInstanceData.SaveSerializedApplicationInstanceData();
            return new Tuple<bool, string>(true, @"");
        }

        public static Tuple<bool, string> PostCustomValues<T>(ApplicationInstanceData applicationInstanceData, string apiPath, DbSet<T> data)
            where T : class, IObjectToJsonConvertible<T>, ISyncableAsChild<T>, ISyncableWithCustomFieldSecondParent<T>, new()
        {
            try
            {                
                foreach (var objectInstance in data.Where(a => a.GetExternalId() == null))
                {
                    objectInstance.SetMdpmsdbContext(applicationInstanceData.Data);
                    objectInstance.SetIds();
                    var objectJson = objectInstance.GetJsonFromObject();
                    var postSuccess = Helper.Rest.RestHelper.PerformRestPostRequestWithApiKey(
                        applicationInstanceData.SerializedApplicationInstanceData.Url,
                        apiPath,
                        applicationInstanceData.SerializedApplicationInstanceData.ApiKey,
                        objectJson);

                    if (postSuccess.Item1)
                    {
                        dynamic jsonResponseParse = JsonConvert.DeserializeObject(postSuccess.Item2);
                        if (jsonResponseParse.status.Value.ToString().Equals(@"success"))
                        {
                            objectInstance.SetLastUpdatedAt(jsonResponseParse.updated_at.Value);
                            objectInstance.SetCreatedAt(jsonResponseParse.created_at.Value);
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
                    var tContextSet = new T();
                    tContextSet.SetMdpmsdbContext(applicationInstanceData.Data);
                    T newObject = tContextSet.GetObjectFromJson(objectInstance);
                    idsInParent.Add((int)newObject.GetExternalId());                    
                    var query = data.Where(a => a.GetExternalId().Equals(newObject.GetExternalId()));
                    if (query.Any())
                    {
                        // sync non new records
                        var record = query.First();
                        record.SetMdpmsdbContext(applicationInstanceData.Data);

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
                                            record.SetLastUpdatedAt(jsonResponseParse.updated_at.Value);
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
                            objectInstance.SetLastUpdatedAt(jsonResponseParse.updated_at.Value);
                            objectInstance.SetCreatedAt(jsonResponseParse.created_at.Value);
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
                    var tContextSet = new T();
                    tContextSet.SetMdpmsdbContext(applicationInstanceData.Data);
                    T newObject = tContextSet.GetObjectFromJson(objectInstance); 
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
                            if (!parent.IncomeSources.Any(a => a.ExternalId == newObject.GetExternalId()))
                            {
                                parent.IncomeSources.Add(newObject as IncomeSource);
                            }
                        }
                        else if (typeof(T) == typeof(Person))
                        {
                            var parent = parentQuery.First() as Household;
                            if (parent.Members == null) parent.Members = new List<Person>();
                            if (!parent.Members.Any(a => a.ExternalId == newObject.GetExternalId()))
                            {
                                parent.Members.Add(newObject as Person);
                            }
                        }
                        else if (typeof(T) == typeof(PersonFollowUp))
                        {
                            var parent = parentQuery.First() as Person;
                            if (parent.PeopleFollowUps == null) parent.PeopleFollowUps = new List<PersonFollowUp>();
                            if (!parent.PeopleFollowUps.Any(a => a.ExternalId == newObject.GetExternalId()))
                            {
                                parent.PeopleFollowUps.Add(newObject as PersonFollowUp);
                            }
                        }
                        else if (typeof(T) == typeof(ServiceType))
                        {
                            var parent = parentQuery.First() as ServiceTypeCategory;
                            if (parent.ServiceTypes == null) parent.ServiceTypes = new List<ServiceType>();
                            if (!parent.ServiceTypes.Any(a => a.ExternalId == newObject.GetExternalId()))
                            {
                                parent.ServiceTypes.Add(newObject as ServiceType);
                            }
                        }
                        else if (typeof(T) == typeof(Service))
                        {
                            var parent = parentQuery.First() as ServiceType;
                            if (parent.Services == null) parent.Services = new List<Service>();
                            if (!parent.Services.Any(a => a.ExternalId == newObject.GetExternalId()))
                            {
                                parent.Services.Add(newObject as Service);
                            }
                        }
                        else if (typeof(T) == typeof(ServiceInstance))
                        {
                            var parent = parentQuery.First() as Person;
                            if (parent.ServiceInstances == null) parent.ServiceInstances = new List<ServiceInstance>();
                            if (!parent.ServiceInstances.Any(a => a.ExternalId == newObject.GetExternalId()))
                            {
                                parent.ServiceInstances.Add(newObject as ServiceInstance);
                            }
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
                    objectInstance.SetMdpmsdbContext(applicationInstanceData.Data);
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
                            objectInstance.SetLastUpdatedAt(jsonResponseParse.updated_at.Value);
                            objectInstance.SetCreatedAt(jsonResponseParse.created_at.Value);
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

        public static Tuple<bool, string> SyncCustomValueObject<T>(
            ApplicationInstanceData applicationInstanceData,
            bool allowAlreadySyncedUpdateToParent,
            string apiPath,
            DbSet<T> data,
            List<CustomField> customFields)
            where T : class, ISyncableAsChild<T>, ISyncableWithCustomFieldSecondParent<T>, new()
        {
            // parent objects
            try
            {
                // get existing objects from parent
                var existingObjectsJson = Helper.Rest.RestHelper.PerformRestGetRequestWithApiKey(
                    applicationInstanceData.SerializedApplicationInstanceData.Url,
                    apiPath,
                    applicationInstanceData.SerializedApplicationInstanceData.ApiKey);

                var idsInParent = new List<int>();
                dynamic objectParse = JsonConvert.DeserializeObject(existingObjectsJson);               
                foreach (var objectInstance in objectParse)
                {
                    dynamic json = objectInstance;
                    var customId = json.custom_field_id.Value;
                    if (customFields.Any(a => a.GetExternalId() == customId))
                    {
                        var tContextSet = new T();
                        tContextSet.SetMdpmsdbContext(applicationInstanceData.Data);
                        T newObject = tContextSet.GetObjectFromJson(objectInstance);
                        newObject.SetMdpmsdbContext(applicationInstanceData.Data);
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
                                                record.SetLastUpdatedAt(jsonResponseParse.updated_at.Value);
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
                            newObject.SetIds();
                            data.Add(newObject);
                        }
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
    }
}
