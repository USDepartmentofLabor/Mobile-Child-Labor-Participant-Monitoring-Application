using System.Collections.ObjectModel;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views.ContentPages;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels.ContentPageModels
{
    public class LocalizationSelectionContentPageModel : ViewModelBase
    {                
        public ObservableCollection<Localization> Localizations { get; set; }
        public Localization SelectedLocalization { get; set; }

        public Command NavigateToLandingPageCommand { get; set; }
        public Command NavigateToLandingPageCheckSelectionCommand { get; set; }

        public LocalizationSelectionContentPageModel(ApplicationInstanceData applicationInstanceData)
        {
            NavigateToLandingPageCommand = new Command(() => ExecuteNavigateToLandingPageCommand());
            NavigateToLandingPageCheckSelectionCommand = new Command(() => ExecuteNavigateToLandingPageCheckSelectionCommand());
            ApplicationInstanceData = applicationInstanceData;                     
            Localizations = new ObservableCollection<Localization>();
            foreach (var availableLocalization in ApplicationInstanceData.AvailableLocalizations)
            {
                Localizations.Add(availableLocalization);
            }
            SelectedLocalization = ApplicationInstanceData.SelectedLocalization;
        }
        
        private void ExecuteNavigateToLandingPageCommand()
        {            
            Application.Current.MainPage = new LandingContentPage
            {
                BindingContext = new LandingContentPageModel(ApplicationInstanceData)
            };
        }

        private void ExecuteNavigateToLandingPageCheckSelectionCommand()
        {
            if (!ApplicationInstanceData.SelectedLocalization.Abbreviation.Equals(SelectedLocalization.Abbreviation))
            {
                ApplicationInstanceData.SetLocalization(SelectedLocalization.Abbreviation);
            }

            Application.Current.MainPage = new LandingContentPage
            {
                BindingContext = new LandingContentPageModel(ApplicationInstanceData)
            };
        }
    }
}
