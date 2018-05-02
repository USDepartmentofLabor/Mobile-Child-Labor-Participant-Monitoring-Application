using Xamarin.Forms;

namespace MDPMS.Shared.Views.ContentPages
{
    public partial class PersonFollowUpViewContentPage : ContentPage
    {
        public PersonFollowUpViewContentPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //var viewModel = (PersonFollowUpViewContentPageModel)BindingContext;
            //var childView = new PersonFollowUpViewContentView();
            //var childViewModel = new PersonFollowUpViewContentViewModel(viewModel.ApplicationInstanceData, viewModel.PersonFollowUp);
            //childView.BindingContext = childViewModel;
            //this.ScrollView.Content = childView;
            //childView.OnAppearing();
        }
    }
}
