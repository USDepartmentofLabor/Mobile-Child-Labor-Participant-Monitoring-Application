using System.Threading.Tasks;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {        
        public Command HideMenuCommand { get; set; }
        public Command NavigateToLandingViewCommand { get; set; }
        public Command NavigateToHouseholdsCommand { get; set; }
        public Command SyncCommand { get; set; }
        public Command NavigateToSettingsCommand { get; set; }
        public Command NavigateToAboutCommand { get; set; }
    
        public MenuViewModel(ApplicationInstanceData applicationInstanceData)
        {
            HideMenuCommand = new Command(ExecuteHideMenuCommand);
            NavigateToLandingViewCommand = new Command(ExecuteNavigateToLandingViewCommand);
            NavigateToHouseholdsCommand = new Command(ExecuteNavigateToHouseholdsCommand);
            SyncCommand = new Command(ExecuteSyncCommand);
            NavigateToSettingsCommand = new Command(ExecuteNavigateToSettingsCommand);
            NavigateToAboutCommand = new Command(ExecuteNavigateToAboutCommand);
            ApplicationInstanceData = applicationInstanceData;
        }

        private void ExecuteHideMenuCommand()
        {
            HideMenu();
        }

        private void ExecuteNavigateToLandingViewCommand()
        {
            ApplicationInstanceData.NavigateToLandingView();
        }

        private void ExecuteNavigateToHouseholdsCommand()
        {
            ApplicationInstanceData.GoToView(new HouseholdsView
            {
                BindingContext = new HouseholdsViewModel(ApplicationInstanceData)
            });
        }
        
        private async void ExecuteSyncCommand()
        {            
            // save current view
            var currentView = ApplicationInstanceData.NavigationPage;
            
            // show sync view
            var syncViewModel = new SyncViewModel(ApplicationInstanceData);
            ApplicationInstanceData.GoToView(new SyncView { BindingContext = syncViewModel });
            
            HideMenu();

            // disable menu
            ApplicationInstanceData.RootPage.IsGestureEnabled = false;

            // sync            
            syncViewModel.StatusMessage = ApplicationInstanceData.SelectedLocalization.Translations[@"Syncing"];
            syncViewModel.IsBusy = true;
            var taskResult = false;
            await Task.Run(() => { taskResult = Workers.SyncWorker.Sync(ApplicationInstanceData); });            
            syncViewModel.IsBusy = false;
            
            // display original view
            ApplicationInstanceData.NavigationPage = currentView;
            ApplicationInstanceData.RootPage.Detail = ApplicationInstanceData.NavigationPage;
            ApplicationInstanceData.RootPage.IsPresented = false;

            // re-enable menu
            ApplicationInstanceData.RootPage.IsGestureEnabled = true;
            ExecutePostExecuteSyncCommand(taskResult);
        }

        private void ExecutePostExecuteSyncCommand(bool success)
        {
            ApplicationInstanceData.App.MainPage.DisplayAlert(
                ApplicationInstanceData.SelectedLocalization.Translations[@"Sync"],
                success
                    ? ApplicationInstanceData.SelectedLocalization.Translations[@"Yes"]
                    : ApplicationInstanceData.SelectedLocalization.Translations[@"Error"],
                ApplicationInstanceData.SelectedLocalization.Translations[@"OK"]);
        }

        private void ExecuteNavigateToSettingsCommand()
        {
            ApplicationInstanceData.GoToView(new SettingsView
            {
                BindingContext = new SettingsViewModel(ApplicationInstanceData)
            });
        }

        private void ExecuteNavigateToAboutCommand()
        {
            ApplicationInstanceData.GoToView(new AboutView
            {
                BindingContext = new AboutViewModel(ApplicationInstanceData)
            });
        }

        private void HideMenu()
        {
            ApplicationInstanceData.NavigationPage.Navigation.PopToRootAsync();
            ApplicationInstanceData.RootPage.IsPresented = false;
        }
    }
}
