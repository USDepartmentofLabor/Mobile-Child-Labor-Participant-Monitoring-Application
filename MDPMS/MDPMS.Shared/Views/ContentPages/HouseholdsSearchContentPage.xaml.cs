using MDPMS.Shared.ViewModels.ContentPageModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MDPMS.Shared.Views.ContentPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HouseholdsSearchContentPage : ContentPage
	{
        public HouseholdsSearchContentPage ()
		{
			InitializeComponent ();
		}

        void Handle_Appearing(object sender, System.EventArgs e)
        {
            var viewModel = (HouseholdsSearchContentPageModel)this.BindingContext;
            viewModel.ExecuteAppearingCommand();
        }
	}
}