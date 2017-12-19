using System.Threading.Tasks;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private string _statusMessage { get; set; }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (value == _statusMessage) return;
                _statusMessage = value;
                RaisePropertyChanged(() => StatusMessage);
            }
        }

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
            ApplicationInstanceData.HideMenu();
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
            // SYNC
            StatusMessage = @"Syncing";
            IsBusy = true;

            await Task.Run(() =>
            {
                System.Threading.Thread.Sleep(5000);
            });

            IsBusy = false;
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
    }
}
