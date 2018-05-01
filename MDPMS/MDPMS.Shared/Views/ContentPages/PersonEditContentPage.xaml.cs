using MDPMS.Shared.ViewModels.ContentPageModels;
using MDPMS.Shared.ViewModels.ContentViewModels;
using MDPMS.Shared.Views.ContentViews;
using Xamarin.Forms;

namespace MDPMS.Shared.Views.ContentPages
{
    public partial class PersonEditContentPage : ContentPage
    {
        public PersonEditContentPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = (PersonEditContentPageModel)BindingContext;

            if (viewModel.IsCreate)
            {
                viewModel.PersonEditContentViewModel = new PersonEditContentViewModel(viewModel.ApplicationInstanceData, viewModel.ParentHousehold);
            }
            else
            {
                viewModel.PersonEditContentViewModel = new PersonEditContentViewModel(viewModel.ApplicationInstanceData, viewModel.Person);
            }

            viewModel.PersonEditContentView = new PersonEditContentView();
            viewModel.PersonEditContentView.BindingContext = viewModel.PersonEditContentViewModel;

            Scrollview.Content = viewModel.PersonEditContentView;
            viewModel.PersonEditContentView.OnAppearing(!viewModel.IsCreate);
        }
    }
}
