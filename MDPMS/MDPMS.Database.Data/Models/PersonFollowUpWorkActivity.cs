namespace MDPMS.Database.Data.Models
{
    public class PersonFollowUpWorkActivity
    {
        public int PersonFollowUpInternalId { get; set; }
        public PersonFollowUp PersonFollowUp { get; set; }
        public int WorkActivityInternalId { get; set; }
        public StatusCustomizationWorkActivity WorkActivity { get; set; }
    }
}
