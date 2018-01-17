using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels
{
    public class HouseholdsViewModel : ViewModelBase
    {
        public string SearchText { get; set; } = @"";
        public ObservableCollection<Household> Households { get; set; }
        public Household SelectedHousehold { get; set; } = null;
        public Command NavigateToAddNewHouseholdCommand { get; set; }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public Command RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    await Task.Run(() => { LoadHouseholds(); });
                    IsRefreshing = false;
                });
            }
        }

        public Command SearchCommand { get; set; }

        public HouseholdsViewModel(ApplicationInstanceData applicationInstanceData)
        {
            ApplicationInstanceData = applicationInstanceData;
            SearchCommand = new Command(LoadHouseholds);
            NavigateToAddNewHouseholdCommand = new Command(ExecuteNavigateToAddNewHouseholdCommand);
            LoadHouseholds();
        }
        
        private void LoadHouseholds()
        {
            Households = new ObservableCollection<Household>();
            var query = SearchText.Equals(string.Empty) ? ApplicationInstanceData.Data.Households : ApplicationInstanceData.Data.Households.Where(a => a.HouseholdName.Contains(SearchText));
            foreach (var household in query.OrderBy(a => a.HouseholdName))
            {
                Households.Add(household);
            }
            if (Households.Any()) SelectedHousehold = Households.First();
            OnPropertyChanged(nameof(Households));
            OnPropertyChanged(nameof(SelectedHousehold));
        }

        private void ExecuteNavigateToAddNewHouseholdCommand()
        {
            ApplicationInstanceData.GoToView(new HouseholdIntakeView
                {
                    BindingContext = new HouseholdIntakeViewModel(ApplicationInstanceData)
                });
        }
    }
}
