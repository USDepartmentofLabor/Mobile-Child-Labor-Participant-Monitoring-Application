using System.ComponentModel.DataAnnotations;

namespace MDPMS.EfDatabase.EfModels
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
    }
}
