using System;
using System.IO;
using System.Text;
using MDPMS.Database.Data.Models.Base;
using Newtonsoft.Json;

namespace MDPMS.Database.Data.Models
{
    /// <summary>
    /// Household income source, note fields stored as string since they can be open ended answers
    /// </summary>
    public class IncomeSource : EfBaseModel, ISyncable<IncomeSource>, ISyncableAsChild<IncomeSource>
    {
        /// <summary>
        /// Name of product or service
        /// </summary>
        public string ProductServiceName { get; set; }

        /// <summary>
        /// Estimated volume produced
        /// </summary>
        public int? EstimatedVolumeProduced { get; set; }

        /// <summary>
        /// Estimated volume sold
        /// </summary>
        public int? EstimatedVolumeSold { get; set; }

        /// <summary>
        /// Unit of Measure
        /// </summary>
        public string UnitOfMeasure { get; set; }

        /// <summary>
        /// Estimated Income
        /// </summary>
        public decimal? EstimatedIncome { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        public string Currency { get; set; }

        public IncomeSource GetObjectFromJson(dynamic json)
        {
            return new IncomeSource
            {
                ExternalId = json.id,
                CreatedAt = json.created_at,
                LastUpdatedAt = json.updated_at,
                SoftDeleted = false,
                ProductServiceName = json.name,
                EstimatedVolumeProduced = json.estimated_volume_produced,
                EstimatedVolumeSold = json.estimated_volume_sold,
                UnitOfMeasure = json.unit_of_measure,
                EstimatedIncome = json.estimated_income,
                Currency = json.currency
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
                writer.WriteValue(ProductServiceName);
                writer.WritePropertyName("estimated_volume_produced");
                writer.WriteValue(EstimatedVolumeProduced);
                writer.WritePropertyName("estimated_volume_sold");
                writer.WriteValue(EstimatedVolumeSold);
                writer.WritePropertyName("unit_of_measure");
                writer.WriteValue(UnitOfMeasure);
                writer.WritePropertyName("estimated_income");
                writer.WriteValue(EstimatedIncome);
                writer.WritePropertyName("currency");
                writer.WriteValue(Currency);
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
        }

        public bool GetObjectNeedsUpate(IncomeSource checkUpdateFrom)
        {
            if (!ProductServiceName.Equals(checkUpdateFrom.ProductServiceName)) return true;
            if (!EstimatedVolumeProduced.Equals(checkUpdateFrom.EstimatedVolumeProduced)) return true;
            if (!EstimatedVolumeSold.Equals(checkUpdateFrom.EstimatedVolumeSold)) return true;
            if (!UnitOfMeasure.Equals(checkUpdateFrom.UnitOfMeasure)) return true;
            if (!EstimatedIncome.Equals(checkUpdateFrom.EstimatedIncome)) return true;
            if (!Currency.Equals(checkUpdateFrom.Currency)) return true;
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
                writer.WriteValue(updateFrom.ProductServiceName);
            }

            if (!EstimatedVolumeProduced.Equals(updateFrom.EstimatedVolumeProduced))
            {
                writer.WritePropertyName("estimated_volume_produced");
                writer.WriteValue(updateFrom.EstimatedVolumeProduced);
            }

            if (!EstimatedVolumeSold.Equals(updateFrom.EstimatedVolumeSold))
            {
                writer.WritePropertyName("estimated_volume_sold");
                writer.WriteValue(updateFrom.EstimatedVolumeSold);
            }

            if (!UnitOfMeasure.Equals(updateFrom.UnitOfMeasure))
            {
                writer.WritePropertyName("unit_of_measure");
                writer.WriteValue(updateFrom.UnitOfMeasure);
            }

            if (!EstimatedIncome.Equals(updateFrom.EstimatedIncome))
            {
                writer.WritePropertyName("estimated_income");
                writer.WriteValue(updateFrom.EstimatedIncome);
            }

            if (!Currency.Equals(updateFrom.Currency))
            {
                writer.WritePropertyName("currency");
                writer.WriteValue(updateFrom.Currency);
            }

            writer.WriteEndObject();
            writer.WriteEndObject();
            return sw.ToString();
        }        
    }
}
