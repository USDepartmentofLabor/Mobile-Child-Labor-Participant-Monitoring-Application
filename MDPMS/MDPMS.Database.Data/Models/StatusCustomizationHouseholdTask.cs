﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MDPMS.Database.Data.Database;
using MDPMS.Database.Data.Models.Base;
using Newtonsoft.Json;

namespace MDPMS.Database.Data.Models
{
    public class StatusCustomizationHouseholdTask : EfBaseModel, ISyncable<StatusCustomizationHouseholdTask>
    {
        public string Code { get; set; }
        public string CanonicalName { get; set; }
        public string DisplayName { get; set; }

        public virtual ICollection<PersonHouseholdTask> PeopleHouseholdTasks { get; set; } = new List<PersonHouseholdTask>();
        public virtual ICollection<PersonFollowUpHouseholdTask> PeopleFollowUpHouseholdTasks { get; set; } = new List<PersonFollowUpHouseholdTask>();

        public StatusCustomizationHouseholdTask GetObjectFromJson(dynamic json)
        {
            return new StatusCustomizationHouseholdTask
            {
                ExternalId = json.id,
                CreatedAt = json.created_at,
                LastUpdatedAt = json.updated_at,
                SoftDeleted = false,
                Code = json.code,
                CanonicalName = json.canonical_name,
                DisplayName = json.display_name
            };
        }

        public string GetJsonFromObject()
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.None;
                writer.WriteStartObject();
                writer.WritePropertyName("code");
                writer.WriteValue(Code);
                writer.WritePropertyName("canonical_name");
                writer.WriteValue(CanonicalName);
                writer.WritePropertyName("display_name");
                writer.WriteValue(DisplayName);
                writer.WriteEndObject();
                writer.WriteEndObject();
            }
            return sw.ToString();
        }

        public void UpdateObject(StatusCustomizationHouseholdTask updateFrom)
        {
            LastUpdatedAt = updateFrom.LastUpdatedAt;
            Code = updateFrom.Code;
            CanonicalName = updateFrom.CanonicalName;
            DisplayName = updateFrom.DisplayName;
        }

        public bool GetObjectNeedsUpate(StatusCustomizationHouseholdTask checkUpdateFrom)
        {
            if (!Code.Equals(checkUpdateFrom.Code)) return true;
            if (!CanonicalName.Equals(checkUpdateFrom.CanonicalName)) return true;
            if (!DisplayName.Equals(checkUpdateFrom.DisplayName)) return true;
            return false;
        }

        public string GenerateUpdateJsonFromObject(StatusCustomizationHouseholdTask updateFrom)
        {
            // form the json (determine the fields that need to be updated)
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            var writer = new JsonTextWriter(sw) { Formatting = Formatting.None };
            writer.WriteStartObject();
            writer.WritePropertyName(@"income_source");
            writer.WriteStartObject();

            if (!Code.Equals(updateFrom.Code))
            {
                writer.WritePropertyName("code");
                writer.WriteValue(updateFrom.Code);
            }

            if (!CanonicalName.Equals(updateFrom.CanonicalName))
            {
                writer.WritePropertyName("canonical_name");
                writer.WriteValue(updateFrom.CanonicalName);
            }

            if (!DisplayName.Equals(updateFrom.DisplayName))
            {
                writer.WritePropertyName("display_name");
                writer.WriteValue(updateFrom.DisplayName);
            }

            writer.WriteEndObject();
            writer.WriteEndObject();
            return sw.ToString();
        }

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
    }
}
