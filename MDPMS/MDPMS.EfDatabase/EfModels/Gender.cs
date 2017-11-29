using System.ComponentModel.DataAnnotations;

namespace MDPMS.EfDatabase.EfModels
{
    /// <summary>
    /// Look up table for Gender
    /// </summary>
    public class Gender
    {
        [Key]
        public int GenderId { get; set; }
        public string GenderReadable { get; set; }
    }
}
