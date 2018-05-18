using MDPMS.Shared.ViewModels.ContentPageModels;
using MDPMS.Shared.ViewModels.ContentViewModels;
using MDPMS.Shared.Views.ContentViews;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MDPMS.Shared.Views.ContentPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
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

            if (viewModel.IsCreate)
            {
                viewModel.HouseholdEditContentViewModel = new HouseholdEditContentViewModel(viewModel.ApplicationInstanceData);
            }
            else
            {
                viewModel.HouseholdEditContentViewModel = new HouseholdEditContentViewModel(viewModel.ApplicationInstanceData, viewModel.Household);
            }

            viewModel.HouseholdEditContentView = new HouseholdEditContentView();
            viewModel.HouseholdEditContentView.BindingContext = viewModel.HouseholdEditContentViewModel;

            Scrollview.Content = viewModel.HouseholdEditContentView;
            viewModel.HouseholdEditContentView.OnAppearing(!viewModel.IsCreate);
		}
	}
}
