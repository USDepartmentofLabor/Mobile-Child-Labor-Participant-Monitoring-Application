using System.ComponentModel.DataAnnotations;

namespace MDPMS.Database.Localization.Models
{
    public class Localization
    {
        /// <summary>
        /// GUID Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// ISO 639-1 code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Localization name
        /// </summary>
        public string Name { get; set; }
    }
}
