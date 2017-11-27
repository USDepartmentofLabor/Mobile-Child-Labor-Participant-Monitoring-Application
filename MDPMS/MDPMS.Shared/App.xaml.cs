using MDPMS.Shared.Models;
using MDPMS.Shared.Views;
using MDPMS.Shared.ViewModels;
using Xamarin.Forms;

namespace MDPMS.Shared
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

            var applicationInstanceData = new ApplicationInstanceData();

            MainPage = new LandingView()
            {
                BindingContext = new LandingViewModel(applicationInstanceData),
                Title = @"mDPMS"
            };            
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
