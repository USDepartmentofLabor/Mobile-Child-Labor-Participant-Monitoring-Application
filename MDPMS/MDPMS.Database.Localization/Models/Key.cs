using System.ComponentModel.DataAnnotations;

namespace MDPMS.Database.Localization.Models
{
    public class Key
    {
        [Key]
        public int Id { get; set; }
        public string KeyName { get; set; }
    }
}
