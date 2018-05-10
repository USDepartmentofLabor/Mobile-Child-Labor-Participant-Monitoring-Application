using MDPMS.Shared.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MDPMS.Shared.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HouseholdsSearchView : ContentPage
	{
        public HouseholdsSearchView ()
		{
			InitializeComponent ();
		}

        void Handle_Appearing(object sender, System.EventArgs e)
        {
            var viewModel = (HouseholdsSearchViewModel)this.BindingContext;
            viewModel.ExecuteAppearingCommand();
        }
	}
}