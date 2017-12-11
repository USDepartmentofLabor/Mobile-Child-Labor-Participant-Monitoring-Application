using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MDPMS.Database.Localization.Models
{
    public class Value
    {
        [Key]
        public int Id { get; set; }  
        
        [Required]
        public Key Key { get; set; }
    
        [Required]
        public Localization Localization { get; set; }
        public string KeyLocalizationValue { get; set; }
    }
}
