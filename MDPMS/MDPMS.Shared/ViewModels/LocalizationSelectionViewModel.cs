using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels
{
    public class LocalizationSelectionViewModel : ViewModelBase
    {                
        public ObservableCollection<Localization> Localizations { get; set; }
        public Localization SelectedLocalization { get; set; }

        public Command NavigateToLandingPageCommand { get; set; }
        public Command NavigateToLandingPageCheckSelectionCommand { get; set; }

        public LocalizationSelectionViewModel(ApplicationInstanceData applicationInstanceData)
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
            Application.Current.MainPage = new LandingView
            {
                BindingContext = new LandingViewModel(ApplicationInstanceData)
            };
        }

        private void ExecuteNavigateToLandingPageCheckSelectionCommand()
        {
            if (!ApplicationInstanceData.SelectedLocalization.Abbreviation.Equals(SelectedLocalization.Abbreviation))
            {
                ApplicationInstanceData.SetLocalization(SelectedLocalization.Abbreviation);
            }

            Application.Current.MainPage = new LandingView
            {
                BindingContext = new LandingViewModel(ApplicationInstanceData)
            };
        }
    }
}
