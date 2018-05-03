using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.ViewModels.Helpers;

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

            WorkActivitiesReadableList = StatusCustomizationValueConverter.GetWorkActivitiesReadableList(PersonFollowUp, ApplicationInstanceData);
            HazardousConditionsReadableList = StatusCustomizationValueConverter.GetHazardousConditionsReadableList(PersonFollowUp, ApplicationInstanceData);
            HouseholdTasksReadableList = StatusCustomizationValueConverter.GetHouseholdTasksReadableList(personFollowUp, ApplicationInstanceData);

            HaveJobReturningToReadable = GetBooleanTranslated(ApplicationInstanceData.SelectedLocalization, PersonFollowUp.HaveJobReturningTo);
            EnrolledInSchoolReadable = GetBooleanTranslated(ApplicationInstanceData.SelectedLocalization, PersonFollowUp.EnrolledInSchool);
        }

        private string GetBooleanTranslated(Localization localization, bool? value)
        {
            if (value == null) return @"";
            return localization.Translations[(bool)value ? @"Yes" : @"No"];
        }
    }
}
