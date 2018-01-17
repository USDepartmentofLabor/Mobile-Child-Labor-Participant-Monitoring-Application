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

        public string NameOfProductOrService { get; set; }
        public int EstimatedVolumeProduced { get; set; }
        public int EstimatedVolumeSold { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal EstimatedIncome { get; set; }
        public string Currency { get; set; }

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
    }
}
