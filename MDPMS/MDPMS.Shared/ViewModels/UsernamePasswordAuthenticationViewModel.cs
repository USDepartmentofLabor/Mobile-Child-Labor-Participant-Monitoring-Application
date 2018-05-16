using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.ViewModels.ContentPageModels;
using MDPMS.Shared.Views.ContentPages;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels
{
    public class UsernamePasswordAuthenticationViewModel : ViewModelBase
    {
        public Command AuthenticateCommand { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        private bool _showPassword;

        public bool ShowPassword
        {
            get => _showPassword;
            set
            {
                _showPassword = value;
                OnPropertyChanged(@"ShowPassword");
            }
        }
        
        public UsernamePasswordAuthenticationViewModel(ApplicationInstanceData applicationInstanceData)
        {
            ApplicationInstanceData = applicationInstanceData;
            AuthenticateCommand = new Command(ExecuteAuthenticateCommand);
            Username = ApplicationInstanceData.SerializedApplicationInstanceData.LastSuccessfulUsernameUsed;
        }

        private void ExecuteAuthenticateCommand()
        {            
            // TODO: on check connectivity also check if DPMS is reachable, separate msgs for internet vs. DPMS problems
            if (CrossConnectivity.Current.IsConnected)
            {
                // get api key from URL/api/tokens - http basicauth - username + password

                try
                {
                    var apiKeyResponse = Helper.Rest.RestHelper.PerformRestGetRequestWithHttpBasicAuth(
                        ApplicationInstanceData.SerializedApplicationInstanceData.Url,
                        @"/api/tokens",
                        Username,
                        Password);
                    // parse json response to get api key and store it
                    ApplicationInstanceData.SerializedApplicationInstanceData.ApiKey = Helper.Json.JsonFileHelper.ParseTokenResponse(apiKeyResponse);
                    // save last successfully used username
                    ApplicationInstanceData.SerializedApplicationInstanceData.LastSuccessfulUsernameUsed = Username;
                    // Save app data
                    Helper.Json.JsonFileHelper.SaveDataToJsonFile(ApplicationInstanceData.SerializedApplicationInstanceData, System.IO.Path.Combine(ApplicationInstanceData.PlatformDataPath, ApplicationInstanceData.ApplicationInstanceDataFileName));
                    // go back to settings since successful
                    ApplicationInstanceData.GoToView(new SettingsContentPage { BindingContext = new SettingsContentPageModel(ApplicationInstanceData) });
                }
                catch
                {
                    // TODO: log error
                    ApplicationInstanceData.App.MainPage.DisplayAlert(
                        ApplicationInstanceData.SelectedLocalization.Translations[@"Alert"],
                        ApplicationInstanceData.SelectedLocalization.Translations[@"Error"],
                        ApplicationInstanceData.SelectedLocalization.Translations[@"OK"]);
                }                
            }
            else
            {                
                ApplicationInstanceData.App.MainPage.DisplayAlert(
                    ApplicationInstanceData.SelectedLocalization.Translations[@"Alert"],
                    ApplicationInstanceData.SelectedLocalization.Translations[@"ConnectivityProblem"],
                    ApplicationInstanceData.SelectedLocalization.Translations[@"OK"]);
            }            
        }
    }
}
