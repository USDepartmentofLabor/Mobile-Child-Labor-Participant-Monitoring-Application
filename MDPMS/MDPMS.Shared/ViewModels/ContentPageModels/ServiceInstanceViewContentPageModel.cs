using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views.ContentPages;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels.ContentPageModels
{
    public class ServiceInstanceViewContentPageModel : ViewModelBase
    {
        public ServiceInstance ServiceInstance { get; set; }

        public bool AllowEditDelete { get; set; }
        public GridLength EditDeleteRowHeight { get; set; }

        public Command EditCommand { get; set; }
        public Command DeleteCommand { get; set; }

        public ServiceInstanceViewContentPageModel(ApplicationInstanceData applicationInstanceData, ServiceInstance serviceInstance)
        {
            ApplicationInstanceData = applicationInstanceData;
            ServiceInstance = serviceInstance;
            EditCommand = new Command(ExecuteEditCommand);
            DeleteCommand = new Command(ExecuteDeleteCommand);
            AllowEditDelete = !serviceInstance.HasExternalId;
            EditDeleteRowHeight = AllowEditDelete ? 80 : 0;
        }

        private void ExecuteEditCommand()
        {
            // is it allowed?
            if (ServiceInstance.HasExternalId) return;
            ApplicationInstanceData.NavigationPage.PushAsync(new ServiceInstanceEditContentPage { BindingContext = new ServiceInstanceEditContentPageModel(ApplicationInstanceData, ServiceInstance) });
        }

        private async void ExecuteDeleteCommand()
        {
            // is it allowed?
            if (ServiceInstance.HasExternalId) return;

            // are you sure?
            var actionDecision = await ApplicationInstanceData.App.MainPage.DisplayAlert(
                ApplicationInstanceData.SelectedLocalization.Translations[@"Confirm"],
                ApplicationInstanceData.SelectedLocalization.Translations[@"ConfirmationMessageDeleteServiceAssignment"],
                ApplicationInstanceData.SelectedLocalization.Translations[@"OK"],
                ApplicationInstanceData.SelectedLocalization.Translations[@"Cancel"]);

            if (actionDecision)
            {
                ApplicationInstanceData.Data.DeleteServiceInstance(ServiceInstance.InternalId);
                ApplicationInstanceData.Data.SaveChanges();
                await ApplicationInstanceData.NavigationPage.PopAsync();
            }
        }
    }
}
