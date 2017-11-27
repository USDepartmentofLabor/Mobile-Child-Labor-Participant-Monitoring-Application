using MDPMS.Shared.Models.Base;

namespace MDPMS.Shared.Models
{
    /// <summary>
    /// The family or social relationship of one person to another, e.g. parent, grandchild, etc.
    /// </summary>
    public class PersonRelationship : BaseRecord
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
    }
}
