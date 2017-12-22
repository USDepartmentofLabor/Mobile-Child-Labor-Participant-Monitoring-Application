using System;
using System.Linq;
using MDPMS.Shared.Models;

namespace MDPMS.Shared.Workers
{
    public static class SyncWorker
    {
        public static bool Sync(ApplicationInstanceData applicationInstanceData)
        {
            try
            {
                // get Households
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
