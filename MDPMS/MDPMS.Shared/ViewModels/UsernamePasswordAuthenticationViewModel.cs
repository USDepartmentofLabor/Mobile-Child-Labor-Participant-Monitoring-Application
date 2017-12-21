using System;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
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
        }

        private void ExecuteAuthenticateCommand()
        {            
            // TODO: on check connectivity also check if DPMS is reachable, separate msgs for internet vs. DPMS problems
            if (CrossConnectivity.Current.IsConnected)
            {
                // get api key from URL/api/v1/tokens - http basicauth - username + password

                try
                {
                    var apiKeyResponse = Helper.Rest.RestHelper.PerformRestGetRequestWithHttpBasicAuth(
                        ApplicationInstanceData.SerializedApplicationInstanceData.Url,
                        @"/api/v1/tokens",
                        Username,
                        Password);
                    // parse json response to get api key and store it
                    ApplicationInstanceData.SerializedApplicationInstanceData.ApiKey = Helper.Json.JsonFileHelper.ParseTokenResponse(apiKeyResponse);
                    Helper.Json.JsonFileHelper.SaveDataToJsonFile(ApplicationInstanceData.SerializedApplicationInstanceData, System.IO.Path.Combine(ApplicationInstanceData.PlatformDataPath, ApplicationInstanceData.ApplicationInstanceDataFileName));                    
                }
                catch (Exception e)
                {
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
