using System.ComponentModel;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public Command RevertChangesCommand { get; set; }
        public Command CommitChangesCommand { get; set; }

        public string DpmsUrl { get; set; }
        public string DpmsApiKey { get; set; }

        public SettingsViewModel(ApplicationInstanceData applicationInstanceData)
        {
            RevertChangesCommand = new Command(ExecuteRevertChangesCommand);
            CommitChangesCommand = new Command(ExecuteCommitChangesCommand);
            ApplicationInstanceData = applicationInstanceData;
            SetDpmsUrl();
            SetDpmsApiKey();
        }

        private void ExecuteCommitChangesCommand()
        {
            ApplicationInstanceData.SerializedApplicationInstanceData.Url = DpmsUrl;
            ApplicationInstanceData.SerializedApplicationInstanceData.ApiKey = DpmsApiKey;
            Helper.Json.JsonFileHelper.SaveDataToJsonFile(ApplicationInstanceData.SerializedApplicationInstanceData, System.IO.Path.Combine(ApplicationInstanceData.PlatformDataPath, ApplicationInstanceData.ApplicationInstanceDataFileName));
        }

        private void ExecuteRevertChangesCommand()
        {
            SetDpmsUrl();
        }

        private void SetDpmsUrl()
        {
            DpmsUrl = ApplicationInstanceData.SerializedApplicationInstanceData.Url;
            OnPropertyChanged(@"DpmsUrl");
        }

        private void SetDpmsApiKey()
        {
            DpmsApiKey = ApplicationInstanceData.SerializedApplicationInstanceData.ApiKey;
            OnPropertyChanged(@"ApiKey");
        }
    }
}
