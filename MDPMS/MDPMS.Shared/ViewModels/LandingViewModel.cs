using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels
{
    public class LandingViewModel : ViewModelBase
    {
        public Command NavigateToLocalizationSelectionCommand { get; set; }
        
        public LandingViewModel(ApplicationInstanceData applicationInstanceData)
        {
            NavigateToLocalizationSelectionCommand = new Command(() => ExecuteNavigateToLocalizationSelectionCommand());            
            ApplicationInstanceData = applicationInstanceData;
        }

        private void ExecuteNavigateToLocalizationSelectionCommand()
        {
            Application.Current.MainPage = new LocalizationSelectionView()
            {
                BindingContext = new LocalizationSelectionViewModel(ApplicationInstanceData)
            };
        }        
    }
}
