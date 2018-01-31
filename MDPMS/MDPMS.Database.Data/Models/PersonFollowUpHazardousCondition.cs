namespace MDPMS.Database.Data.Models
{
    public class PersonFollowUpHazardousCondition
    {
        public int PersonFollowUpInternalId { get; set; }
        public PersonFollowUp PersonFollowUp { get; set; }
        public int HazardousConditionInternalId { get; set; }
        public StatusCustomizationHazardousCondition HazardousCondition { get; set; }
    }
}
