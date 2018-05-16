using System;
using System.Linq;
using System.Threading.Tasks;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.ViewModels.ContentPageModels;
using MDPMS.Shared.Views;
using MDPMS.Shared.Views.ContentPages;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {        
        public Command HideMenuCommand { get; set; }
        public Command NavigateToLandingViewCommand { get; set; }
        public Command NavigateToHouseholdsCommand { get; set; }
        public Command NavigateToHouseholdMembersCommand { get; set; }
        public Command SyncCommand { get; set; }
        public Command NavigateToSettingsCommand { get; set; }
        public Command NavigateToAboutCommand { get; set; }
        
        public MenuViewModel(ApplicationInstanceData applicationInstanceData)
        {
            HideMenuCommand = new Command(ExecuteHideMenuCommand);
            NavigateToLandingViewCommand = new Command(ExecuteNavigateToLandingViewCommand);
            NavigateToHouseholdsCommand = new Command(ExecuteNavigateToHouseholdsCommand);
            NavigateToHouseholdMembersCommand = new Command(ExecuteNavigateToHouseholdMembersCommand);
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
            ApplicationInstanceData.GoToView(new HouseholdsSearchView
            {
                BindingContext = new HouseholdsSearchViewModel(ApplicationInstanceData)
            });
        }

        private void ExecuteNoInternetConnectivityErrorMessageAction()
        {
            ApplicationInstanceData.App.MainPage.DisplayAlert(                   
                ApplicationInstanceData.SelectedLocalization.Translations[@"Error"],
                ApplicationInstanceData.SelectedLocalization.Translations[@"AlertMessageSyncCancelledNoInternetConnectivity"],
                ApplicationInstanceData.SelectedLocalization.Translations[@"OK"]);
        }

        private async void ExecuteSyncCommand()
        {
            // check for internet connectivity
            var hasInternetConnectivity = Plugin.Connectivity.CrossConnectivity.Current.IsConnected;
            if (!hasInternetConnectivity)
            {
                ExecuteNoInternetConnectivityErrorMessageAction();
                ApplicationInstanceData.RootPage.IsPresented = false;
                return;
            }

            // save current view
            var currentView = ApplicationInstanceData.NavigationPage;
            
            // show sync view
            var syncContentPageModel = new SyncContentPageModel(ApplicationInstanceData);
            ApplicationInstanceData.GoToView(new SyncContentPage { BindingContext = syncContentPageModel });
            
            HideMenu();

            // disable menu
            ApplicationInstanceData.RootPage.IsGestureEnabled = false;

            // sync            
            syncContentPageModel.StatusMessage = ApplicationInstanceData.SelectedLocalization.Translations[@"Syncing"];
            syncContentPageModel.IsBusy = true;
            var taskResult = new Tuple<bool, string>(false, @"");
            await Task.Run(() => { taskResult = Workers.SyncWorker.Sync(ApplicationInstanceData, false); });
            syncContentPageModel.IsBusy = false;
            
            // display original view
            ApplicationInstanceData.NavigationPage = currentView;

            // if HouseholdsView then refresh
            if (currentView.Pages.First().GetType() == typeof(HouseholdsSearchView))
            {
                var householdsSearchViewModel = (HouseholdsSearchViewModel)currentView.Pages.First().BindingContext;
                await Task.Run(() => { householdsSearchViewModel.RefreshCommand.Execute(null); });
            }

            // if HouseholdMembersSearchView then refresh
            if (currentView.Pages.First().GetType() == typeof(HouseholdMembersSearchView))
            {
                var viewModel = (HouseholdMembersSearchViewModel)currentView.Pages.First().BindingContext;
                await Task.Run(() => { viewModel.RefreshCommand.Execute(null); });
            }

            ApplicationInstanceData.RootPage.Detail = ApplicationInstanceData.NavigationPage;
            ApplicationInstanceData.RootPage.IsPresented = false;

            // re-enable menu
            ApplicationInstanceData.RootPage.IsGestureEnabled = true;
            ExecutePostExecuteSyncCommand(taskResult);
        }

        private void ExecutePostExecuteSyncCommand(Tuple<bool, string> taskResult)
        {
            ApplicationInstanceData.App.MainPage.DisplayAlert(
                ApplicationInstanceData.SelectedLocalization.Translations[@"Sync"],
                taskResult.Item1
                    ? ApplicationInstanceData.SelectedLocalization.Translations[@"SyncSuccessful"]
                    : ApplicationInstanceData.SelectedLocalization.Translations[@"Error"] + @" - " + taskResult.Item2,
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
            ApplicationInstanceData.GoToView(new AboutContentPage
            {
                BindingContext = new AboutContentPageModel(ApplicationInstanceData)
            });
        }

        private void ExecuteNavigateToHouseholdMembersCommand()
        {
            ApplicationInstanceData.GoToView(new HouseholdMembersSearchView
            {
                BindingContext = new HouseholdMembersSearchViewModel(ApplicationInstanceData)
            });
        }

        private void HideMenu()
        {
            ApplicationInstanceData.NavigationPage.Navigation.PopToRootAsync();
            ApplicationInstanceData.RootPage.IsPresented = false;
        }
    }
}
