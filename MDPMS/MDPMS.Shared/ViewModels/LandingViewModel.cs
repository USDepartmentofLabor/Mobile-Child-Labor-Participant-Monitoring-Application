using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels
{
    public class LandingViewModel : ViewModelBase
    {
        public Command NavigateToLocalizationSelectionCommand { get; set; }
        public Command NavigateToMainContentCommand { get; set; }
        
        public LandingViewModel(ApplicationInstanceData applicationInstanceData)
        {
            NavigateToLocalizationSelectionCommand = new Command(ExecuteNavigateToLocalizationSelectionCommand);    
            NavigateToMainContentCommand = new Command(ExecuteNavigateToMainContentCommand);
            ApplicationInstanceData = applicationInstanceData;
        }

        private void ExecuteNavigateToLocalizationSelectionCommand()
        {
            Application.Current.MainPage = new LocalizationSelectionView()
            {
                BindingContext = new LocalizationSelectionViewModel(ApplicationInstanceData)
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
