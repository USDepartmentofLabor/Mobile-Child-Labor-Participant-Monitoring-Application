using MDPMS.Database.Data.Models.Base;

namespace MDPMS.Database.Data.Models
{
    /// <summary>
    /// Household income source, note fields stored as string since they can be open ended answers
    /// </summary>
    public class IncomeSource : EfBaseModel
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
    }
}
