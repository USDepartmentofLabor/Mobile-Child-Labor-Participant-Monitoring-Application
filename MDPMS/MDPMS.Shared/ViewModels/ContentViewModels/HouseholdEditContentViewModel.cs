using System;
using System.Collections.Generic;
using System.Linq;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views;
using Xamarin.Forms;

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

        // Custom fields
        public List<CustomField> CustomFields { get; set; }
        public List<ContentView> CustomFieldControls { get; set; }

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

            // Custom Fields
            var customFieldInit = Helpers.CustomFieldInit.InitCustomFields(@"Household", applicationInstanceData);
            CustomFields = customFieldInit.Item1;
            CustomFieldControls = customFieldInit.Item2;
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
            // existing household
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
            Household.LastUpdatedAt = DateTime.UtcNow;

            // save/update custom values
            var existingHouseholdValues = ApplicationInstanceData.Data.CustomHouseholdValues.Where(a => a.Household.InternalId == Household.InternalId).ToList();
            var saveTheseNewValues = new List<Tuple<CustomField, string>>();
            var updateTheseValues = new List<Tuple<CustomHouseholdValue, string>>();
            for (var i = 0; i < CustomFields.Count(); i++)
            {
                var queryExisting = existingHouseholdValues.Where(a => a.CustomField.InternalId == CustomFields[i].InternalId);
                var queryCount = queryExisting.Count();
                // TODO: error log if query count is > 1, should not be more than 1 value per custom field per object
                switch (CustomFields[i].FieldType)
                {
                    case "text":
                        var textValue = ((CustomFieldStringValueViewModel)CustomFieldControls[i].BindingContext).EntryValue;
                        if (textValue != null && !textValue.Equals(string.Empty))
                        {
                            var valueConverted = Helpers.CustomValueConverter.ConvertCustomValueToJsonText(textValue);
                            if (queryCount == 0) saveTheseNewValues.Add(new Tuple<CustomField, string>(CustomFields[i], valueConverted));
                            if (queryCount == 1) updateTheseValues.Add(new Tuple<CustomHouseholdValue, string>(queryExisting.First(), valueConverted));
                        }
                        break;
                    case "textarea":
                        var textAreaValue = ((CustomFieldStringValueViewModel)CustomFieldControls[i].BindingContext).GetEntryValueWithCrlf();
                        if (textAreaValue != null && !textAreaValue.Equals(string.Empty))
                        {
                            var valueConverted = Helpers.CustomValueConverter.ConvertCustomValueToJsonTextArea(textAreaValue);
                            if (queryCount == 0) saveTheseNewValues.Add(new Tuple<CustomField, string>(CustomFields[i], valueConverted));
                            if (queryCount == 1) updateTheseValues.Add(new Tuple<CustomHouseholdValue, string>(queryExisting.First(), valueConverted));
                        }
                        break;
                    case "check_box":
                        var checkBoxValues = ((CustomFieldSwitchArrayView)CustomFieldControls[i]).GetSelectedValuesAsList();
                        if (checkBoxValues.Any())
                        {
                            var valueConverted = Helpers.CustomValueConverter.ConvertCustomValueToJsonCheckBox(checkBoxValues);
                            if (queryCount == 0) saveTheseNewValues.Add(new Tuple<CustomField, string>(CustomFields[i], valueConverted));
                            if (queryCount == 1) updateTheseValues.Add(new Tuple<CustomHouseholdValue, string>(queryExisting.First(), valueConverted));
                        }
                        break;
                    case "radio_button":
                        var radioButtonValue = ((CustomFieldPickerViewModel)CustomFieldControls[i].BindingContext).SelectedBindableOption;
                        if (radioButtonValue != null && !radioButtonValue.Equals(string.Empty))
                        {
                            var valueConverted = Helpers.CustomValueConverter.ConvertCustomValueToJsonRadioButton(radioButtonValue);
                            if (queryCount == 0) saveTheseNewValues.Add(new Tuple<CustomField, string>(CustomFields[i], valueConverted));
                            if (queryCount == 1) updateTheseValues.Add(new Tuple<CustomHouseholdValue, string>(queryExisting.First(), valueConverted));
                        }
                        break;
                    case "select":
                        var selectValue = ((CustomFieldPickerViewModel)CustomFieldControls[i].BindingContext).SelectedBindableOption;
                        if (selectValue != null && !selectValue.Equals(string.Empty))
                        {
                            var valueConverted = Helpers.CustomValueConverter.ConvertCustomValueToJsonSelect(selectValue);
                            if (queryCount == 0) saveTheseNewValues.Add(new Tuple<CustomField, string>(CustomFields[i], valueConverted));
                            if (queryCount == 1) updateTheseValues.Add(new Tuple<CustomHouseholdValue, string>(queryExisting.First(), valueConverted));
                        }
                        break;
                    case "number":
                        var numberValue = ((CustomFieldDoubleValueViewModel)CustomFieldControls[i].BindingContext).GetDoubleValue();
                        if (numberValue != null)
                        {
                            var valueConverted = Helpers.CustomValueConverter.ConvertCustomValueToJsonNumber((double)numberValue);
                            if (queryCount == 0) saveTheseNewValues.Add(new Tuple<CustomField, string>(CustomFields[i], valueConverted));
                            if (queryCount == 1) updateTheseValues.Add(new Tuple<CustomHouseholdValue, string>(queryExisting.First(), valueConverted));
                        }
                        break;
                    case "date":
                        var dateValue = ((CustomFieldDateTimeValueViewModel)CustomFieldControls[i].BindingContext).DateValue;
                        if (dateValue != null && !dateValue.ToString().Equals(string.Empty))
                        {
                            var valueConverted = Helpers.CustomValueConverter.ConvertCustomValueToJsonDate((DateTime)dateValue);
                            if (queryCount == 0) saveTheseNewValues.Add(new Tuple<CustomField, string>(CustomFields[i], valueConverted));
                            if (queryCount == 1) updateTheseValues.Add(new Tuple<CustomHouseholdValue, string>(queryExisting.First(), valueConverted));
                        }
                        break;
                    case "rank_list":
                        var rankedValues = ((CustomFieldRankListViewModel)CustomFieldControls[i].BindingContext).GetRankedValues();
                        if (!rankedValues.Equals(@""))
                        {
                            var valueConverted = Helpers.CustomValueConverter.ConvertCustomValueToJsonRankList(((CustomFieldRankListViewModel)CustomFieldControls[i].BindingContext).Entries.ToList());
                            if (queryCount == 0) saveTheseNewValues.Add(new Tuple<CustomField, string>(CustomFields[i], valueConverted));
                            if (queryCount == 1) updateTheseValues.Add(new Tuple<CustomHouseholdValue, string>(queryExisting.First(), valueConverted));
                        }
                        break;
                }
            }

            // update values
            foreach (var valueToUpdate in updateTheseValues)
            {
                valueToUpdate.Item1.LastUpdatedAt = DateTime.UtcNow;
                valueToUpdate.Item1.Value = valueToUpdate.Item2;
            }

            // save new values
            foreach (var newValue in saveTheseNewValues)
            {
                ApplicationInstanceData.Data.CustomHouseholdValues.Add(new CustomHouseholdValue
                {
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow,
                    SoftDeleted = false,
                    CustomField = newValue.Item1,
                    Value = newValue.Item2,
                    Household = Household
                });
            }

            ApplicationInstanceData.Data.SaveChanges();
        }
    }
}
