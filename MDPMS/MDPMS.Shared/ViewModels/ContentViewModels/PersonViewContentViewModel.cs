using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;

namespace MDPMS.Shared.ViewModels.ContentViewModels
{
    public class PersonViewContentViewModel : ViewModelBase
    {
        public Person Person { get; set; }

        public string GenderTranslated { get { return ApplicationInstanceData.SelectedLocalization.Translations[Person.Gender.GenderReadable]; } }

        public string IsTheBirthdayAnApproximateDateTranslated { get { return ApplicationInstanceData.SelectedLocalization.Translations[Person.DateOfBirthIsApproximate.ToString()]; } }

        public PersonViewContentViewModel(ApplicationInstanceData applicationInstanceData, Person person)
        {
            ApplicationInstanceData = applicationInstanceData;
            Person = person;
        }
    }
}
