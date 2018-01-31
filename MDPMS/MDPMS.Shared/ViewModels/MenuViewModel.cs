﻿using System;
using System.Linq;
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
            var taskResult = new Tuple<bool, string>(false, @"");
            await Task.Run(() => { taskResult = Workers.SyncWorker.Sync(ApplicationInstanceData, false); });
            syncViewModel.IsBusy = false;
            
            // display original view
            ApplicationInstanceData.NavigationPage = currentView;

            // if HouseholdsView then refresh
            if (currentView.Pages.First().GetType() == typeof(HouseholdsView))
            {
                var householdsViewModel = (HouseholdsViewModel)currentView.Pages.First().BindingContext;
                await Task.Run(() => { householdsViewModel.RefreshCommand.Execute(null); });
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
            ApplicationInstanceData.GoToView(new AboutView
            {
                BindingContext = new AboutViewModel(ApplicationInstanceData)
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
