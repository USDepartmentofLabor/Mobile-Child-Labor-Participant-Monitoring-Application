namespace MDPMS.Database.Data.Models
{
    public class PersonFollowUpHouseholdTask
    {
        public int PersonFollowUpInternalId { get; set; }
        public PersonFollowUp PersonFollowUp { get; set; }
        public int HouseholdTaskInternalId { get; set; }
        public StatusCustomizationHouseholdTask HouseholdTask { get; set; }
    }
}
