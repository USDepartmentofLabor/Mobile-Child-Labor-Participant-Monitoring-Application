using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MDPMS.Database.Data.Models;
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
                var idsInParent = new List<int>();
                dynamic householdParse = JsonConvert.DeserializeObject(householdsJson);
                foreach (var household in householdParse)
                {                                        
                    Household newHousehold = GetHouseholdFromJson(household);
                    idsInParent.Add((int)newHousehold.ExternalId);
                    var householdDbRecord = applicationInstanceData.Data.Households.Where(a => a.ExternalId.Equals(newHousehold.ExternalId));
                    if (householdDbRecord.Any())
                    {
                        // sync non new records
                        var record = householdDbRecord.First();
                        
                        // the parent is newer so update the local
                        if (record.LastUpdatedAt < newHousehold.LastUpdatedAt) UpdateHouseholdRecord(record, newHousehold);
                        else if (record.LastUpdatedAt > newHousehold.LastUpdatedAt)
                        {
                            var updateParentIsAllowed = true;
                            if (updateParentIsAllowed)
                            {
                                // IF_SUPPORTED: the local is newer so update the parent with new info
                                if (GetHouseholdNeedsUpdate(newHousehold, record))
                                {
                                    var putSuccess = Helper.Rest.RestHelper.PerformRestPutRequestWithApiKeyAndId(
                                        applicationInstanceData.SerializedApplicationInstanceData.Url,
                                        @"/api/v1/households",
                                        applicationInstanceData.SerializedApplicationInstanceData.ApiKey,
                                        GenerateUpdateJsonForHousehold(newHousehold, record),
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
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        // TODO: error log    
                                        return false;
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
                                return false;
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
                    var householdJson = GetJsonFromHousehold(household);                    
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
                            return false;
                        }
                    }
                    else
                    {
                        // TODO: error log    
                        return false;
                    }
                }
                applicationInstanceData.Data.SaveChanges();
            }
            catch(Exception e)
            {
                // TODO: error log
                return false;
            }
            return true;
        }

        public static Household GetHouseholdFromJson(dynamic householdJson)
        {            
            return new Household
            {
                ExternalId = householdJson.id,
                CreatedAt = householdJson.created_at,
                LastUpdatedAt = householdJson.updated_at,
                SoftDeleted = false,
                HouseholdName = householdJson.name,
                IntakeDate = householdJson.intake_date,
                AddressLine1 = householdJson.address_line_1,
                AddressLine2 = householdJson.address_line_2,
                PostalCode = householdJson.postal_code,
                DependentLocality = householdJson.dependent_locality,
                Locality = householdJson.locality,
                AdminvArea = householdJson.adminv_area,
                DependentAdminvArea = householdJson.dependent_adminv_area,
                Country = householdJson.country,
                AddressInfo = householdJson.address_info
            };
        }

        public static string GetJsonFromHousehold(Household household)
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
            return sw.ToString();
        }

        public static void UpdateHouseholdRecord(Household recordToUpdate, Household updatedHousehold)
        {
            recordToUpdate.LastUpdatedAt = updatedHousehold.LastUpdatedAt;
            recordToUpdate.HouseholdName = updatedHousehold.HouseholdName;
            recordToUpdate.IntakeDate = updatedHousehold.IntakeDate;
            recordToUpdate.AddressLine1 = updatedHousehold.AddressLine1;
            recordToUpdate.AddressLine2 = updatedHousehold.AddressLine1;
            recordToUpdate.PostalCode = updatedHousehold.PostalCode;
            recordToUpdate.DependentLocality = updatedHousehold.DependentLocality;
            recordToUpdate.Locality = updatedHousehold.Locality;
            recordToUpdate.AdminvArea = updatedHousehold.AdminvArea;
            recordToUpdate.DependentAdminvArea = updatedHousehold.DependentAdminvArea;
            recordToUpdate.Country = updatedHousehold.Country;
            recordToUpdate.AddressInfo = updatedHousehold.AddressInfo;
        }

        public static bool GetHouseholdNeedsUpdate(Household older, Household newer)
        {
            if (!older.HouseholdName.Equals(newer.HouseholdName)) return true;
            if (!older.IntakeDate.Equals(newer.IntakeDate)) return true;
            if (!older.AddressLine1.Equals(newer.AddressLine1)) return true;
            if (!older.AddressLine2.Equals(newer.AddressLine2)) return true;
            if (!older.PostalCode.Equals(newer.PostalCode)) return true;
            if (!older.DependentLocality.Equals(newer.DependentLocality)) return true;
            if (!older.Locality.Equals(newer.Locality)) return true;
            if (!older.AdminvArea.Equals(newer.AdminvArea)) return true;
            if (!older.DependentAdminvArea.Equals(newer.DependentAdminvArea)) return true;
            if (!older.Country.Equals(newer.Country)) return true;
            if (!older.AddressInfo.Equals(newer.AddressInfo)) return true;
            return false;
        }

        public static string GenerateUpdateJsonForHousehold(Household older, Household newer)
        {
            // form the json (determine the fields that need to be updated)
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            var writer = new JsonTextWriter(sw) {Formatting = Formatting.None};
            writer.WriteStartObject();
            writer.WritePropertyName(@"household");
            writer.WriteStartObject();
            
            if (!older.HouseholdName.Equals(newer.HouseholdName))
            {
                writer.WritePropertyName("name");
                writer.WriteValue(newer.HouseholdName);
            }

            if (!older.IntakeDate.Equals(newer.IntakeDate))
            {
                writer.WritePropertyName("intake_date");
                writer.WriteValue(newer.IntakeDate.ToString("yyyy-MM-dd"));
            }

            if (!older.AddressLine1.Equals(newer.AddressLine1))
            {
                writer.WritePropertyName("address_line_1");
                writer.WriteValue(newer.AddressLine1);
            }

            if (!older.AddressLine2.Equals(newer.AddressLine2))
            {
                writer.WritePropertyName("address_line_2");
                writer.WriteValue(newer.AddressLine2);
            }

            if (!older.PostalCode.Equals(newer.PostalCode))
            {
                writer.WritePropertyName("postal_code");
                writer.WriteValue(newer.PostalCode);
            }

            if (!older.DependentLocality.Equals(newer.DependentLocality))
            {
                writer.WritePropertyName("dependent_locality");
                writer.WriteValue(newer.DependentLocality);
            }

            if (!older.Locality.Equals(newer.Locality))
            {
                writer.WritePropertyName("locality");
                writer.WriteValue(newer.Locality);
            }

            if (!older.AdminvArea.Equals(newer.AdminvArea))
            {
                writer.WritePropertyName("adminv_area");
                writer.WriteValue(newer.AdminvArea);
            }

            if (!older.DependentAdminvArea.Equals(newer.DependentAdminvArea))
            {
                writer.WritePropertyName("dependent_adminv_area");
                writer.WriteValue(newer.DependentAdminvArea);
            }

            if (!older.Country.Equals(newer.Country))
            {
                writer.WritePropertyName("country");
                writer.WriteValue(newer.Country);
            }

            if (!older.AddressInfo.Equals(newer.AddressInfo))
            {
                writer.WritePropertyName("address_info");
                writer.WriteValue(newer.AddressInfo);
            }
            
            writer.WriteEndObject();
            writer.WriteEndObject();            
            return sw.ToString();
        }
    }
}
