using MDPMS.Shared.ViewModels.ContentPageModels;
using MDPMS.Shared.ViewModels.ContentViewModels;
using MDPMS.Shared.Views.ContentViews;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MDPMS.Shared.Views.ContentPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonFollowUpViewContentPage : ContentPage
    {
        public PersonFollowUpViewContentPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = (PersonFollowUpViewContentPageModel)BindingContext;
            var childView = new PersonFollowUpViewContentView();
            var childViewModel = new PersonFollowUpViewContentViewModel(viewModel.ApplicationInstanceData, viewModel.PersonFollowUp);
            childView.BindingContext = childViewModel;
            this.ScrollView.Content = childView;
            childView.OnAppearing();
        }
    }
}
