using MDPMS.Shared.ViewModels.ContentPageModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MDPMS.Shared.Views.ContentPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HouseholdMembersSearchContentPage : ContentPage
	{
        public HouseholdMembersSearchContentPage ()
		{
			InitializeComponent ();
		}

        void Handle_Appearing(object sender, System.EventArgs e)
        {
            var viewModel = (HouseholdMembersSearchContentPageModel)this.BindingContext;
            viewModel.ExecuteAppearingCommand();
        }
    }
}