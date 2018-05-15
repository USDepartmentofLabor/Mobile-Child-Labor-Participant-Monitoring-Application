using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.ViewModels.ContentPageModels;
using MDPMS.Shared.Views.ContentPages;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels.ContentViewModels
{
    public class PersonFollowUpsViewContentViewModel : ViewModelBase
    {
        public Command AddPersonFollowUpCommand { get; set; }

        public Person Person { get; set; }

        private PersonFollowUp _selectedPersonFollowUp;
        public PersonFollowUp SelectedPersonFollowUp
        {
            get => _selectedPersonFollowUp;
            set
            {
                _selectedPersonFollowUp = value;
                if (_selectedPersonFollowUp == null) return;
                ApplicationInstanceData.NavigationPage.PushAsync(new PersonFollowUpViewContentPage
                {
                    BindingContext = new PersonFollowUpViewContentPageModel(ApplicationInstanceData, value)
                });
            }
        }

        public PersonFollowUpsViewContentViewModel(ApplicationInstanceData applicationInstanceData, Person person)
        {
            ApplicationInstanceData = applicationInstanceData;
            Person = person;
            AddPersonFollowUpCommand = new Command(ExecuteAddPersonFollowUpCommand);
        }

        private void ExecuteAddPersonFollowUpCommand()
        {
            ApplicationInstanceData.NavigationPage.PushAsync(new PersonFollowUpEditContentPage { BindingContext = new PersonFollowUpEditContentPageModel(ApplicationInstanceData, Person) });
        }
    }
}
