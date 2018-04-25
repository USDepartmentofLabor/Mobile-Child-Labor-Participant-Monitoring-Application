using MDPMS.Shared.ViewModels.ContentPageModels;
using MDPMS.Shared.ViewModels.ContentViewModels;
using MDPMS.Shared.Views.ContentViews;
using Xamarin.Forms;

namespace MDPMS.Shared.Views.ContentPages
{
    public partial class IncomeSourceViewContentPage : ContentPage
    {
        public IncomeSourceViewContentPage()
        {
            InitializeComponent();
        }

		protected override void OnAppearing()
		{
			base.OnAppearing();

            var viewModel = (IncomeSourceViewContentPageModel)BindingContext;
            var childView = new IncomeSourceViewContentView();
            var childViewModel = new IncomeSourceViewContentViewModel(viewModel.ApplicationInstanceData, viewModel.IncomeSource);
            childView.BindingContext = childViewModel;
            this.ScrollView.Content = childView;
		}
	}
}
