using System;
using System.Collections.ObjectModel;
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

		    var databasePath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), @"DATA.db");
		    var localizationDatabasePath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), @"LOCAL.db");

            // Application Instance Data
            var applicationInstanceData = new ApplicationInstanceData
            {
                PlatformDataPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                ApplicationInstanceDataFileName = @"AppData.json",
                DatabasePath = databasePath,
                LocalizationDatabasePath = localizationDatabasePath,
                SerializedApplicationInstanceData = new SerializedApplicationInstanceData(),
                AvailableLocalizations = new ObservableCollection<Localization>(),
                App = this
            };

		    var applicationInstanceDataFilePath = System.IO.Path.Combine(applicationInstanceData.PlatformDataPath, applicationInstanceData.ApplicationInstanceDataFileName);
		    if (!System.IO.File.Exists(applicationInstanceDataFilePath))
		    {
		        Helper.Json.JsonFileHelper.SaveDataToJsonFile(applicationInstanceData.SerializedApplicationInstanceData, applicationInstanceDataFilePath);
		    }
            applicationInstanceData.SerializedApplicationInstanceData = Helper.Json.JsonFileHelper.GetDataFromJsonFile<SerializedApplicationInstanceData>(applicationInstanceDataFilePath);            

            // Init db
		    var db = new Database.Data.Database.MDPMSDatabaseContext(applicationInstanceData.DatabasePath);
            db.Database.EnsureCreated();            
            //db.Database.Migrate();
            db.LoadRelatedData();
            Database.Data.Database.DatabaseSeed.SeedDatabase(db);
		    applicationInstanceData.Data = db;

            // Init Language Database
            var localizationDb = new Database.Localization.Database.LocalizationDatabaseContext(applicationInstanceData.LocalizationDatabasePath);
		    localizationDb.Database.EnsureCreated();
            Database.Localization.Database.DatabaseSeed.SeedDatabase(localizationDb);            
		    applicationInstanceData.Localization = localizationDb;

            // Load Localizations
            applicationInstanceData.GetAvailableLocalizations();
		    // Load en as default for now
            applicationInstanceData.SetLocalization(@"en");
                       
            // Load view
            applicationInstanceData.NavigateToLandingView();                        
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
