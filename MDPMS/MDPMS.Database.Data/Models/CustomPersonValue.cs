using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using MDPMS.Database.Data.Database;
using MDPMS.Database.Data.Models.Base;
using Newtonsoft.Json;

namespace MDPMS.Database.Data.Models
{
    public class CustomPersonValue : EfBaseModel, ISyncableAsChild<CustomPersonValue>, ISyncableWithCustomFieldSecondParent<CustomPersonValue>
    {
        public CustomField CustomField { get; set; }
        public string Value { get; set; }
        public Person Person { get; set; }

        // NOTE: on custom values syncables, parent is household, person, followUp
        // and ISyncableWithCustomFieldSecondParent<T> parent is the CustomField

        public int? GetInternalId()
        {
            return InternalId;
        }

        public int? GetExternalId()
        {
            return ExternalId;
        }

        public void SetExternalId(int? id)
        {
            ExternalId = id;
        }

        public int? InternalParentId { get; set; } = null; // Person

        public int? GetInternalParentId()
        {
            return InternalParentId;
        }

        public void SetInternalParentId(int? id)
        {
            InternalParentId = id;
        }

        public int? ExternalParentId { get; set; } = null; // Person

        public void SetExternalParentId(int? id)
        {
            ExternalParentId = id;
        }

        public int? GetExternalParentId()
        {
            return ExternalParentId;
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

        [NotMapped]
        public MDPMSDatabaseContext MdpmsDatabaseContext { get; set; }

        public void SetMdpmsdbContext(MDPMSDatabaseContext context)
        {
            MdpmsDatabaseContext = context;
        }

        // Custom Field Parent Internal and External Ids

        public int? InternalCustomFieldId { get; set; } = null; // CustomField

        public int? GetInternalCustomFieldId()
        {
            return InternalCustomFieldId;
        }

        public void SetInternalCustomFieldId(int? id)
        {
            InternalCustomFieldId = id;
        }

        public int? ExternalCustomFieldId { get; set; } = null; // CustomField

        public int? GetExternalCustomFieldId()
        {
            return ExternalCustomFieldId;
        }

        public void SetExternalCustomFieldId(int? id)
        {
            ExternalCustomFieldId = id;
        }

        public string GetJsonFromObject()
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.None;
                writer.WriteStartObject();
                writer.WritePropertyName(@"custom_value");
                writer.WriteStartObject();
                writer.WritePropertyName("custom_field_id");
                writer.WriteValue(CustomField.ExternalId);
                writer.WritePropertyName("value_text");
                writer.WriteValue(Value);
                writer.WritePropertyName("model_id");
                writer.WriteValue(Person.ExternalId);
                writer.WriteEndObject();
                writer.WriteEndObject();
            }
            return sw.ToString();
        }

        public CustomPersonValue GetObjectFromJson(dynamic json)
        {
            var customValue = new CustomPersonValue
            {
                ExternalId = json.id,
                CreatedAt = json.created_at,
                LastUpdatedAt = json.updated_at,
                SoftDeleted = false,
                ExternalParentId = json.model_id,
                ExternalCustomFieldId = json.custom_field_id,
                Value = json.value_text
            };
            return customValue;
        }

        public bool GetObjectNeedsUpate(CustomPersonValue checkUpdateFrom)
        {
            if (!Value.Equals(checkUpdateFrom.Value)) return true;
            if (!ExternalCustomFieldId.Equals(checkUpdateFrom.ExternalParentId)) return true;
            if (!ExternalParentId.Equals(checkUpdateFrom.ExternalCustomFieldId)) return true;
            return false;
        }

        public void UpdateObject(CustomPersonValue updateFrom)
        {
            Value = updateFrom.Value;
            ExternalCustomFieldId = updateFrom.ExternalCustomFieldId;
            ExternalParentId = updateFrom.ExternalParentId;
        }

        public Tuple<int, CustomPersonValue> GetObjectFromJsonWithParentId(dynamic json, string parentIdPropertyName)
        {
            int id = json.parentIdPropertyName;
            CustomPersonValue value = GetObjectFromJson(json);
            return new Tuple<int, CustomPersonValue>(id, value);
        }

        public string GenerateUpdateJsonFromObject(CustomPersonValue updateFrom)
        {
            // form the json (determine the fields that need to be updated)
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            var writer = new JsonTextWriter(sw) { Formatting = Formatting.None };
            writer.WriteStartObject();
            writer.WritePropertyName(@"custom_value");
            writer.WriteStartObject();

            if (!Value.Equals(updateFrom.Value))
            {
                writer.WritePropertyName("name");
                writer.WriteValue(updateFrom.Value ?? @"");
            }

            if (!ExternalCustomFieldId.Equals(updateFrom.ExternalCustomFieldId))
            {
                writer.WritePropertyName("custom_field_id");
                writer.WriteValue(updateFrom.ExternalCustomFieldId ?? null);
            }

            if (!ExternalParentId.Equals(updateFrom.ExternalParentId))
            {
                writer.WritePropertyName("model_id");
                writer.WriteValue(updateFrom.ExternalParentId ?? null);
            }

            writer.WriteEndObject();
            writer.WriteEndObject();
            return sw.ToString();
        }

        public void SetIds()
        {
            if (MdpmsDatabaseContext == null) return;
            if (Person == null)
            {
                var parentModelQuery = MdpmsDatabaseContext.People.Where(a => a.GetExternalId() == ExternalParentId);
                if (parentModelQuery.Any()) Person = parentModelQuery.First();
            }

            if (CustomField == null)
            {
                var parentCustomFieldQuery = MdpmsDatabaseContext.CustomFields.Where(a => a.GetExternalId() == ExternalCustomFieldId);
                if (parentCustomFieldQuery.Any()) CustomField = parentCustomFieldQuery.First();
            }
        }
    }
}
