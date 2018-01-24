using System.ComponentModel.DataAnnotations;

namespace MDPMS.Database.Data.Models
{
    /// <summary>
    /// Look up table for Gender
    /// </summary>
    public class Gender
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public int GenderId { get; set; }

        /// <summary>
        /// Display name for gender
        /// </summary>
        public string GenderReadable { get; set; }

        /// <summary>
        /// Parent DPMS int value for this gender
        /// </summary>
        public int DpmsGenderNumber { get; set; }
    }
}
