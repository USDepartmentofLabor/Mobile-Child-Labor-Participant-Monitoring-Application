using MDPMS.Shared.ViewModels.ContentPageModels;
using MDPMS.Shared.ViewModels.ContentViewModels;
using MDPMS.Shared.Views.ContentViews;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MDPMS.Shared.Views.ContentPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HouseholdViewContentPage : ContentPage
    {
        public HouseholdViewContentPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = (HouseholdViewContentPageModel)BindingContext;
            var childView = new HouseholdViewContentView();
            var childViewModel = new HouseholdViewContentViewModel(viewModel.ApplicationInstanceData, viewModel.Household);
            childView.BindingContext = childViewModel;
            this.ScrollView.Content = childView;
            childView.OnAppearing();
        }
    }
}
