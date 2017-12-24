using System;
using MDPMS.Database.Data.Models.Base;
using System.Collections.Generic;

namespace MDPMS.Database.Data.Models
{
    /// <summary>
    /// Household, contains 1 or more people
    /// </summary>
    public class Household : EfBaseModel
    {
        // TODO: QUESTION - is household name unique?

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

        /// <summary>
        /// Household members
        /// </summary>
        public List<Person> Members { get; set; }
    }
}
