using System;
using System.Collections.Generic;
using MDPMS.Database.Data.Models;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels
{
    public class HouseholdIntakeViewModel : ViewModelBase
    {
        Position GPSPosition;

        public Command CancelCommand { get; set; }
        public Command SubmitCommand { get; set; }
        public Command AddIncomeSourceCommand { get; set; }
        public Command AddHouseholdMemberCommand { get; set; }

        public List<IncomeSource> IncomeSources { get; set; } = new List<IncomeSource>();
        public List<Person> HouseholdMembers { get; set; } = new List<Person>();

        public string HouseholdName { get; set; } = @"";
        public DateTime IntakeDate { get; set; } = DateTime.Today;
        public string AddressLine1 { get; set; } = @"";
        public string AddressLine2 { get; set; } = @"";
        public string PostalCode { get; set; } = @"";
        public string DependentLocality { get; set; } = @"";
        public string Locality { get; set; } = @"";
        public string AdminvArea { get; set; } = @"";
        public string DependentAdminvArea { get; set; } = @"";
        public string Country { get; set; } = @"";
        public string AddressInfo { get; set; } = @"";

        public HouseholdIntakeViewModel(ApplicationInstanceData applicationInstanceData)
        {
            ApplicationInstanceData = applicationInstanceData;
            CancelCommand = new Command(ExecuteCancelCommand);
            SubmitCommand = new Command(ExecuteSubmitCommand);
            AddIncomeSourceCommand = new Command(ExecuteAddIncomeSourceCommand);
            AddHouseholdMemberCommand = new Command(ExecuteAddHouseholdMemberCommand);
        }

        private void ExecuteCancelCommand()
        {
            Exit();
        }

        private async void ExecuteSubmitCommand()
        {
            if (!NewHouseholdValidation()) return;
            var position = await CrossGeolocator.Current.GetPositionAsync(new TimeSpan(0, 0, 0, 0, 1000));
            GPSPosition = position;
            ExecutePostSubmitCommand();
        }

        private void ExecutePostSubmitCommand()
        {            
            var newHousehold = new Household
            {
                IntakeDate = IntakeDate,
                HouseholdName = HouseholdName,
                AddressLine1 = AddressLine1,
                AddressLine2 = AddressLine2,
                PostalCode = PostalCode,
                DependentLocality = DependentLocality,
                Locality = Locality,
                AdminvArea = AdminvArea,
                DependentAdminvArea = DependentAdminvArea,
                Country = Country,
                AddressInfo = AddressInfo,
                GpsLatitude = GPSPosition.Latitude,
                GpsLongitude = GPSPosition.Longitude,
                GpsPositionAccuracy = GPSPosition.Accuracy,
                GpsAltitude = GPSPosition.Altitude,
                GpsAltitudeAccuracy = GPSPosition.AltitudeAccuracy,
                GpsHeading = GPSPosition.Heading,
                GpsSpeed = GPSPosition.Speed,
                GpsPositionTime = DateTime.Now,
                IncomeSources = new List<IncomeSource>()
            };

            ApplicationInstanceData.Data.Households.Add(newHousehold);
            ApplicationInstanceData.Data.SaveChanges();

            foreach (var incomeSource in IncomeSources)
            {
                newHousehold.AddIncomeSource(incomeSource);
            }

            foreach (var person in HouseholdMembers)
            {
                newHousehold.AddMember(person);
            }

            ApplicationInstanceData.Data.SaveChanges();
            Exit();
        }

        private void Exit()
        {
            ApplicationInstanceData.GoToView(new HouseholdsView { BindingContext = new HouseholdsViewModel(ApplicationInstanceData) });
        }

        private void ExecuteAddIncomeSourceCommand()
        {
            // Modal navigate to add income source and retain household before submit
            ApplicationInstanceData.NavigationPage.PushAsync(new IncomeSourceAddView
            {
                BindingContext = new IncomeSourceAddViewModel(ApplicationInstanceData)
            });            
        }

        private void ExecuteAddHouseholdMemberCommand()
        {
            // Modal navigate to add household member and retain household before submit
            ApplicationInstanceData.NavigationPage.PushAsync(new HouseholdMemberIntakeView
            {
                BindingContext = new HouseholdMemberIntakeViewModel(ApplicationInstanceData)
            });
        }

        private bool NewHouseholdValidation()
        {
            // TODO: More "cheatable" char checks or find std regex
            var validateableName = HouseholdName.Replace(" ", "");
            if (validateableName.Equals(string.Empty))
            {
                ApplicationInstanceData.App.MainPage.DisplayAlert(
                    ApplicationInstanceData.SelectedLocalization.Translations[@"Error"],
                    ApplicationInstanceData.SelectedLocalization.Translations[@"ErrorNameCanNotBeBlank"],
                    ApplicationInstanceData.SelectedLocalization.Translations[@"OK"]);
                return false;
            }            
            return true;
        }        
    }
}
