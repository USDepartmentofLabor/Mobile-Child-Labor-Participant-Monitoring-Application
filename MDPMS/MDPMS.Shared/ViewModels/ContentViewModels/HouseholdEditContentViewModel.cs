using System;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;

namespace MDPMS.Shared.ViewModels.ContentViewModels
{
    public class HouseholdEditContentViewModel : ViewModelBase
    {
        public Household Household { get; set; }

        // temp/locally cached properties
        public DateTime IntakeDate { get; set; }
        public string HouseholdName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostalCode { get; set; }
        public string DependentLocality { get; set; }
        public string Locality { get; set; }
        public string AdminvArea { get; set; }
        public string DependentAdminvArea { get; set; }
        public string Country { get; set; }
        public string AddressInfo { get; set; }

        public HouseholdEditContentViewModel(ApplicationInstanceData applicationInstanceData, Household household)
        {
            ApplicationInstanceData = applicationInstanceData;
            Household = household;

            // set temp/locally cached properties to support save/cancel
            IntakeDate = Household.IntakeDate;
            HouseholdName = Household.HouseholdName;
            AddressLine1 = Household.AddressLine1;
            AddressLine2 = Household.AddressLine2;
            PostalCode = Household.PostalCode;
            DependentLocality = Household.DependentLocality;
            Locality = Household.Locality;
            AdminvArea = Household.AdminvArea;
            DependentAdminvArea = Household.DependentAdminvArea;
            Country = Household.Country;
            AddressInfo = Household.AddressInfo;
        }

        public bool ValidateHousehold()
        {
            // TODO: More "cheatable" char checks or find std regex
            var validateableName = HouseholdName.Replace(" ", "");
            if (validateableName.Equals(string.Empty))
            {
                ApplicationInstanceData.App.MainPage.DisplayAlert(
                    ApplicationInstanceData.SelectedLocalization.Translations[@"Error"],
                    ApplicationInstanceData.SelectedLocalization.Translations[@"ErrorHouseholdNameCanNotBeBlank"],
                    ApplicationInstanceData.SelectedLocalization.Translations[@"OK"]);
                return false;
            }
            return true;
        }

        public void SaveHousehold()
        {
            Household.IntakeDate = IntakeDate;
            Household.HouseholdName = HouseholdName;
            Household.AddressLine1 = AddressLine1;
            Household.AddressLine2 = AddressLine2;
            Household.PostalCode = PostalCode;
            Household.DependentLocality = DependentLocality;
            Household.Locality = Locality;
            Household.AdminvArea = AdminvArea;
            Household.DependentAdminvArea = DependentAdminvArea;
            Household.Country = Country;
            Household.AddressInfo = AddressInfo;
        }
    }
}
