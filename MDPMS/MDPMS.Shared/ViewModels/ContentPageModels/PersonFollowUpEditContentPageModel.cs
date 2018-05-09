using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.ViewModels.ContentViewModels;
using MDPMS.Shared.Views.ContentViews;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels.ContentPageModels
{
    public class PersonFollowUpEditContentPageModel : ViewModelBase
    {
        public string Title { get; set; }
        public string SaveCommandVerb { get; set; }

        public Command CancelCommand { get; set; }
        public Command SaveCommand { get; set; }

        public PersonFollowUpEditContentView PersonFollowUpEditContentView { get; set; }
        public PersonFollowUpEditContentViewModel PersonFollowUpEditContentViewModel { get; set; }

        public PersonFollowUp PersonFollowUp { get; set; }

        public bool IsCreate { get; set; }
        public Person ParentPerson { get; set; }

        public PersonFollowUpEditContentPageModel(ApplicationInstanceData applicationInstanceData, Person parentPerson)
        {
            IsCreate = true;
            SaveCommandVerb = applicationInstanceData.SelectedLocalization.Translations[@"Submit"];
            Title = applicationInstanceData.SelectedLocalization.Translations[@"AddFollowUp"];
            ParentPerson = parentPerson;
            Init(applicationInstanceData);
        }

        public PersonFollowUpEditContentPageModel(ApplicationInstanceData applicationInstanceData, PersonFollowUp personFollowUp)
        {
            IsCreate = false;
            SaveCommandVerb = applicationInstanceData.SelectedLocalization.Translations[@"Save"];
            Title = applicationInstanceData.SelectedLocalization.Translations[@"EditFollowUp"];
            PersonFollowUp = personFollowUp;
            Init(applicationInstanceData);
        }

        private void Init(ApplicationInstanceData applicationInstanceData)
        {
            ApplicationInstanceData = applicationInstanceData;

            CancelCommand = new Command(ExecuteCancelCommand);
            SaveCommand = new Command(ExecuteSaveCommand);
        }

        private void ExecuteCancelCommand()
        {
            CloseView();
        }

        private void ExecuteSaveCommand()
        {
            var validation = PersonFollowUpEditContentViewModel.Validate();
            if (validation)
            {
                PersonFollowUpEditContentViewModel.Save();
                CloseView();
            }
        }

        private void CloseView()
        {
            ApplicationInstanceData.NavigationPage.PopAsync();
        }
    }
}
