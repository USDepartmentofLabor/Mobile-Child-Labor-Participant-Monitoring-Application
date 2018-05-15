using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.ViewModels.ContentViewModels;
using MDPMS.Shared.Views;
using MDPMS.Shared.Views.ContentViews;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels.ContentPageModels
{
    public class HouseholdEditContentPageModel : ViewModelBase
    {
        public string Title { get; set; }
        public string SaveCommandVerb { get; set; }

        public Command CancelCommand { get; set; }
        public Command SaveCommand { get; set; }

        public HouseholdEditContentView HouseholdEditContentView { get; set; }
        public HouseholdEditContentViewModel HouseholdEditContentViewModel { get; set; }

        public Household Household { get; set; }

        public bool IsCreate { get; set; }

        public HouseholdEditContentPageModel(ApplicationInstanceData applicationInstanceData)
        {
            IsCreate = true;
            SaveCommandVerb = applicationInstanceData.SelectedLocalization.Translations[@"Submit"];
            Title = applicationInstanceData.SelectedLocalization.Translations[@"AddNewHousehold"];
            Init(applicationInstanceData);
        }

        public HouseholdEditContentPageModel(ApplicationInstanceData applicationInstanceData, Household household)
        {
            IsCreate = false;
            SaveCommandVerb = applicationInstanceData.SelectedLocalization.Translations[@"Save"];
            Title = applicationInstanceData.SelectedLocalization.Translations[@"EditHousehold"];
            Household = household;
            Init(applicationInstanceData);
        }

        private void Init(ApplicationInstanceData applicationInstanceData)
        {
            ApplicationInstanceData = applicationInstanceData;

            CancelCommand = new Command(ExecuteCancelCommand);
            SaveCommand = new Command(ExecuteSaveCommand);
        }

        private void ExecuteCancelCommand()
        {            
            CloseView();
        }

        private async void ExecuteSaveCommand()
        {
            IsBusy = true;
            var validation = HouseholdEditContentViewModel.ValidateHousehold();
            if (validation)
            {
                await HouseholdEditContentViewModel.Save();
                CloseView();
            }
            IsBusy = false;
        }

        private void CloseView()
        {
            if (IsCreate)
            {
                // Should be HouseholdsSearchView + Model, send user to the household they just created if new
                var currentParentView = ApplicationInstanceData.NavigationPage.RootPage;
                if (currentParentView.GetType() == typeof(HouseholdsSearchView))
                {
                    var viewModel = (HouseholdsSearchViewModel)currentParentView.BindingContext;
                    viewModel.HouseholdInternalIdTarget = HouseholdEditContentViewModel.Household.InternalId;
                }
            }
            ApplicationInstanceData.NavigationPage.PopAsync();
        }
    }
}
