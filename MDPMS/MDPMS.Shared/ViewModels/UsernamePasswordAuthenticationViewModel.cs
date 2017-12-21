﻿using MDPMS.Shared.Models;
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
            // get api key from URL/api/v1/tokens - http basicauth - username + password      
            // TODO: on check connectivity also check if DPMS is reachable, separate msgs for internet vs. DPMS problems
            if (CrossConnectivity.Current.IsConnected)
            {
                //
                ApplicationInstanceData.App.MainPage.DisplayAlert(@"title", @"yes", @"accept", @"cancel");
            }
            else
            {
                //
                ApplicationInstanceData.App.MainPage.DisplayAlert(@"title", @"no", @"accept", @"cancel");
            }


        }
    }
}
