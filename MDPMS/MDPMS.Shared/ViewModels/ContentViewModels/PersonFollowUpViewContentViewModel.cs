using System.Linq;
using System.Text;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;

namespace MDPMS.Shared.ViewModels.ContentViewModels
{
    public class PersonFollowUpViewContentViewModel : ViewModelBase
    {
        public PersonFollowUp PersonFollowUp { get; set; }

        public string WorkActivitiesReadableList { get; set; }
        public string HazardousConditionsReadableList { get; set; }
        public string HouseholdTasksReadableList { get; set; }

        public string HaveJobReturningToReadable { get; set; }
        public string EnrolledInSchoolReadable { get; set; }

        public PersonFollowUpViewContentViewModel(ApplicationInstanceData applicationInstanceData, PersonFollowUp personFollowUp)
        {
            ApplicationInstanceData = applicationInstanceData;
            PersonFollowUp = personFollowUp;

            WorkActivitiesReadableList = GetWorkActivitiesReadableList(PersonFollowUp);
            HazardousConditionsReadableList = GetHazardousConditionsReadableList(PersonFollowUp);
            HouseholdTasksReadableList = GetHouseholdTasksReadableList(personFollowUp);

            HaveJobReturningToReadable = GetBooleanTranslated(ApplicationInstanceData.SelectedLocalization, PersonFollowUp.HaveJobReturningTo);
            EnrolledInSchoolReadable = GetBooleanTranslated(ApplicationInstanceData.SelectedLocalization, PersonFollowUp.EnrolledInSchool);
        }

        private string GetWorkActivitiesReadableList(PersonFollowUp personFollowUp)
        {
            var sb = new StringBuilder();
            var i = 0;
            var workActivityCount = personFollowUp.PeopleFollowUpWorkActivities.Count();
            foreach (var workActivity in personFollowUp.PeopleFollowUpWorkActivities)
            {
                sb.Append(ApplicationInstanceData.Data.StatusCustomizationWorkActivities.First(a => a.InternalId == workActivity.WorkActivityInternalId).DisplayName);
                if (i != (workActivityCount - 1)) sb.Append(@", ");
                i++;
            }
            return sb.ToString();
        }

        private string GetHazardousConditionsReadableList(PersonFollowUp personFollowUp)
        {
            var sb = new StringBuilder();
            var i = 0;
            var objectCount = personFollowUp.PeopleFollowUpHazardousConditions.Count();
            foreach (var workActivity in personFollowUp.PeopleFollowUpHazardousConditions)
            {
                sb.Append(ApplicationInstanceData.Data.StatusCustomizationHazardousConditions.First(a => a.InternalId == workActivity.HazardousConditionInternalId).DisplayName);
                if (i != (objectCount - 1)) sb.Append(@", ");
                i++;
            }
            return sb.ToString();
        }

        private string GetHouseholdTasksReadableList(PersonFollowUp personFollowUp)
        {
            var sb = new StringBuilder();
            var i = 0;
            var objectCount = personFollowUp.PeopleFollowUpHouseholdTasks.Count();
            foreach (var workActivity in personFollowUp.PeopleFollowUpHouseholdTasks)
            {
                sb.Append(ApplicationInstanceData.Data.StatusCustomizationHouseholdTasks.First(a => a.InternalId == workActivity.HouseholdTaskInternalId).DisplayName);
                if (i != (objectCount - 1)) sb.Append(@", ");
                i++;
            }
            return sb.ToString();
        }

        private string GetBooleanTranslated(Localization localization, bool? value)
        {
            if (value == null) return @"";
            return localization.Translations[(bool)value ? @"Yes" : @"No"];
        }
    }
}
