using MDPMS.Shared.ViewModels.ContentPageModels;
using MDPMS.Shared.ViewModels.ContentViewModels;
using MDPMS.Shared.Views.ContentViews;
using Xamarin.Forms;

namespace MDPMS.Shared.Views.ContentPages
{
    public partial class HouseholdEditContentPage : ContentPage
    {
        public HouseholdEditContentPage()
        {
            InitializeComponent();
        }

		protected override void OnAppearing()
		{
			base.OnAppearing();

            var viewModel = (HouseholdEditContentPageModel)BindingContext;
            viewModel.HouseholdEditContentViewModel = new HouseholdEditContentViewModel(viewModel.ApplicationInstanceData, viewModel.Household);
            viewModel.HouseholdEditContentView = new HouseholdEditContentView();
            viewModel.HouseholdEditContentView.BindingContext = viewModel.HouseholdEditContentViewModel;

            Scrollview.Content = viewModel.HouseholdEditContentView;
            viewModel.HouseholdEditContentView.OnAppearing(true);
		}
	}
}
