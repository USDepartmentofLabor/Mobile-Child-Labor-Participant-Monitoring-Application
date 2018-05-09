using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.ViewModels.ContentViewModels;
using MDPMS.Shared.Views.ContentViews;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels.ContentPageModels
{
    public class ServiceInstanceEditContentPageModel : ViewModelBase
    {
        public string Title { get; set; }
        public string SaveCommandVerb { get; set; }

        public Command CancelCommand { get; set; }
        public Command SaveCommand { get; set; }

        public ServiceInstanceEditContentView ServiceInstanceEditContentView { get; set; }
        public ServiceInstanceEditContentViewModel ServiceInstanceEditContentViewModel { get; set; }

        public ServiceInstance ServiceInstance { get; set; }

        public bool IsCreate { get; set; }
        public Person ParentPerson { get; set; }

        public ServiceInstanceEditContentPageModel(ApplicationInstanceData applicationInstanceData, Person parentPerson)
        {
            IsCreate = true;
            SaveCommandVerb = applicationInstanceData.SelectedLocalization.Translations[@"Submit"];
            Title = applicationInstanceData.SelectedLocalization.Translations[@"AssignService"];
            ParentPerson = parentPerson;
            Init(applicationInstanceData);
        }

        public ServiceInstanceEditContentPageModel(ApplicationInstanceData applicationInstanceData, ServiceInstance serviceInstance)
        {
            IsCreate = false;
            SaveCommandVerb = applicationInstanceData.SelectedLocalization.Translations[@"Save"];
            Title = applicationInstanceData.SelectedLocalization.Translations[@"EditServiceAssignment"];
            ServiceInstance = serviceInstance;
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
            var validation = ServiceInstanceEditContentViewModel.Validate();
            if (validation)
            {
                ServiceInstanceEditContentViewModel.Save();
                CloseView();
            }
        }

        private void CloseView()
        {
            ApplicationInstanceData.NavigationPage.PopAsync();
        }
    }
}
