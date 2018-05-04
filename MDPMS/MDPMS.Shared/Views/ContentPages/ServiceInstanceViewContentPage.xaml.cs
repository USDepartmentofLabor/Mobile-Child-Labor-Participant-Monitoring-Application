using MDPMS.Shared.ViewModels.ContentPageModels;
using Xamarin.Forms;

namespace MDPMS.Shared.Views.ContentPages
{
    public partial class ServiceInstanceViewContentPage : ContentPage
    {
        public ServiceInstanceViewContentPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = (ServiceInstanceViewContentPageModel)BindingContext;
            //var childView = new ServiceInstanceViewContentView();
            //var childViewModel = new ServiceInstanceViewContentViewModel(viewModel.ApplicationInstanceData, viewModel.Person);
            //childView.BindingContext = childViewModel;
            //this.ScrollView.Content = childView;
            //childView.OnAppearing();
        }
    }
}
