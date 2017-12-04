using MDPMS.EfDatabase.EfModels.Base;

namespace MDPMS.EfDatabase.EfModels
{
    /// <summary>
    /// The family or social relationship of one person to another, e.g. parent, grandchild, etc.
    /// </summary>
    public class PersonRelationship : EfBaseModel
    {
        /// <summary>
        /// DPMS code field, open ended string
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Display name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Canonical name
        /// </summary>
        public string CanonicalName { get; set; }

        /// <summary>
        /// Indicates if it is a field denoting "other", requiring special extra string
        /// </summary>
        public bool IsOther { get; set; }
    }
}
