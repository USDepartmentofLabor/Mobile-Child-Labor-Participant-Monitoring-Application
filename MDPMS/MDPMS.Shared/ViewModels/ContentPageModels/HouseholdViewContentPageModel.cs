using System.Linq;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels.ContentPageModels
{
    public class HouseholdViewContentPageModel : ViewModelBase
    {
        public Household Household { get; set; }

        public bool AllowEditDelete { get; set; }
        public Xamarin.Forms.GridLength EditDeleteRowHeight { get; set; }

        public Command DeleteCommand { get; set; }

        public HouseholdViewContentPageModel(ApplicationInstanceData applicationInstanceData, Household household)
        {
            ApplicationInstanceData = applicationInstanceData;
            Household = household;
            DeleteCommand = new Command(ExecuteDeleteCommand);
            AllowEditDelete = !household.HasExternalId;
            EditDeleteRowHeight = AllowEditDelete ? 80 : 0;
        }

        private async void ExecuteDeleteCommand()
        {
            // is it allowed?
            if (Household.HasExternalId) return;

            // are you sure?  all children will be deleted as well
            var actionDecision = await ApplicationInstanceData.App.MainPage.DisplayAlert(
                ApplicationInstanceData.SelectedLocalization.Translations[@"Confirm"],
                ApplicationInstanceData.SelectedLocalization.Translations[@"ConfirmationMessageDeleteHousehold"],
                ApplicationInstanceData.SelectedLocalization.Translations[@"OK"],
                ApplicationInstanceData.SelectedLocalization.Translations[@"Cancel"]);

            if (actionDecision)
            {
                ApplicationInstanceData.Data.CustomHouseholdValues
                    .RemoveRange(ApplicationInstanceData.Data.CustomHouseholdValues.Where(a => a.Household.InternalId == Household.InternalId));
                ApplicationInstanceData.Data.Households.Remove(Household);
                ApplicationInstanceData.Data.SaveChanges();
                ApplicationInstanceData.GoToView(new HouseholdsSearchView { BindingContext = new HouseholdsSearchViewModel(ApplicationInstanceData) });
            }
        }
    }
}
