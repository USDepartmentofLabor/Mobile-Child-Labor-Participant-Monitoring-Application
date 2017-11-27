using System.Collections.Generic;
using MDPMS.Shared.Models.Base;

namespace MDPMS.Shared.Models
{
    /// <summary>
    /// Household
    /// </summary>
    public class Household : BaseRecord
    {        
        /// <summary>
        /// Household name
        /// </summary>
        public string Name { get; set; }

        // TODO: Intake

        #region "Child objects"

        /// <summary>
        /// Income sources (type IncomeSource)
        /// </summary>
        public List<IncomeSource> IncomeSources { get; set; }

        /// <summary>
        /// Household members (people are type Person)
        /// </summary>
        public List<Person> Members { get; set; } = new List<Person>();

        // TODO: Follow ups

        #endregion

    }
}
