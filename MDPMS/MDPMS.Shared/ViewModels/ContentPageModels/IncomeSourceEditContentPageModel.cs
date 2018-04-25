using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.ViewModels.ContentViewModels;
using MDPMS.Shared.Views.ContentViews;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels.ContentPageModels
{
    public class IncomeSourceEditContentPageModel : ViewModelBase
    {
        

        public Command CancelCommand { get; set; }
        public Command SaveCommand { get; set; }

        public IncomeSourceEditContentView IncomeSourceEditContentView { get; set; }
        public IncomeSourceEditContentViewModel IncomeSourceEditContentViewModel { get; set; }

        public IncomeSource IncomeSource { get; set; }


        public IncomeSourceEditContentPageModel(ApplicationInstanceData applicationInstanceData, IncomeSource incomeSource)
        {
            ApplicationInstanceData = applicationInstanceData;

            CancelCommand = new Command(ExecuteCancelCommand);
            SaveCommand = new Command(ExecuteSaveCommand);

            IncomeSource = incomeSource;
        }

        private void ExecuteCancelCommand()
        {
            CloseView();
        }

        private void ExecuteSaveCommand()
        {
            var validation = IncomeSourceEditContentViewModel.ValidateIncomeSource();
            if (validation)
            {
                IncomeSourceEditContentViewModel.SaveIncomeSource();
                CloseView();
            }
        }

        private void CloseView()
        {
            ApplicationInstanceData.NavigationPage.PopAsync();
        }
    }
}
