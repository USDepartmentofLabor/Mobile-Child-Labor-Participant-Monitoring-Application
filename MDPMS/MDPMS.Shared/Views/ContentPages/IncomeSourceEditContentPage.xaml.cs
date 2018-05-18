using MDPMS.Shared.ViewModels.ContentPageModels;
using MDPMS.Shared.ViewModels.ContentViewModels;
using MDPMS.Shared.Views.ContentViews;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MDPMS.Shared.Views.ContentPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IncomeSourceEditContentPage : ContentPage
    {
        public IncomeSourceEditContentPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = (IncomeSourceEditContentPageModel)BindingContext;

            if (viewModel.IsCreate)
            {
                viewModel.IncomeSourceEditContentViewModel = new IncomeSourceEditContentViewModel(viewModel.ApplicationInstanceData, viewModel.ParentHousehold);
            }
            else
            {
                viewModel.IncomeSourceEditContentViewModel = new IncomeSourceEditContentViewModel(viewModel.ApplicationInstanceData, viewModel.IncomeSource);
            }

            viewModel.IncomeSourceEditContentView = new IncomeSourceEditContentView();
            viewModel.IncomeSourceEditContentView.BindingContext = viewModel.IncomeSourceEditContentViewModel;

            Scrollview.Content = viewModel.IncomeSourceEditContentView;
        }
    }
}
