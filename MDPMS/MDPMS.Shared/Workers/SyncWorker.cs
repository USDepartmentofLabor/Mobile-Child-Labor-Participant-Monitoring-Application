using System.IO;
using System.Linq;
using System.Text;
using MDPMS.Shared.Models;
using Newtonsoft.Json;

namespace MDPMS.Shared.Workers
{
    public static class SyncWorker
    {
        public static bool Sync(ApplicationInstanceData applicationInstanceData)
        {
            /*
             SYNC STRATEGY
             =============
             * SUMMARY => Record existence and update age
                * For now, mobile can not edit households after it is synced to parent
                * Use integer id from parent to identify records
                * Mobile will have internal and external (parent DPMS) ids
                * Record age will be determined by updated_at datetime
                              
               Mobile                                               Parent_DPMS
               ======                                               ===========
             * GetRecordFromParent <------------------------------- Exists on parent but not on mobile
             * Exists on Mobile but not on parent ----------------> GetRecordFromMobile
             * Older on mobile than parent <--UpdateMobileRecord--- Newer on parent than mobile
             * Newer on mobile than parent <--NotYetSupported------ Older on parent than mobile             (Currently invalid, would be supported by updating DPMS record, also resolve conflicts?)
                            
             REASONING =>

             Exists
             ======
             Mobile | Parent
             ---------------
             0      |   0       * N/A
             0      |   1       * Exists only on parent, mobile get from parent
             1      |   0       * Exists only on mobile, send to parent
             1      |   1       * Exists on both, see Record_Age table ==>
 
            Record_Age
            ==========
            Mobile | Parent
            ---------------
            0      |   0        * Same age, no action
            0      |   1        * Newer on parent, update mobile
            1      |   0        * Newer on mobile, update parent    (NOT_YET_SUPPORTED)
            1      |   1        * Same age, no action
 
            NOTE ON Newer on mobile:
            For now we can send those to the parent and mark as NEEDS_TO_BE_MANUALLY_RESOLVED
             */
             
            try
            {
                // post new households first
                foreach (var household in applicationInstanceData.Data.Households.Where(a => a.ExternalId.HasValue == false))
                {
                    var sb = new StringBuilder();
                    var sw = new StringWriter(sb);
                    using(JsonWriter writer = new JsonTextWriter(sw))
                    {
                        writer.Formatting = Formatting.None;                        
                        writer.WriteStartObject();
                        writer.WritePropertyName(@"household");
                        writer.WriteStartObject();                      
                        writer.WritePropertyName("name");
                        writer.WriteValue(household.HouseholdName);
                        writer.WritePropertyName("intake_date");
                        writer.WriteValue(household.IntakeDate.ToString("yyyy-MM-dd"));
                        writer.WriteEndObject();
                        writer.WriteEndObject();
                    }
                    var postSuccess = Helper.Rest.RestHelper.PerformRestPostRequestWithApiKey(
                        applicationInstanceData.SerializedApplicationInstanceData.Url,
                        @"/api/v1/households",
                        applicationInstanceData.SerializedApplicationInstanceData.ApiKey,
                        sw.ToString());
                    // TODO: handle error on POST
                }
                
                // get/update existing Households
                var householdsJson = Helper.Rest.RestHelper.PerformRestGetRequestWithApiKey(
                    applicationInstanceData.SerializedApplicationInstanceData.Url, @"/api/v1/households",
                    applicationInstanceData.SerializedApplicationInstanceData.ApiKey);

                // deserialize and sync Households
                dynamic householdParse = Newtonsoft.Json.JsonConvert.DeserializeObject(householdsJson);
                foreach (var household in householdParse)
                {
                    var newHousehold = new Database.Data.Models.Household
                    {
                        ExternalId = household.id,
                        HouseholdName = household.name,
                        CreatedAt = household.created_at,
                        LastUpdatedAt = household.updated_at
                    };

                    // compare against database and use updated_at vs. LastUpdateAt
                    var householdDbRecord = applicationInstanceData.Data.Households.Where(a => a.ExternalId.Equals(newHousehold.ExternalId));
                    if (householdDbRecord.Any())
                    {
                        // TODO: sync non new records
                        if (householdDbRecord.First().LastUpdatedAt <= newHousehold.LastUpdatedAt)
                        {
                            // the parent is newer or equal so replace the local
                            applicationInstanceData.Data.Households.Remove(householdDbRecord.First());
                            applicationInstanceData.Data.Households.Add(newHousehold);
                        }
                        else
                        {
                            // the local is newer so update the parent with new info
                            // TODO: UPDATE
                        }
                    }
                    else
                    {
                        // add it locally
                        applicationInstanceData.Data.Households.Add(newHousehold);
                    }
                }
                applicationInstanceData.Data.SaveChanges();
            }
            catch
            {
                // TODO: error log
                return false;
            }
            return true;
        }
    }
}
