using System.Linq;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels
{
    public class IncomeSourceAddViewModel : ViewModelBase
    {
        public Command CancelCommand { get; set; }
        public Command SubmitCommand { get; set; }

        public string NameOfProductOrService { get; set; } = @"";
        public int EstimatedVolumeProduced { get; set; } = 0;
        public int EstimatedVolumeSold { get; set; } = 0;
        public string UnitOfMeasure { get; set; } = @"";
        public decimal EstimatedIncome { get; set; } = 0.0m;
        public string Currency { get; set; } = @"";

        public IncomeSourceAddViewModel(ApplicationInstanceData applicationInstanceData)
        {
            ApplicationInstanceData = applicationInstanceData;
            CancelCommand = new Command(ExecuteCancelCommand);
            SubmitCommand = new Command(ExecuteSubmitCommand);
        }

        private void ExecuteCancelCommand()
        {
            Exit();
        }

        private void ExecuteSubmitCommand()
        {
            if (!NewValidation()) return;
            var incomeSource = new IncomeSource
            {
                ExternalId = null,
                CreatedAt = null,
                LastUpdatedAt = null,
                SoftDeleted = false,
                ProductServiceName = NameOfProductOrService,
                EstimatedVolumeProduced = EstimatedVolumeProduced,
                EstimatedVolumeSold = EstimatedVolumeSold,
                UnitOfMeasure = UnitOfMeasure,
                EstimatedIncome = EstimatedIncome,
                Currency = Currency
            };
            if (ApplicationInstanceData.NavigationPage.Pages.First().GetType() == typeof(HouseholdIntakeView))
            {
                var householdsViewModel = (HouseholdIntakeViewModel)ApplicationInstanceData.NavigationPage.Pages.First().BindingContext;
                householdsViewModel.IncomeSources.Add(incomeSource);
            }            
            Exit();
        }

        private void Exit()
        {
            // Back to add household view
            ApplicationInstanceData.NavigationPage.PopAsync();
        }

        private bool NewValidation()
        {
            // TODO: More "cheatable" char checks or find std regex
            var validateableName = NameOfProductOrService.Replace(" ", "");
            if (validateableName.Equals(string.Empty))
            {
                ApplicationInstanceData.App.MainPage.DisplayAlert(
                    ApplicationInstanceData.SelectedLocalization.Translations[@"Error"],
                    ApplicationInstanceData.SelectedLocalization.Translations[@"ErrorNameCanNotBeBlank"],
                    ApplicationInstanceData.SelectedLocalization.Translations[@"OK"]);
                return false;
            }
            return true;
        }
    }
}
