using System;
using System.ComponentModel.DataAnnotations;

namespace MDPMS.Database.Data.Models.Base
{
    /// <summary>
    /// Base model for entity framework models tied to external entities
    /// </summary>
    public class EfBaseModel
    {
        /// <summary>
        /// Internal identifier
        /// </summary>
        [Key]
        public int InternalId { get; set; }

        /// <summary>
        /// External identifier
        /// </summary>
        public int? ExternalId { get; set; }

        /// <summary>
        /// Check if it has an external Id
        /// </summary>
        public bool HasExternalId => ExternalId != null;

        /// <summary>
        /// DateTime record was created
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// DateTime record was last changed
        /// </summary>
        public DateTime? LastUpdatedAt { get; set; }

        /// <summary>
        /// Boolean, true for record is soft deleted
        /// </summary>
        public bool SoftDeleted { get; set; }       
    }
}
