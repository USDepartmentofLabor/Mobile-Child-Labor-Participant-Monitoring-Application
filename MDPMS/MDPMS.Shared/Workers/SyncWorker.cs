using System;
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
                dynamic householdParse = JsonConvert.DeserializeObject(householdsJson);
                foreach (var household in householdParse)
                {
                    var newHousehold = new Database.Data.Models.Household
                    {
                        ExternalId = household.id,                        
                        CreatedAt = household.created_at,
                        LastUpdatedAt = household.updated_at,
                        SoftDeleted = false,
                        HouseholdName = household.name,
                        IntakeDate = household.intake_date,
                        AddressLine1 = household.address_line_1,
                        AddressLine2 = household.address_line_2,
                        PostalCode = household.postal_code,
                        DependentLocality = household.dependent_locality,
                        Locality = household.locality,
                        AdminvArea = household.adminv_area,
                        DependentAdminvArea = household.dependent_adminv_area,
                        Country = household.country,
                        AddressInfo = household.address_info
                    };

                    // compare against database and use updated_at vs. LastUpdateAt
                    var householdDbRecord = applicationInstanceData.Data.Households.Where(a => a.ExternalId.Equals(newHousehold.ExternalId));
                    if (householdDbRecord.Any())
                    {
                        // sync non new records
                        if (householdDbRecord.First().LastUpdatedAt < newHousehold.LastUpdatedAt)
                        {
                            // the parent is newer so replace the local TODO: just update the fields to keep the internal id
                            applicationInstanceData.Data.Households.Remove(householdDbRecord.First());
                            applicationInstanceData.Data.Households.Add(newHousehold);
                        }
                        // if = then assume same, should check fields or superstitious?
                        //else
                        //{
                        //    //else, the local is newer so update the parent with new info, TODO: UPDATE, NOTE: NOT_YET_SUPPORTED
                        //    var record = householdDbRecord.First();
                        //    // form the json (determine the fields that need to be updated)
                        //    var sb = new StringBuilder();
                        //    var sw = new StringWriter(sb);
                        //    var writer = new JsonTextWriter(sw);
                        //    writer.Formatting = Formatting.None;
                        //    writer.WriteStartObject();
                        //    writer.WritePropertyName(@"household");
                        //    writer.WriteStartObject();                            
                        //    if (!record.HouseholdName.Equals(newHousehold.HouseholdName))
                        //    {
                        //        writer.WritePropertyName("name");
                        //        writer.WriteValue(record.HouseholdName);
                        //    }
                        //    // TODO: other properties
                        //    writer.WriteEndObject();
                        //    writer.WriteEndObject();
                        //    var putSuccess = Helper.Rest.RestHelper.PerformRestPutRequestWithApiKeyAndId(
                        //        applicationInstanceData.SerializedApplicationInstanceData.Url,
                        //        @"/api/v1/households",
                        //        applicationInstanceData.SerializedApplicationInstanceData.ApiKey,
                        //        sw.ToString(),
                        //        record.ExternalId.ToString());
                        //    // TODO: handle error on POST                        
                        //}
                    }
                    else
                    {
                        // add it locally
                        applicationInstanceData.Data.Households.Add(newHousehold);
                    }
                }
                applicationInstanceData.Data.SaveChanges();

                // TODO: delete local households not on parent, i.e. not in GET but has external id, requires parent suppoorting delete

                // post new households last
                foreach (var household in applicationInstanceData.Data.Households.Where(a => a.ExternalId.HasValue == false))
                {
                    var sb = new StringBuilder();
                    var sw = new StringWriter(sb);
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        writer.Formatting = Formatting.None;
                        writer.WriteStartObject();
                        writer.WritePropertyName(@"household");
                        writer.WriteStartObject();
                        writer.WritePropertyName("name");
                        writer.WriteValue(household.HouseholdName);
                        writer.WritePropertyName("intake_date");
                        writer.WriteValue(household.IntakeDate.ToString("yyyy-MM-dd"));
                        writer.WritePropertyName("address_line_1");
                        writer.WriteValue(household.AddressLine1);
                        writer.WritePropertyName("address_line_2");
                        writer.WriteValue(household.AddressLine2);
                        writer.WritePropertyName("postal_code");
                        writer.WriteValue(household.PostalCode);
                        writer.WritePropertyName("dependent_locality");
                        writer.WriteValue(household.DependentLocality);
                        writer.WritePropertyName("locality");
                        writer.WriteValue(household.Locality);
                        writer.WritePropertyName("adminv_area");
                        writer.WriteValue(household.AdminvArea);
                        writer.WritePropertyName("dependent_adminv_area");
                        writer.WriteValue(household.DependentAdminvArea);
                        writer.WritePropertyName("country");
                        writer.WriteValue(household.Country);
                        writer.WritePropertyName("address_info");
                        writer.WriteValue(household.AddressInfo);                        
                        writer.WriteEndObject();
                        writer.WriteEndObject();
                    }
                    var postSuccess = Helper.Rest.RestHelper.PerformRestPostRequestWithApiKey(
                        applicationInstanceData.SerializedApplicationInstanceData.Url,
                        @"/api/v1/households",
                        applicationInstanceData.SerializedApplicationInstanceData.ApiKey,
                        sw.ToString());

                    if (postSuccess.Item1)
                    {
                        // set last updated at from JSON response
                        dynamic jsonResponseParse = JsonConvert.DeserializeObject(postSuccess.Item2);                        
                        if (jsonResponseParse.status.Value.ToString().Equals(@"success"))
                        {
                            household.LastUpdatedAt = ((DateTime)DateTime.Parse(jsonResponseParse.updated_at.Value)).ToUniversalTime();
                            household.ExternalId = jsonResponseParse.id;
                        }
                        // else // TODO: error log                        
                    }
                    // else // TODO: error log                                     
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
