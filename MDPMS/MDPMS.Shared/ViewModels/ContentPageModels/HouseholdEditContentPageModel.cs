using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.ViewModels.ContentViewModels;
using MDPMS.Shared.Views.ContentViews;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels.ContentPageModels
{
    public class HouseholdEditContentPageModel : ViewModelBase
    {
        public Command CancelCommand { get; set; }
        public Command SaveCommand { get; set; }

        public HouseholdEditContentView HouseholdEditContentView { get; set; }
        public HouseholdEditContentViewModel HouseholdEditContentViewModel { get; set; }

        public Household Household { get; set; }

        public HouseholdEditContentPageModel(ApplicationInstanceData applicationInstanceData, Household household)
        {
            ApplicationInstanceData = applicationInstanceData;

            CancelCommand = new Command(ExecuteCancelCommand);
            SaveCommand = new Command(ExecuteSaveCommand);

            Household = household;
        }

        private void ExecuteCancelCommand()
        {            
            CloseView();
        }

        private void ExecuteSaveCommand()
        {
            var validation = HouseholdEditContentViewModel.ValidateHousehold();
            if (validation)
            {
                HouseholdEditContentViewModel.SaveHousehold();
                CloseView();
            }
        }

        private void CloseView()
        {
            ApplicationInstanceData.NavigationPage.PopAsync();
        }
    }
}
