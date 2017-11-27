using System;

namespace MDPMS.Shared.Models.Base
{
    /// <summary>
    /// Base record for models tied to external entities
    /// </summary>
    public class BaseRecord
    {
        // TODO: Identifier as separate type for substitution?
        // TODO: Single Identifier, substitute external for internal as it is available?

        /// <summary>
        /// Internal identifier
        /// </summary>
        public Guid? InternalId { get; set; }

        /// <summary>
        /// Check if it has an internal Id
        /// </summary>
        public bool HasInternalId { get { return InternalId == null; } }

        /// <summary>
        /// External identifier
        /// </summary>
        public int? ExternalId { get; set; }

        /// <summary>
        /// Check if it has an external Id
        /// </summary>
        public bool HasExternalId { get { return ExternalId == null; } }

        /// <summary>
        /// DateTime record was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// DateTime record was last changed
        /// </summary>
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Boolean, true for record is soft deleted
        /// </summary>
        public bool SoftDeleted { get; set; }
    }
}
