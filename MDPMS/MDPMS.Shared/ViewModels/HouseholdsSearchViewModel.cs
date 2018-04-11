using System;
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
    public class HouseholdsSearchViewModel : ViewModelBase
    {
        public string SearchText { get; set; } = @"";
        public Command NavigateToAddNewHouseholdCommand { get; set; }
        public string HouseholdNoun { get; set; }

        public ObservableCollection<HouseholdSearchResultCellModel> Households { get; set; }

        private HouseholdSearchResultCellModel _selectedHousehold;
        public HouseholdSearchResultCellModel SelectedHousehold
        {
            get => _selectedHousehold;
            set
            {
                _selectedHousehold = value;
                if (_selectedHousehold == null) return;
                ApplicationInstanceData.NavigationPage.PushAsync(new HouseholdView
                {
                    BindingContext = new HouseholdViewModel(ApplicationInstanceData, value.Household)
                });
            }
        }

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

        public HouseholdsSearchViewModel(ApplicationInstanceData applicationInstanceData)
        {
            ApplicationInstanceData = applicationInstanceData;
            SearchCommand = new Command(LoadHouseholds);
            NavigateToAddNewHouseholdCommand = new Command(ExecuteNavigateToAddNewHouseholdCommand);
            LoadHouseholds();
        }
        
        private void LoadHouseholds()
        {
            Households = new ObservableCollection<HouseholdSearchResultCellModel>();
            var query = SearchText.Equals(string.Empty) ? ApplicationInstanceData.Data.Households : ApplicationInstanceData.Data.Households.Where(a => a.HouseholdName.Contains(SearchText));
            foreach (var household in query.OrderBy(a => a.HouseholdName))
            {
                Households.Add(new HouseholdSearchResultCellModel(household));
            }
            OnPropertyChanged(nameof(Households));
            OnPropertyChanged(nameof(SelectedHousehold));

            HouseholdNoun = Households.Count().Equals(1) ?
                ApplicationInstanceData.SelectedLocalization.Translations[@"Household"] :
                ApplicationInstanceData.SelectedLocalization.Translations[@"Households"];
        }

        private void ExecuteNavigateToAddNewHouseholdCommand()
        {
            if (!ApplicationInstanceData.WasInitialSyncPerformed())
            {                
                ApplicationInstanceData.App.MainPage.DisplayAlert(
                    ApplicationInstanceData.SelectedLocalization.Translations[@"Alert"],
                    ApplicationInstanceData.SelectedLocalization.Translations[@"AlertMessageActionNotAllowedUntilInitialSyncIsPerformed"],
                    ApplicationInstanceData.SelectedLocalization.Translations[@"OK"]);
                return;
            }

            ApplicationInstanceData.GoToView(new HouseholdIntakeView               
            {                   
                BindingContext = new HouseholdIntakeViewModel(ApplicationInstanceData)               
            });
        }
    }

    public class HouseholdSearchResultCellModel
    {
        public Household Household { get; set; }
        public int BeneficiaryCount { get; set; }
        public int AdultCount { get; set; }

        public HouseholdSearchResultCellModel(Household household)
        {
            Household = household;
            BeneficiaryCount = household.Members.Count(a => a.IsBeneficiary());
            AdultCount = household.Members.Count(a => a.IsInAgeRaneBasedOnDate(DateTime.Now, 18, 200));
        }
    }
}
