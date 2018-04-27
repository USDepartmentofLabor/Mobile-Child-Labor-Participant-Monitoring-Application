using MDPMS.Shared.ViewModels.ContentViewModels;
using Xamarin.Forms;

namespace MDPMS.Shared.Views.ContentViews
{
    public partial class HouseholdMembersViewContentView : ContentView
    {
        public HouseholdMembersViewContentView()
        {
            InitializeComponent();
        }

        public void OnAppearing()
        {
            // fix for list view height being too tall
            var viewModel = (HouseholdMembersViewContentViewModel)BindingContext;
            var objectCount = (viewModel.Household.Members == null) ? 0 : viewModel.Household.Members.Count;
            var gridHeight = 30.0 + (45.0 * objectCount);
            ListViewRowDefinition.Height = new GridLength(gridHeight, GridUnitType.Absolute);
        }
    }
}
