using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views.ContentPages;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels.ContentPageModels
{
    public class IncomeSourceViewContentPageModel : ViewModelBase
    {
        public IncomeSource IncomeSource { get; set; }

        public bool AllowEditDelete { get; set; }
        public GridLength EditDeleteRowHeight { get; set; }

        public Command EditCommand { get; set; }
        public Command DeleteCommand { get; set; }

        public IncomeSourceViewContentPageModel(ApplicationInstanceData applicationInstanceData, IncomeSource incomeSource)
        {
            ApplicationInstanceData = applicationInstanceData;
            IncomeSource = incomeSource;
            EditCommand = new Command(ExecuteEditCommand);
            DeleteCommand = new Command(ExecuteDeleteCommand);
            AllowEditDelete = !incomeSource.HasExternalId;
            EditDeleteRowHeight = AllowEditDelete ? 80 : 0;
        }

        private void ExecuteEditCommand()
        {
            // is it allowed?
            if (IncomeSource.HasExternalId) return;
            ApplicationInstanceData.NavigationPage.PushAsync(new IncomeSourceEditContentPage { BindingContext = new IncomeSourceEditContentPageModel(ApplicationInstanceData, IncomeSource) });
        }

        private async void ExecuteDeleteCommand()
        {
            // is it allowed?
            if (IncomeSource.HasExternalId) return;

            // are you sure?
            var actionDecision = await ApplicationInstanceData.App.MainPage.DisplayAlert(
                ApplicationInstanceData.SelectedLocalization.Translations[@"Confirm"],
                ApplicationInstanceData.SelectedLocalization.Translations[@"ConfirmationMessageDeleteIncomeSource"],
                ApplicationInstanceData.SelectedLocalization.Translations[@"OK"],
                ApplicationInstanceData.SelectedLocalization.Translations[@"Cancel"]);

            if (actionDecision)
            {
                ApplicationInstanceData.Data.DeleteIncomeSource(IncomeSource.InternalId);
                ApplicationInstanceData.Data.SaveChanges();
                await ApplicationInstanceData.NavigationPage.PopAsync();
            }
        }
    }
}
