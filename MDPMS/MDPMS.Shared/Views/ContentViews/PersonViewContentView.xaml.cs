using MDPMS.Shared.ViewModels.ContentViewModels;
using Xamarin.Forms;

namespace MDPMS.Shared.Views.ContentViews
{
    public partial class PersonViewContentView : ContentView
    {
        public PersonViewContentView()
        {
            InitializeComponent();
        }

        public void OnAppearing()
        {
            var viewModel = (PersonViewContentViewModel)BindingContext;

            // Person Follow Ups
            PersonFollowUpsContent.Children.Clear();

            var personFollowUpsViewContentView = new PersonFollowUpsViewContentView();
            var personFollowUpsViewContentViewModel = new PersonFollowUpsViewContentViewModel(viewModel.ApplicationInstanceData, viewModel.Person);
            personFollowUpsViewContentView.BindingContext = personFollowUpsViewContentViewModel;
            PersonFollowUpsContent.Children.Add(personFollowUpsViewContentView);
            personFollowUpsViewContentView.OnAppearing();
        }
    }
}
