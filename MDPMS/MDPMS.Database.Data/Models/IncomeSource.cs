using System;
using System.IO;
using System.Text;
using MDPMS.Database.Data.Database;
using MDPMS.Database.Data.Models.Base;
using Newtonsoft.Json;

namespace MDPMS.Database.Data.Models
{
    /// <summary>
    /// Household income source, note fields stored as string since they can be open ended answers (NOTE: at this time all that it required is at least 1 field is filled in)
    /// </summary>
    public class IncomeSource : EfBaseModel, ISyncableAsChild<IncomeSource>
    {
        /// <summary>
        /// Name of product or service (OPTIONAL)
        /// </summary>
        public string ProductServiceName { get; set; }

        /// <summary>
        /// Estimated volume produced (OPTIONAL)
        /// </summary>
        public int? EstimatedVolumeProduced { get; set; }

        /// <summary>
        /// Estimated volume sold (OPTIONAL)
        /// </summary>
        public int? EstimatedVolumeSold { get; set; }

        /// <summary>
        /// Unit of Measure (OPTIONAL)
        /// </summary>
        public string UnitOfMeasure { get; set; }

        /// <summary>
        /// Estimated Income (OPTIONAL)
        /// </summary>
        public decimal? EstimatedIncome { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        public string Currency { get; set; }

        public int? InternalParentId { get; set; } = null;

        public int? ExternalParentId { get; set; } = null;

        public int? GetExternalId()
        {
            return ExternalId;
        }

        public void SetExternalId(int? id)
        {
            ExternalId = id;
        }

        public DateTime? GetLastUpdatedAt()
        {
            return LastUpdatedAt;
        }

        public void SetLastUpdatedAt(DateTime? dateTime)
        {
            LastUpdatedAt = dateTime;
        }

        public DateTime? GetCreatedAt()
        {
            return CreatedAt;
        }

        public void SetCreatedAt(DateTime? dateTime)
        {
            CreatedAt = dateTime;
        }

        public int? GetInternalId()
        {
            return InternalId;
        }

        public void SetMdpmsdbContext(MDPMSDatabaseContext context)
        {
        }

        public int? GetExternalParentId()
        {
            return ExternalParentId;
        }

        public void SetExternalParentId(int? id)
        {
            ExternalParentId = id;
        }

        public int? GetInternalParentId()
        {
            return InternalParentId;
        }

        public void SetInternalParentId(int? id)
        {
            InternalParentId = id;
        }

        public IncomeSource GetObjectFromJson(dynamic json)
        {
            return new IncomeSource
            {
                ExternalId = json.id,
                CreatedAt = json.created_at,
                LastUpdatedAt = json.updated_at,
                SoftDeleted = false,
                ProductServiceName = json.name ?? @"",
                EstimatedVolumeProduced = json.estimated_volume_produced ?? null,
                EstimatedVolumeSold = json.estimated_volume_sold ?? null,
                UnitOfMeasure = json.unit_of_measure ?? @"",
                EstimatedIncome = json.estimated_income ?? null,
                Currency = json.currency ?? @"",
                ExternalParentId = json.household_id
            };
        }

        public Tuple<int, IncomeSource> GetObjectFromJsonWithParentId(dynamic json, string parentIdPropertyName)
        {
            int id = json.parentIdPropertyName;
            IncomeSource incomeSource = GetObjectFromJson(json);
            return new Tuple<int, IncomeSource>(id, incomeSource);
        }

        public string GetJsonFromObject()
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.None;
                writer.WriteStartObject();
                writer.WritePropertyName(@"income_source");
                writer.WriteStartObject();
                writer.WritePropertyName("name");
                writer.WriteValue(ProductServiceName ?? @"");

                if (EstimatedVolumeProduced != null)
                {
                    writer.WritePropertyName("estimated_volume_produced");
                    writer.WriteValue(EstimatedVolumeProduced ?? null);
                }

                if (EstimatedVolumeSold != null)
                {
                    writer.WritePropertyName("estimated_volume_sold");
                    writer.WriteValue(EstimatedVolumeSold ?? null);
                }

                if (EstimatedIncome != null)
                {
                    writer.WritePropertyName("estimated_income");
                    writer.WriteValue(EstimatedIncome ?? null);
                }

                if (UnitOfMeasure != null | UnitOfMeasure != @"")
                {
                    writer.WritePropertyName("unit_of_measure");
                    writer.WriteValue(UnitOfMeasure);
                }

                if (Currency != @"" | Currency != @"")
                {
                    writer.WritePropertyName("currency");
                    writer.WriteValue(Currency);
                }

                writer.WritePropertyName("household_id");
                writer.WriteValue(ExternalParentId);
                writer.WriteEndObject();
                writer.WriteEndObject();
            }
            return sw.ToString();
        }

        public void UpdateObject(IncomeSource updateFrom)
        {
            LastUpdatedAt = updateFrom.LastUpdatedAt;
            ProductServiceName = updateFrom.ProductServiceName;
            EstimatedVolumeProduced = updateFrom.EstimatedVolumeProduced;
            EstimatedVolumeSold = updateFrom.EstimatedVolumeSold;
            UnitOfMeasure = updateFrom.UnitOfMeasure;
            EstimatedIncome = updateFrom.EstimatedIncome;
            Currency = updateFrom.Currency;
            ExternalParentId = updateFrom.ExternalParentId;
        }

        public bool GetObjectNeedsUpate(IncomeSource checkUpdateFrom)
        {
            if (!ProductServiceName.Equals(checkUpdateFrom.ProductServiceName)) return true;
            if (!EstimatedVolumeProduced.Equals(checkUpdateFrom.EstimatedVolumeProduced)) return true;
            if (!EstimatedVolumeSold.Equals(checkUpdateFrom.EstimatedVolumeSold)) return true;
            if (!UnitOfMeasure.Equals(checkUpdateFrom.UnitOfMeasure)) return true;
            if (!EstimatedIncome.Equals(checkUpdateFrom.EstimatedIncome)) return true;
            if (!Currency.Equals(checkUpdateFrom.Currency)) return true;
            if (!ExternalParentId.Equals(checkUpdateFrom.ExternalParentId)) return true;
            return false;
        }

        public string GenerateUpdateJsonFromObject(IncomeSource updateFrom)
        {
            // form the json (determine the fields that need to be updated)
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            var writer = new JsonTextWriter(sw) { Formatting = Formatting.None };
            writer.WriteStartObject();
            writer.WritePropertyName(@"income_source");
            writer.WriteStartObject();

            if (!ProductServiceName.Equals(updateFrom.ProductServiceName))
            {
                writer.WritePropertyName("name");
                writer.WriteValue(updateFrom.ProductServiceName ?? @"");
            }

            if (!EstimatedVolumeProduced.Equals(updateFrom.EstimatedVolumeProduced))
            {
                writer.WritePropertyName("estimated_volume_produced");
                writer.WriteValue(updateFrom.EstimatedVolumeProduced ?? null);
            }

            if (!EstimatedVolumeSold.Equals(updateFrom.EstimatedVolumeSold))
            {
                writer.WritePropertyName("estimated_volume_sold");
                writer.WriteValue(updateFrom.EstimatedVolumeSold ?? null);
            }

            if (!UnitOfMeasure.Equals(updateFrom.UnitOfMeasure))
            {
                writer.WritePropertyName("unit_of_measure");
                writer.WriteValue(updateFrom.UnitOfMeasure ?? @"");
            }

            if (!EstimatedIncome.Equals(updateFrom.EstimatedIncome))
            {
                writer.WritePropertyName("estimated_income");
                writer.WriteValue(updateFrom.EstimatedIncome ?? null);
            }

            if (!Currency.Equals(updateFrom.Currency))
            {
                writer.WritePropertyName("currency");
                writer.WriteValue(updateFrom.Currency ?? @"");
            }

            if (!ExternalParentId.Equals(updateFrom.ExternalParentId))
            {
                writer.WritePropertyName("household_id");
                writer.WriteValue(updateFrom.ExternalParentId);
            }

            writer.WriteEndObject();
            writer.WriteEndObject();
            return sw.ToString();
        }        
    }
}
