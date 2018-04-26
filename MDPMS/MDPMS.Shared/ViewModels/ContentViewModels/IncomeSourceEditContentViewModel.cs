using System;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;

namespace MDPMS.Shared.ViewModels.ContentViewModels
{
    public class IncomeSourceEditContentViewModel : ViewModelBase
    {
        public IncomeSource IncomeSource { get; set; }

        // temp/locally cached properties
        public string ProductServiceName { get; set; }
        public int EstimatedVolumeProduced { get; set; }
        public int EstimatedVolumeSold { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal EstimatedIncome { get; set; }
        public string Currency { get; set; }

        private readonly bool isCreate;
        private readonly Household household;

        public IncomeSourceEditContentViewModel(ApplicationInstanceData applicationInstanceData, Household parentHousehold)
        {
            isCreate = true;
            household = parentHousehold;
            Init(applicationInstanceData);
        }

        public IncomeSourceEditContentViewModel(ApplicationInstanceData applicationInstanceData, IncomeSource incomeSource)
        {
            IncomeSource = incomeSource;
            isCreate = false;
            Init(applicationInstanceData);
        }

        private void Init(ApplicationInstanceData applicationInstanceData)
        {
            ApplicationInstanceData = applicationInstanceData;
            if (isCreate)
            {
                IncomeSource = new IncomeSource
                {
                    ProductServiceName = @"",
                    EstimatedVolumeProduced = 0,
                    EstimatedVolumeSold = 0,
                    UnitOfMeasure = @"",
                    EstimatedIncome = 0.0m,
                    Currency = @""
                };

            }

            // set temp/locally cached properties to support save/cancel
            ProductServiceName = IncomeSource.ProductServiceName;
            EstimatedVolumeProduced = (IncomeSource.EstimatedVolumeProduced == null) ? 0 : (int)IncomeSource.EstimatedVolumeProduced;
            EstimatedVolumeSold = (IncomeSource.EstimatedVolumeSold == null) ? 0 : (int)IncomeSource.EstimatedVolumeSold;
            UnitOfMeasure = IncomeSource.UnitOfMeasure;
            EstimatedIncome = (IncomeSource.EstimatedIncome == null) ? 0.0m : (decimal)IncomeSource.EstimatedIncome;
            Currency = IncomeSource.Currency;
        }

        public bool ValidateIncomeSource()
        {
            // TODO: More "cheatable" char checks or find std regex
            var validateableName = ProductServiceName.Replace(" ", "");
            if (validateableName.Equals(string.Empty))
            {
                ApplicationInstanceData.App.MainPage.DisplayAlert(
                    ApplicationInstanceData.SelectedLocalization.Translations[@"Error"],
                    ApplicationInstanceData.SelectedLocalization.Translations[@"ErrorIncomeSourceNameCanNotBeBlank"],
                    ApplicationInstanceData.SelectedLocalization.Translations[@"OK"]);
                return false;
            }
            return true;
        }

        public void SaveIncomeSource()
        {
            // existing income source
            IncomeSource.ProductServiceName = ProductServiceName;
            IncomeSource.EstimatedVolumeProduced = EstimatedVolumeProduced;
            IncomeSource.EstimatedVolumeSold = EstimatedVolumeSold;
            IncomeSource.UnitOfMeasure = UnitOfMeasure;
            IncomeSource.EstimatedIncome = EstimatedIncome;
            IncomeSource.Currency = Currency;

            var now = DateTime.UtcNow;
            IncomeSource.LastUpdatedAt = now;

            if (isCreate)
            {
                IncomeSource.ExternalId = null;
                IncomeSource.CreatedAt = null;
                IncomeSource.SoftDeleted = false;
                household.AddIncomeSource(IncomeSource);
            }

            ApplicationInstanceData.Data.SaveChanges();
        }
    }
}
