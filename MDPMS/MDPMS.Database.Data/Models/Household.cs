using System;
using MDPMS.Database.Data.Models.Base;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MDPMS.Database.Data.Database;
using Newtonsoft.Json;

namespace MDPMS.Database.Data.Models
{
    /// <summary>
    /// Household, contains 1 or more people
    /// </summary>
    public class Household : EfBaseModel, ISyncableWithChildren<Household>
    {
        /// <summary>
        /// Household name assigned
        /// </summary>
        public string HouseholdName { get; set; }

        /// <summary>
        /// Intake date
        /// </summary>
        public DateTime IntakeDate { get; set; }

        /// <summary>
        /// Address line 1, address_line_1 from api
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Address line 2, address_line_2 from api
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Postal code, postal_code from api, en ui display localization is Zip Code
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Dependent locality, dependent_locality from api, en ui display localization is Suburb/Neighborhood
        /// </summary>
        public string DependentLocality { get; set; }

        /// <summary>
        /// Locality, locality from api, en ui display localization is City
        /// </summary>
        public string Locality { get; set; }

        /// <summary>
        /// Administrative area, adminv_area from api, en ui display localization is State
        /// </summary>
        public string AdminvArea { get; set; }

        /// <summary>
        /// Dependent administrative area, dependent_adminv_area from api, en ui display localization is County
        /// </summary>
        public string DependentAdminvArea { get; set; }

        /// <summary>
        /// Country, country from api, en ui display localization is Country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Address info, address_info from api, en ui display localization is Other/Address Notes
        /// </summary>
        public string AddressInfo { get; set; }
        
        /// <summary>
        /// FK to manu income sources
        /// </summary>
        public List<IncomeSource> IncomeSources { get; set; }

        public void AddIncomeSource(IncomeSource incomeSource)
        {
            if (IncomeSources == null) IncomeSources = new List<IncomeSource>();
            incomeSource.InternalParentId = InternalId;
            IncomeSources.Add(incomeSource);
        }

        /// <summary>
        /// Household members
        /// </summary>
        public List<Person> Members { get; set; }

        public void AddMember(Person person)
        {
            if (Members == null) Members = new List<Person>();
            person.InternalParentId = InternalId;
            Members.Add(person);
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

        public int? GetInternalId()
        {
            return InternalId;
        }

        public void SetMdpmsdbContext(MDPMSDatabaseContext context)
        {
        }

        public Household GetObjectFromJson(dynamic json)
        {
            return new Household
            {
                ExternalId = json.id,
                CreatedAt = json.created_at,
                LastUpdatedAt = json.updated_at,
                SoftDeleted = false,
                HouseholdName = json.name,
                IntakeDate = json.intake_date,
                AddressLine1 = json.address_line_1,
                AddressLine2 = json.address_line_2,
                PostalCode = json.postal_code,
                DependentLocality = json.dependent_locality,
                Locality = json.locality,
                AdminvArea = json.adminv_area,
                DependentAdminvArea = json.dependent_adminv_area,
                Country = json.country,
                AddressInfo = json.address_info
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
                writer.WritePropertyName(@"household");
                writer.WriteStartObject();
                writer.WritePropertyName("name");
                writer.WriteValue(HouseholdName);
                writer.WritePropertyName("intake_date");
                writer.WriteValue(IntakeDate.ToString("yyyy-MM-dd"));
                writer.WritePropertyName("address_line_1");
                writer.WriteValue(AddressLine1);
                writer.WritePropertyName("address_line_2");
                writer.WriteValue(AddressLine2);
                writer.WritePropertyName("postal_code");
                writer.WriteValue(PostalCode);
                writer.WritePropertyName("dependent_locality");
                writer.WriteValue(DependentLocality);
                writer.WritePropertyName("locality");
                writer.WriteValue(Locality);
                writer.WritePropertyName("adminv_area");
                writer.WriteValue(AdminvArea);
                writer.WritePropertyName("dependent_adminv_area");
                writer.WriteValue(DependentAdminvArea);
                writer.WritePropertyName("country");
                writer.WriteValue(Country);
                writer.WritePropertyName("address_info");
                writer.WriteValue(AddressInfo);
                writer.WriteEndObject();
                writer.WriteEndObject();
            }
            return sw.ToString();
        }

        public void UpdateObject(Household updateFrom)
        {            
            LastUpdatedAt = updateFrom.LastUpdatedAt;
            HouseholdName = updateFrom.HouseholdName;
            IntakeDate = updateFrom.IntakeDate;
            AddressLine1 = updateFrom.AddressLine1;
            AddressLine2 = updateFrom.AddressLine1;
            PostalCode = updateFrom.PostalCode;
            DependentLocality = updateFrom.DependentLocality;
            Locality = updateFrom.Locality;
            AdminvArea = updateFrom.AdminvArea;
            DependentAdminvArea = updateFrom.DependentAdminvArea;
            Country = updateFrom.Country;
            AddressInfo = updateFrom.AddressInfo;
        }

        public bool GetObjectNeedsUpate(Household checkUpdateFrom)
        {
            if (!HouseholdName.Equals(checkUpdateFrom.HouseholdName)) return true;
            if (!IntakeDate.Equals(checkUpdateFrom.IntakeDate)) return true;
            if (!AddressLine1.Equals(checkUpdateFrom.AddressLine1)) return true;
            if (!AddressLine2.Equals(checkUpdateFrom.AddressLine2)) return true;
            if (!PostalCode.Equals(checkUpdateFrom.PostalCode)) return true;
            if (!DependentLocality.Equals(checkUpdateFrom.DependentLocality)) return true;
            if (!Locality.Equals(checkUpdateFrom.Locality)) return true;
            if (!AdminvArea.Equals(checkUpdateFrom.AdminvArea)) return true;
            if (!DependentAdminvArea.Equals(checkUpdateFrom.DependentAdminvArea)) return true;
            if (!Country.Equals(checkUpdateFrom.Country)) return true;
            if (!AddressInfo.Equals(checkUpdateFrom.AddressInfo)) return true;
            return false;
        }

        public string GenerateUpdateJsonFromObject(Household updateFrom)
        {
            // form the json (determine the fields that need to be updated)
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            var writer = new JsonTextWriter(sw) { Formatting = Formatting.None };
            writer.WriteStartObject();
            writer.WritePropertyName(@"household");
            writer.WriteStartObject();

            if (!HouseholdName.Equals(updateFrom.HouseholdName))
            {
                writer.WritePropertyName("name");
                writer.WriteValue(updateFrom.HouseholdName);
            }

            if (!IntakeDate.Equals(updateFrom.IntakeDate))
            {
                writer.WritePropertyName("intake_date");
                writer.WriteValue(updateFrom.IntakeDate.ToString("yyyy-MM-dd"));
            }

            if (!AddressLine1.Equals(updateFrom.AddressLine1))
            {
                writer.WritePropertyName("address_line_1");
                writer.WriteValue(updateFrom.AddressLine1);
            }

            if (!AddressLine2.Equals(updateFrom.AddressLine2))
            {
                writer.WritePropertyName("address_line_2");
                writer.WriteValue(updateFrom.AddressLine2);
            }

            if (!PostalCode.Equals(updateFrom.PostalCode))
            {
                writer.WritePropertyName("postal_code");
                writer.WriteValue(updateFrom.PostalCode);
            }

            if (!DependentLocality.Equals(updateFrom.DependentLocality))
            {
                writer.WritePropertyName("dependent_locality");
                writer.WriteValue(updateFrom.DependentLocality);
            }

            if (!Locality.Equals(updateFrom.Locality))
            {
                writer.WritePropertyName("locality");
                writer.WriteValue(updateFrom.Locality);
            }

            if (!AdminvArea.Equals(updateFrom.AdminvArea))
            {
                writer.WritePropertyName("adminv_area");
                writer.WriteValue(updateFrom.AdminvArea);
            }

            if (!DependentAdminvArea.Equals(updateFrom.DependentAdminvArea))
            {
                writer.WritePropertyName("dependent_adminv_area");
                writer.WriteValue(updateFrom.DependentAdminvArea);
            }

            if (!Country.Equals(updateFrom.Country))
            {
                writer.WritePropertyName("country");
                writer.WriteValue(updateFrom.Country);
            }

            if (!AddressInfo.Equals(updateFrom.AddressInfo))
            {
                writer.WritePropertyName("address_info");
                writer.WriteValue(updateFrom.AddressInfo);
            }

            writer.WriteEndObject();
            writer.WriteEndObject();
            return sw.ToString();
        }

        public void SetParentIdsInChildObjects()
        {
            if (IncomeSources != null)
            {
                foreach (var incomeSource in IncomeSources)
                {
                    if (incomeSource.GetExternalParentId() == null & ExternalId != null)
                    {
                        incomeSource.SetExternalParentId(ExternalId);
                    }
                    incomeSource.SetInternalParentId(InternalId);
                }
            }
            if (Members != null)
            {
                foreach (var member in Members)
                {
                    if (member.GetExternalParentId() == null & ExternalId != null)
                    {
                        member.SetExternalParentId(ExternalId);
                    }
                    member.SetInternalParentId(InternalId);
                }
            }                     
        }
    }
}
