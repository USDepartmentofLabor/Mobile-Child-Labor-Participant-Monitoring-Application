namespace MDPMS.Database.Data.Models
{
    public class PersonWorkActivity
    {
        public int PersonInternalId { get; set; }
        public Person Person { get; set; }
        public int WorkActivityInternalId { get; set; }
        public StatusCustomizationWorkActivity WorkActivity { get; set; }
    }
}
