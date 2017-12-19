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
            GoToView(new HouseholdsView
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
            GoToView(new SyncView { BindingContext = syncViewModel });
            
            HideMenu();

            // sync
            syncViewModel.StatusMessage = @"Syncing";
            syncViewModel.IsBusy = true;
            await Task.Run(() =>
            {
                System.Threading.Thread.Sleep(5000);                
            });
            syncViewModel.IsBusy = false;
            
            // display original view
            ApplicationInstanceData.NavigationPage = currentView;
            ApplicationInstanceData.RootPage.Detail = ApplicationInstanceData.NavigationPage;
            ApplicationInstanceData.RootPage.IsPresented = false;
        }

        private void ExecuteNavigateToSettingsCommand()
        {
            GoToView(new SettingsView
            {
                BindingContext = new SettingsViewModel(ApplicationInstanceData)
            });
        }

        private void ExecuteNavigateToAboutCommand()
        {
            GoToView(new AboutView
            {
                BindingContext = new AboutViewModel(ApplicationInstanceData)
            });
        }

        private void HideMenu()
        {
            ApplicationInstanceData.NavigationPage.Navigation.PopToRootAsync();
            ApplicationInstanceData.RootPage.IsPresented = false;
        }

        private void GoToView(ContentPage view)
        {
            // do not navigate if it is the same choice
            if (view.GetType() != ApplicationInstanceData.NavigationPage.CurrentPage.GetType())
            {
                ApplicationInstanceData.NavigationPage = new NavigationPage(view);
                ApplicationInstanceData.RootPage.Detail = ApplicationInstanceData.NavigationPage;
            }
            ApplicationInstanceData.RootPage.IsPresented = false;
        }
    }
}
