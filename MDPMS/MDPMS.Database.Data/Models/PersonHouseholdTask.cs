namespace MDPMS.Database.Data.Models
{
    public class PersonHouseholdTask
    {
        public int PersonInternalId { get; set; }
        public Person Person { get; set; }
        public int HouseholdTaskInternalId { get; set; }
        public StatusCustomizationHouseholdTask HouseholdTask { get; set; }
    }
}
