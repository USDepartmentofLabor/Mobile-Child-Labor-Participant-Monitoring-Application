using System;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public Command RevertChangesCommand { get; set; }
        public Command CommitChangesCommand { get; set; }
        public Command NavigateToGetNewApiKeyViewCommand { get; set; }

        public string DpmsUrl { get; set; }
        public string ApiKeyObtained { get; set; }

        public SettingsViewModel(ApplicationInstanceData applicationInstanceData)
        {
            RevertChangesCommand = new Command(ExecuteRevertChangesCommand);
            CommitChangesCommand = new Command(ExecuteCommitChangesCommand);
            NavigateToGetNewApiKeyViewCommand = new Command(ExecuteNavigateToGetNewApiKeyViewCommand);
            ApplicationInstanceData = applicationInstanceData;
            SetDpmsUrl();
            SetApiKeyObtained();
        }

        private void ExecuteCommitChangesCommand()
        {
            ApplicationInstanceData.SerializedApplicationInstanceData.Url = DpmsUrl;
            Helper.Json.JsonFileHelper.SaveDataToJsonFile(ApplicationInstanceData.SerializedApplicationInstanceData, System.IO.Path.Combine(ApplicationInstanceData.PlatformDataPath, ApplicationInstanceData.ApplicationInstanceDataFileName));
        }

        private void ExecuteRevertChangesCommand()
        {
            SetDpmsUrl();
            SetApiKeyObtained();
        }

        private void SetDpmsUrl()
        {
            DpmsUrl = ApplicationInstanceData.SerializedApplicationInstanceData.Url;
            OnPropertyChanged(@"DpmsUrl");
        }

        private void SetApiKeyObtained()
        {
            // TODO: regex for api key check
            ApiKeyObtained = ApplicationInstanceData.SerializedApplicationInstanceData.ApiKey.Equals(String.Empty) ?
                ApplicationInstanceData.SelectedLocalization.Translations[@"Yes"] :
                ApplicationInstanceData.SelectedLocalization.Translations[@"No"];            
            OnPropertyChanged(@"ApiKeyObtained");
        }

        private void ExecuteNavigateToGetNewApiKeyViewCommand()
        {
            ApplicationInstanceData.GoToView(new UsernamePasswordAuthenticationView
            {
                BindingContext = new UsernamePasswordAuthenticationViewModel(ApplicationInstanceData)
            });
        }
    }
}
