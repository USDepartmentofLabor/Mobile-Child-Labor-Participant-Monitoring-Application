namespace MDPMS.Database.Data.Models
{
    public class PersonHazardousCondition
    {
        public int PersonInternalId { get; set; }
        public Person Person { get; set; }
        public int HazardousConditionInternalId { get; set; }
        public StatusCustomizationHazardousCondition HazardousCondition { get; set; }
    }
}
