using MDPMS.EfDatabase.EfModels.Base;

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

        // TODO: Intake, hold only 1?
        // TODO: Income sources, hold many
        // TODO: Members, hold many people        
    }
}
