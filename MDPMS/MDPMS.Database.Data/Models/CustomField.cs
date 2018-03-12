using System;
using System.Collections.Generic;
using MDPMS.Database.Data.Database;
using MDPMS.Database.Data.Models.Base;

namespace MDPMS.Database.Data.Models
{
    /// <summary>
    /// Custom fields for households and household members
    /// Used on household intakes, household member intakes, and household member follow ups
    /// </summary>
    public class CustomField : EfBaseModel, ISyncable<CustomField>
    {
        /// <summary>
        /// Name of the custom field
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of the custom field from the parent DPMS
        /// See types below
        /// { API_Value, DPMS_UI_Name, C#_Type, HasArrayOfStringValues}:
        /// {
        ///     { text, Short Text, string, false }
        ///     { textarea, Long Text, string, false }
        ///     { check_box, Check Box, string, true }
        ///     { radio_button, Radio Button, string, true }
        ///     { select, Selection Field, string, true }
        ///     { number, Numeric Field, double, false }
        ///     { date, Date Field, DateTime, false }
        ///     { rank_list, Rank List, string, true }
        /// }
        /// </summary>
        public string FieldType { get; set; }

        /// <summary>
        /// Model type of custom field:
        /// { Household, Person }
        /// </summary>
        public string ModelType { get; set; }

        /// <summary>
        /// Help text for the custom field
        // </summary>
        public string HelpText { get; set; }

        /// <summary>
        /// Sort order
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Selections are choices for field values stored as a string, use GetOptions() to split string
        /// </summary>
        public string Options { get; set; }

        public List<string> GetOptions()
        {
            var rtn = new List<string>();
            if (Options == null || Options.Equals(String.Empty)) return rtn;
            var selectionParse = Options.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (var selection in selectionParse)
            {
                if (!selection.Equals(string.Empty)) rtn.Add(selection);
            }
            return rtn;
        }

        public List<CustomHouseholdValue> CustomHouseholdValues { get; set; } = new List<CustomHouseholdValue>();
        public List<CustomPersonValue> CustomPersonValues { get; set; } = new List<CustomPersonValue>();
        public List<CustomPersonFollowUpValue> CustomPersonFollowUpValues { get; set; } = new List<CustomPersonFollowUpValue>();

        // NOT_CURRENTLY_USED_SYNCED_OR_STORED_FROM_DPMS:
        // validation_rules
        // custom_section_id

        public void SetMdpmsdbContext(MDPMSDatabaseContext context)
        {
        }

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

        public CustomField GetObjectFromJson(dynamic json)
        {            
            return new CustomField
            {
                ExternalId = json.id,
                CreatedAt = json.created_at,
                LastUpdatedAt = json.updated_at,
                SoftDeleted = false,
                Name = json.name,
                FieldType = json.field_type,
                ModelType = json.model_type,
                HelpText = json.help_text,
                SortOrder = json.sort_order,
                Options = json.selections
            };
        }

        public string GetJsonFromObject()
        {
            throw new NotImplementedException();
        }

        public bool GetObjectNeedsUpate(CustomField checkUpdateFrom)
        {
            if (!Name.Equals(checkUpdateFrom.Name)) return true;
            if (!FieldType.Equals(checkUpdateFrom.FieldType)) return true;
            if (!ModelType.Equals(checkUpdateFrom.ModelType)) return true;
            if (!HelpText.Equals(checkUpdateFrom.HelpText)) return true;
            if (!SortOrder.Equals(checkUpdateFrom.SortOrder)) return true;
            if (!Options.Equals(checkUpdateFrom.Options)) return true;
            return false;
        }

        public void UpdateObject(CustomField updateFrom)
        {
            LastUpdatedAt = updateFrom.LastUpdatedAt;
            Name = updateFrom.Name;
            FieldType = updateFrom.FieldType;
            ModelType = updateFrom.ModelType;
            HelpText = updateFrom.HelpText;
            SortOrder = updateFrom.SortOrder;
            Options = updateFrom.Options;
        }

        public string GenerateUpdateJsonFromObject(CustomField updateFrom)
        {
            throw new NotImplementedException();
        }
    }
}
