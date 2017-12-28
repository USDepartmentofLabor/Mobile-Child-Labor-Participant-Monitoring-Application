using System;
using System.Globalization;
using System.Linq;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        public Command Test1Command { get; set; }
        public Command Test2Command { get; set; }

        public AboutViewModel(ApplicationInstanceData applicationInstanceData)
        {
            ApplicationInstanceData = applicationInstanceData;
            Test1Command = new Command(ExecuteTest1Command);
            Test2Command = new Command(ExecuteTest2Command);
        }

        private void ExecuteTest1Command()
        {
            // add new house            
            var newHouse = new Household()
            {
                ExternalId = null,
                CreatedAt = DateTime.Now,
                LastUpdatedAt = DateTime.Now,
                SoftDeleted = false,
                HouseholdName = @"I am from the mobile app as a new house",
                IntakeDate = DateTime.Today.AddDays(2),
                AddressLine1 = @"123 Main Street",
                AddressLine2 = @"Apt 42",
                PostalCode = @"21044",
                DependentLocality = @"Townhouses",
                Locality = @"Columbia",
                AdminvArea = @"MD",
                DependentAdminvArea = @"Howard",
                Country = @"USA",
                AddressInfo = @"Behind another house"
            };
            ApplicationInstanceData.Data.Households.Add(newHouse);
            ApplicationInstanceData.Data.SaveChanges();
        }

        private void ExecuteTest2Command()
        {
            // change a house
            var tempRecord = ApplicationInstanceData.Data.Households.First();
            tempRecord.HouseholdName = DateTime.Now.ToUniversalTime().ToString(CultureInfo.InvariantCulture);
            tempRecord.LastUpdatedAt = DateTime.Now.ToUniversalTime();
            ApplicationInstanceData.Data.SaveChanges();
        }
    }
}
