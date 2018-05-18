using MDPMS.Shared.ViewModels.ContentViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MDPMS.Shared.Views.ContentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IncomeSourcesViewContentView : ContentView
    {
        public IncomeSourcesViewContentView()
        {
            InitializeComponent();
        }

        public void OnAppearing()
        {
            // fix for list view height being too tall
            var viewModel = (IncomeSourcesViewContentViewModel)BindingContext;
            var incomeSourceCount = (viewModel.Household.IncomeSources == null) ? 0 : viewModel.Household.IncomeSources.Count;
            var gridHeight = 30.0 + (45.0 * incomeSourceCount);
            ListViewRowDefinition.Height = new GridLength(gridHeight, GridUnitType.Absolute);
        }
    }
}
