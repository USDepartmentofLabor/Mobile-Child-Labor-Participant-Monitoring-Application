using MDPMS.Shared.ViewModels.ContentViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MDPMS.Shared.Views.ContentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServiceInstancesViewContentView : ContentView
    {
        public ServiceInstancesViewContentView()
        {
            InitializeComponent();
        }

        public void OnAppearing()
        {
            // fix for list view height being too tall
            var viewModel = (ServiceInstancesViewContentViewModel)BindingContext;
            var objectCount = (viewModel.Person.ServiceInstances == null) ? 0 : viewModel.Person.ServiceInstances.Count;
            var gridHeight = 30.0 + (45.0 * objectCount);
            ListViewRowDefinition.Height = new GridLength(gridHeight, GridUnitType.Absolute);
        }
    }
}
