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
        public int? EstimatedVolumeProduced { get; set; }
        public int? EstimatedVolumeSold { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal? EstimatedIncome { get; set; }
        public string Currency { get; set; }

        public IncomeSourceEditContentViewModel(ApplicationInstanceData applicationInstanceData, IncomeSource incomeSource)
        {
            ApplicationInstanceData = applicationInstanceData;
            IncomeSource = incomeSource;

            // set temp/locally cached properties to support save/cancel
            ProductServiceName = IncomeSource.ProductServiceName;
            EstimatedVolumeProduced = IncomeSource.EstimatedVolumeProduced;
            EstimatedVolumeSold = IncomeSource.EstimatedVolumeSold;
            UnitOfMeasure = IncomeSource.UnitOfMeasure;
            EstimatedIncome = IncomeSource.EstimatedIncome;
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
            IncomeSource.LastUpdatedAt = DateTime.UtcNow;

            ApplicationInstanceData.Data.SaveChanges();
        }
    }
}
