using System;

namespace MDPMS.Shared.Models
{
    /// <summary>
    /// Project details, one per app instance
    /// </summary>
    public class ProjectDetails
    {
        /// <summary>
        /// Short name
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Full name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Cooperative agreement number
        /// </summary>
        public string CooperativeAgreementNumber { get; set; }

        /// <summary>
        /// DateTime Start date of agreement
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// DateTime End date of agreement
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Grantee name
        /// </summary>
        public string GranteeName { get; set; }

        /// <summary>
        /// Funding amount
        /// </summary>
        public uint FundingAmount { get; set; }

        /// <summary>
        /// Region
        /// </summary>
        public string Region { get; set; }

        #region "Child objects"

        // TODO: Locations
        // TODO: Direct Beneficiary Targets

        #endregion

    }
}
