using System.Linq;
using System.Text;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;

namespace MDPMS.Shared.ViewModels.Helpers
{
    public static class StatusCustomizationValueConverter
    {
        public static string GetWorkActivitiesReadableList(Person person, ApplicationInstanceData applicationInstanceData)
        {
            var sb = new StringBuilder();
            var i = 0;
            var workActivityCount = person.PeopleWorkActivities.Count();
            foreach (var workActivity in person.PeopleWorkActivities)
            {
                sb.Append(applicationInstanceData.Data.StatusCustomizationWorkActivities.First(a => a.InternalId == workActivity.WorkActivityInternalId).DisplayName);
                if (i != (workActivityCount - 1)) sb.Append(@", ");
                i++;
            }
            return sb.ToString();
        }

        public static string GetWorkActivitiesReadableList(PersonFollowUp personFollowUp, ApplicationInstanceData applicationInstanceData)
        {
            var sb = new StringBuilder();
            var i = 0;
            var workActivityCount = personFollowUp.PeopleFollowUpWorkActivities.Count();
            foreach (var workActivity in personFollowUp.PeopleFollowUpWorkActivities)
            {
                sb.Append(applicationInstanceData.Data.StatusCustomizationWorkActivities.First(a => a.InternalId == workActivity.WorkActivityInternalId).DisplayName);
                if (i != (workActivityCount - 1)) sb.Append(@", ");
                i++;
            }
            return sb.ToString();
        }

        public static string GetHazardousConditionsReadableList(Person person, ApplicationInstanceData applicationInstanceData)
        {
            var sb = new StringBuilder();
            var i = 0;
            var objectCount = person.PeopleHazardousConditions.Count();
            foreach (var workActivity in person.PeopleHazardousConditions)
            {
                sb.Append(applicationInstanceData.Data.StatusCustomizationHazardousConditions.First(a => a.InternalId == workActivity.HazardousConditionInternalId).DisplayName);
                if (i != (objectCount - 1)) sb.Append(@", ");
                i++;
            }
            return sb.ToString();
        }

        public static string GetHazardousConditionsReadableList(PersonFollowUp personFollowUp, ApplicationInstanceData applicationInstanceData)
        {
            var sb = new StringBuilder();
            var i = 0;
            var objectCount = personFollowUp.PeopleFollowUpHazardousConditions.Count();
            foreach (var workActivity in personFollowUp.PeopleFollowUpHazardousConditions)
            {
                sb.Append(applicationInstanceData.Data.StatusCustomizationHazardousConditions.First(a => a.InternalId == workActivity.HazardousConditionInternalId).DisplayName);
                if (i != (objectCount - 1)) sb.Append(@", ");
                i++;
            }
            return sb.ToString();
        }

        public static string GetHouseholdTasksReadableList(Person person, ApplicationInstanceData applicationInstanceData)
        {
            var sb = new StringBuilder();
            var i = 0;
            var objectCount = person.PeopleHouseholdTasks.Count();
            foreach (var workActivity in person.PeopleHouseholdTasks)
            {
                sb.Append(applicationInstanceData.Data.StatusCustomizationHouseholdTasks.First(a => a.InternalId == workActivity.HouseholdTaskInternalId).DisplayName);
                if (i != (objectCount - 1)) sb.Append(@", ");
                i++;
            }
            return sb.ToString();
        }

        public static string GetHouseholdTasksReadableList(PersonFollowUp personFollowUp, ApplicationInstanceData applicationInstanceData)
        {
            var sb = new StringBuilder();
            var i = 0;
            var objectCount = personFollowUp.PeopleFollowUpHouseholdTasks.Count();
            foreach (var workActivity in personFollowUp.PeopleFollowUpHouseholdTasks)
            {
                sb.Append(applicationInstanceData.Data.StatusCustomizationHouseholdTasks.First(a => a.InternalId == workActivity.HouseholdTaskInternalId).DisplayName);
                if (i != (objectCount - 1)) sb.Append(@", ");
                i++;
            }
            return sb.ToString();
        }
    }
}
