using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.ViewModels.ContentPageModels;
using MDPMS.Shared.Views.ContentPages;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels
{
    public class HouseholdsSearchViewModel : ViewModelBase
    {
        public string SearchText { get; set; } = @"";
        public Command NavigateToAddNewHouseholdCommand { get; set; }
        public string HouseholdNoun { get; set; }

        public ObservableCollection<HouseholdSearchResultCellModel> Households { get; set; }

        public int? HouseholdInternalIdTarget { get; set; } = null;

        private HouseholdSearchResultCellModel _selectedHousehold;
        public HouseholdSearchResultCellModel SelectedHousehold
        {
            get => _selectedHousehold;
            set
            {
                _selectedHousehold = value;
                if (_selectedHousehold == null) return;
                ApplicationInstanceData.NavigationPage.PushAsync(new HouseholdViewContentPage
                {
                    BindingContext = new HouseholdViewContentPageModel(ApplicationInstanceData, value.Household)
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

        public void ExecuteAppearingCommand()
        {
            RefreshCommand.Execute(null);
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

            GoToHouseholdView();
        }

        private void GoToHouseholdView()
        {
            // on refresh send user to a specified household view if HouseholdInternalIdTarget set
            if (HouseholdInternalIdTarget != null)
            {
                var search = Households.Where(a => a.Household.InternalId == HouseholdInternalIdTarget).ToList();
                if (search.Any()) _selectedHousehold = search.First();
                HouseholdInternalIdTarget = null;
                OnPropertyChanged(nameof(SelectedHousehold));
            }
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

            ApplicationInstanceData.NavigationPage.PushAsync(new HouseholdEditContentPage
            {
                BindingContext = new HouseholdEditContentPageModel(ApplicationInstanceData)
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
            var now = DateTime.Now;
            BeneficiaryCount = household.Members == null ? 0 : household.Members.Count(a => a.IsYouthNow(now));
            AdultCount = household.Members == null ? 0 : household.Members.Count(a => !a.IsYouthNow(now));
        }
    }
}
