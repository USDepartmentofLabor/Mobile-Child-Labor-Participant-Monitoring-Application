using MDPMS.EfDatabase.EfModels.Base;
using System.Collections.Generic;

namespace MDPMS.EfDatabase.EfModels
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
        public string Name { get; set; }

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
