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
        public string Title { get; set; }
        public string SaveCommandVerb { get; set; }

        public Command CancelCommand { get; set; }
        public Command SaveCommand { get; set; }

        public IncomeSourceEditContentView IncomeSourceEditContentView { get; set; }
        public IncomeSourceEditContentViewModel IncomeSourceEditContentViewModel { get; set; }

        public IncomeSource IncomeSource { get; set; }

        public bool IsCreate { get; set; }
        public Household ParentHousehold { get; set; }

        public IncomeSourceEditContentPageModel(ApplicationInstanceData applicationInstanceData, Household parentHousehold)
        {
            IsCreate = true;
            SaveCommandVerb = applicationInstanceData.SelectedLocalization.Translations[@"Submit"];
            Title = applicationInstanceData.SelectedLocalization.Translations[@"AddIncomeSource"];
            ParentHousehold = parentHousehold;
            Init(applicationInstanceData);
        }

        public IncomeSourceEditContentPageModel(ApplicationInstanceData applicationInstanceData, IncomeSource incomeSource)
        {
            IsCreate = false;
            SaveCommandVerb = applicationInstanceData.SelectedLocalization.Translations[@"Save"];
            Title = applicationInstanceData.SelectedLocalization.Translations[@"EditIncomeSource"];
            IncomeSource = incomeSource;
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
