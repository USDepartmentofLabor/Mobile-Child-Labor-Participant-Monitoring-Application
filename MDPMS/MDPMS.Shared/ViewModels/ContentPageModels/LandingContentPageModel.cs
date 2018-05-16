using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views;
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
            ApplicationInstanceData.NavigationPage = new NavigationPage(new HouseholdsSearchView
            {
                BindingContext = new HouseholdsSearchViewModel(ApplicationInstanceData)
            });
            var rootPage = new RootPage
            {
                Master = new MenuView
                {
                    BindingContext = new MenuViewModel(ApplicationInstanceData),
                    Title = ApplicationInstanceData.Title
                },
                Detail = ApplicationInstanceData.NavigationPage
            };
            ApplicationInstanceData.RootPage = rootPage;
            ApplicationInstanceData.App.MainPage = rootPage;
        }
    }
}
