using MDPMS.Shared.ViewModels.ContentPageModels;
using MDPMS.Shared.ViewModels.ContentViewModels;
using MDPMS.Shared.Views.ContentViews;
using Xamarin.Forms;

namespace MDPMS.Shared.Views.ContentPages
{
    public partial class ServiceInstanceEditContentPage : ContentPage
    {
        public ServiceInstanceEditContentPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = (ServiceInstanceEditContentPageModel)BindingContext;

            if (viewModel.IsCreate)
            {
                viewModel.ServiceInstanceEditContentViewModel = new ServiceInstanceEditContentViewModel(viewModel.ApplicationInstanceData, viewModel.ParentPerson);
            }
            else
            {
                viewModel.ServiceInstanceEditContentViewModel = new ServiceInstanceEditContentViewModel(viewModel.ApplicationInstanceData, viewModel.ServiceInstance);
            }

            viewModel.ServiceInstanceEditContentView = new ServiceInstanceEditContentView();
            viewModel.ServiceInstanceEditContentView.BindingContext = viewModel.ServiceInstanceEditContentViewModel;

            Scrollview.Content = viewModel.ServiceInstanceEditContentView;
        }
    }
}
