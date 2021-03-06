﻿using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views.ContentPages;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels.ContentPageModels
{
    public class LandingContentPageModel : ViewModelBase
    {
        public Command NavigateToLocalizationSelectionCommand { get; set; }
        public Command NavigateToMainContentCommand { get; set; }
        
        public LandingContentPageModel(ApplicationInstanceData applicationInstanceData)
        {
            NavigateToLocalizationSelectionCommand = new Command(ExecuteNavigateToLocalizationSelectionCommand);    
            NavigateToMainContentCommand = new Command(ExecuteNavigateToMainContentCommand);
            ApplicationInstanceData = applicationInstanceData;
        }

        private void ExecuteNavigateToLocalizationSelectionCommand()
        {
            Application.Current.MainPage = new LocalizationSelectionContentPage()
            {
                BindingContext = new LocalizationSelectionContentPageModel(ApplicationInstanceData)
            };
        }

        private void ExecuteNavigateToMainContentCommand()
        {
            // TODO: can determine initial set up status and navigate to st up wizard
            ApplicationInstanceData.NavigationPage = new NavigationPage(new MenuLandingContentPage
            {
                BindingContext = new MenuLandingContentPageModel(ApplicationInstanceData)
            });
            var rootPage = new RootPage
            {
                Master = new MenuContentPage
                {
                    BindingContext = new MenuContentPageModel(ApplicationInstanceData),
                    Title = ApplicationInstanceData.Title
                },
                Detail = ApplicationInstanceData.NavigationPage
            };
            ApplicationInstanceData.RootPage = rootPage;
            ApplicationInstanceData.App.MainPage = rootPage;
            ApplicationInstanceData.RootPage.IsPresented = true;
        }
    }
}
