using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.ViewModels.ContentPageModels;
using MDPMS.Shared.Views.ContentPages;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels.ContentViewModels
{
    public class IncomeSourcesViewContentViewModel : ViewModelBase
    {
        public Command AddIncomeSourceCommand { get; set; }

        public Household Household { get; set; }

        private IncomeSource _selectedIncomeSource;
        public IncomeSource SelectedIncomeSource
        {
            get => _selectedIncomeSource;
            set
            {
                _selectedIncomeSource = value;
                if (_selectedIncomeSource == null) return;
                ApplicationInstanceData.NavigationPage.PushAsync(new IncomeSourceViewContentPage
                {
                    BindingContext = new IncomeSourceViewContentPageModel(ApplicationInstanceData, value)
                });
            }
        }

        public IncomeSourcesViewContentViewModel(ApplicationInstanceData applicationInstanceData, Household household)
        {
            ApplicationInstanceData = applicationInstanceData;
            Household = household;
            AddIncomeSourceCommand = new Command(ExecuteAddIncomeSourceCommand);
        }

        private void ExecuteAddIncomeSourceCommand()
        {            
            ApplicationInstanceData.NavigationPage.PushAsync(new IncomeSourceEditContentPage { BindingContext = new IncomeSourceEditContentPageModel(ApplicationInstanceData, Household) });
        }
    }
}
