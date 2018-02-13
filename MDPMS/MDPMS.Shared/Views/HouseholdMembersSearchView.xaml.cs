using MDPMS.Shared.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MDPMS.Shared.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HouseholdMembersSearchView : ContentPage
	{
		public HouseholdMembersSearchView ()
		{
			InitializeComponent ();
		}

        void Handle_Appearing(object sender, System.EventArgs e)
        {
            var viewModel = (HouseholdMembersSearchViewModel)this.BindingContext;
            viewModel.ExecuteAppearingCommand();
        }
    }
}