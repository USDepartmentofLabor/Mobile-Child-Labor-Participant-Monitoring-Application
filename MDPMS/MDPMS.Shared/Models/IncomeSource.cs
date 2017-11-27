﻿using MDPMS.Shared.Models.Base;

namespace MDPMS.Shared.Models
{
    /// <summary>
    /// Household income source
    /// </summary>
    public class IncomeSource : BaseRecord
    {
        /// <summary>
        /// Name of product or service
        /// Fields stored as string since they can be open ended answers
        /// </summary>
        public string ProductServiceName { get; set; }

        /// <summary>
        /// Estimated volume produced
        /// </summary>
        public string EstimatedVolumeProduced { get; set; }

        /// <summary>
        /// Estimated volume sold
        /// </summary>
        public string EstimatedVolumeSold { get; set; }

        /// <summary>
        /// Unit of Measure
        /// </summary>
        public string UnitOfMeasure { get; set; }

        /// <summary>
        /// Estimated Income
        /// </summary>
        public string EstimatedIncome { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        public string Currency { get; set; }        
    }
}
