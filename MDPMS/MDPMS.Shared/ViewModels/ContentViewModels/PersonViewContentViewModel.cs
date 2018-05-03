using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.ViewModels.Helpers;

namespace MDPMS.Shared.ViewModels.ContentViewModels
{
    public class PersonViewContentViewModel : ViewModelBase
    {
        public Person Person { get; set; }

        public string GenderTranslated { get { return ApplicationInstanceData.SelectedLocalization.Translations[Person.Gender.GenderReadable]; } }

        public string IsTheBirthdayAnApproximateDateTranslated { get { return ApplicationInstanceData.SelectedLocalization.Translations[Person.DateOfBirthIsApproximate.ToString()]; } }

        public string WorkActivitiesReadableList { get; set; }
        public string HazardousConditionsReadableList { get; set; }
        public string HouseholdTasksReadableList { get; set; }

        public string HaveJobReturningToReadable { get; set; }
        public string EnrolledInSchoolReadable { get; set; }

        public PersonViewContentViewModel(ApplicationInstanceData applicationInstanceData, Person person)
        {
            ApplicationInstanceData = applicationInstanceData;
            Person = person;

            WorkActivitiesReadableList = StatusCustomizationValueConverter.GetWorkActivitiesReadableList(Person, ApplicationInstanceData);
            HazardousConditionsReadableList = StatusCustomizationValueConverter.GetHazardousConditionsReadableList(Person, ApplicationInstanceData);
            HouseholdTasksReadableList = StatusCustomizationValueConverter.GetHouseholdTasksReadableList(person, ApplicationInstanceData);

            HaveJobReturningToReadable = ViewModelDisplayValueConverter.GetBooleanTranslated(ApplicationInstanceData.SelectedLocalization, Person.HaveJobReturningTo);
            EnrolledInSchoolReadable = ViewModelDisplayValueConverter.GetBooleanTranslated(ApplicationInstanceData.SelectedLocalization, Person.EnrolledInSchool);
        }
    }
}
