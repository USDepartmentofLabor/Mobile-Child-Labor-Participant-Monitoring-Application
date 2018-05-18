using MDPMS.Shared.ViewModels.ContentPageModels;
using MDPMS.Shared.ViewModels.ContentViewModels;
using MDPMS.Shared.Views.ContentViews;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MDPMS.Shared.Views.ContentPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonFollowUpEditContentPage : ContentPage
    {
        public PersonFollowUpEditContentPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = (PersonFollowUpEditContentPageModel)BindingContext;

            if (viewModel.IsCreate)
            {
                viewModel.PersonFollowUpEditContentViewModel = new PersonFollowUpEditContentViewModel(viewModel.ApplicationInstanceData, viewModel.ParentPerson);
            }
            else
            {
                viewModel.PersonFollowUpEditContentViewModel = new PersonFollowUpEditContentViewModel(viewModel.ApplicationInstanceData, viewModel.PersonFollowUp);
            }

            viewModel.PersonFollowUpEditContentView = new PersonFollowUpEditContentView();
            viewModel.PersonFollowUpEditContentView.BindingContext = viewModel.PersonFollowUpEditContentViewModel;

            Scrollview.Content = viewModel.PersonFollowUpEditContentView;
            viewModel.PersonFollowUpEditContentView.OnAppearing(!viewModel.IsCreate);
        }
    }
}
