using MDPMS.Shared.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MDPMS.Shared.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HouseholdIntakeView : ContentPage
	{
		public HouseholdIntakeView ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // get view model
            var viewModel = (HouseholdIntakeViewModel)BindingContext;

            CustomFieldContent.Children.Clear();

            // set dynamic content from the data in the vm
            // Custom fields
            foreach (var control in viewModel.CustomFieldControls)
            {
                CustomFieldContent.Children.Add(control);
            }
        }
	}
}