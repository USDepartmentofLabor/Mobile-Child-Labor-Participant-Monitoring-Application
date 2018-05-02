using MDPMS.Shared.ViewModels.ContentViewModels;
using Xamarin.Forms;

namespace MDPMS.Shared.Views.ContentViews
{
    public partial class PersonFollowUpsViewContentView : ContentView
    {
        public PersonFollowUpsViewContentView()
        {
            InitializeComponent();
        }

        public void OnAppearing()
        {
            // fix for list view height being too tall
            var viewModel = (PersonFollowUpsViewContentViewModel)BindingContext;
            var objectCount = (viewModel.Person.PeopleFollowUps == null) ? 0 : viewModel.Person.PeopleFollowUps.Count;
            var gridHeight = 30.0 + (45.0 * objectCount);
            ListViewRowDefinition.Height = new GridLength(gridHeight, GridUnitType.Absolute);
        }
    }
}
