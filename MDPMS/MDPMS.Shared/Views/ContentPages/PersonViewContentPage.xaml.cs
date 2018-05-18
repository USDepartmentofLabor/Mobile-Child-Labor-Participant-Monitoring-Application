using MDPMS.Shared.ViewModels.ContentPageModels;
using MDPMS.Shared.ViewModels.ContentViewModels;
using MDPMS.Shared.Views.ContentViews;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MDPMS.Shared.Views.ContentPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonViewContentPage : ContentPage
    {
        public PersonViewContentPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = (PersonViewContentPageModel)BindingContext;
            var childView = new PersonViewContentView();
            var childViewModel = new PersonViewContentViewModel(viewModel.ApplicationInstanceData, viewModel.Person);
            childView.BindingContext = childViewModel;
            this.ScrollView.Content = childView;
            childView.OnAppearing();
        }
    }
}
