using System;
using System.Collections.Generic;
using System.Linq;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using Newtonsoft.Json;

namespace MDPMS.Shared.Workers
{
    public static class SyncWorker
    {
        public static Tuple<bool, string> Sync(ApplicationInstanceData applicationInstanceData, bool allowAlreadySyncedUpdateToParent)
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
             
            try
            {                                
                // get existing Households from parent
                var householdsJson = Helper.Rest.RestHelper.PerformRestGetRequestWithApiKey(
                    applicationInstanceData.SerializedApplicationInstanceData.Url,
                    @"/api/v1/households",
                    applicationInstanceData.SerializedApplicationInstanceData.ApiKey);

                // deserialize and sync Households
                var idsInParent = new List<int>();
                dynamic householdParse = JsonConvert.DeserializeObject(householdsJson);
                foreach (var household in householdParse)
                {                                   
                    Household newHousehold = new Household().GetObjectFromJson(household);
                    idsInParent.Add((int)newHousehold.ExternalId);
                    var householdDbRecord = applicationInstanceData.Data.Households.Where(a => a.ExternalId.Equals(newHousehold.ExternalId));
                    if (householdDbRecord.Any())
                    {
                        // sync non new records
                        var record = householdDbRecord.First();
                        
                        // the parent is newer so update the local
                        if (record.LastUpdatedAt < newHousehold.LastUpdatedAt) record.UpdateObject(newHousehold);
                        else if (record.LastUpdatedAt > newHousehold.LastUpdatedAt)
                        {
                            if (allowAlreadySyncedUpdateToParent)
                            {
                                // IF_SUPPORTED: the local is newer so update the parent with new info
                                if (record.GetObjectNeedsUpate(newHousehold))
                                {
                                    var putSuccess = Helper.Rest.RestHelper.PerformRestPutRequestWithApiKeyAndId(
                                        applicationInstanceData.SerializedApplicationInstanceData.Url,
                                        @"/api/v1/households",
                                        applicationInstanceData.SerializedApplicationInstanceData.ApiKey,
                                        record.GenerateUpdateJsonFromObject(newHousehold),
                                        record.ExternalId.ToString());
                                    if (putSuccess.Item1)
                                    {
                                        // set last updated at from JSON response
                                        dynamic jsonResponseParse = JsonConvert.DeserializeObject(putSuccess.Item2);
                                        if (jsonResponseParse.status.Value.ToString().Equals(@"success"))
                                        {
                                            household.LastUpdatedAt = ((DateTime)DateTime.Parse(jsonResponseParse.updated_at.Value)).ToUniversalTime();
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
                                    record.LastUpdatedAt = newHousehold.LastUpdatedAt;
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
                        applicationInstanceData.Data.Households.Add(newHousehold);
                    }
                }
                applicationInstanceData.Data.SaveChanges();

                // Delete local households not on parent, i.e. not in GET but has external id, requires parent suppoorting delete
                var idQuery = applicationInstanceData.Data.Households.Where(a => a.HasExternalId.Equals(true)).Select(a => (int)a.ExternalId).ToList();
                var toDeleteIds = idQuery.Except(idsInParent).ToList();
                foreach (var id in toDeleteIds)
                {
                    var rec = applicationInstanceData.Data.Households.Where(a => a.ExternalId.Equals(id));
                    applicationInstanceData.Data.Households.Remove(rec.First());
                }
                applicationInstanceData.Data.SaveChanges();
                
                // post new households last
                foreach (var household in applicationInstanceData.Data.Households.Where(a => a.ExternalId.HasValue == false))
                {
                    var householdJson = household.GetJsonFromObject();                    
                    var postSuccess = Helper.Rest.RestHelper.PerformRestPostRequestWithApiKey(
                        applicationInstanceData.SerializedApplicationInstanceData.Url,
                        @"/api/v1/households",
                        applicationInstanceData.SerializedApplicationInstanceData.ApiKey,
                        householdJson);

                    if (postSuccess.Item1)
                    {
                        // set last updated at from JSON response
                        dynamic jsonResponseParse = JsonConvert.DeserializeObject(postSuccess.Item2);                        
                        if (jsonResponseParse.status.Value.ToString().Equals(@"success"))
                        {
                            household.LastUpdatedAt = ((DateTime)DateTime.Parse(jsonResponseParse.updated_at.Value)).ToUniversalTime();
                            household.ExternalId = jsonResponseParse.id;
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
