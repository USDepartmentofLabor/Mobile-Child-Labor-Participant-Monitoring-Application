using MDPMS.EfDatabase.Database;
using MDPMS.Shared.Models;
using MDPMS.Shared.Views;
using MDPMS.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;

namespace MDPMS.Shared
{
	public partial class App : Application
	{
		public App (string databasePath)
		{
			InitializeComponent();

            // init db
            var db = new MDPMSDatabaseContext(databasePath);

            db.Database.EnsureCreated();            
            //db.Database.Migrate();
            DatabaseSeed.SeedDatabase(db);
            
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
