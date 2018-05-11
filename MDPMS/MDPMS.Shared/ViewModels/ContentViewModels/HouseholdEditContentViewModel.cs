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
            var now = DateTime.UtcNow;

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
            Household.LastUpdatedAt = now;

            // Custom Values edit
            for (var i = 0; i < CustomFields.Count(); i++)
            {
                // has value
                // is same
                // delete if no value but previously existed
                // add if has value but not existing
                // edit value if it changed

                var jsonValue = @"";
                var hasValue = false;

                switch (CustomFields[i].FieldType)
                {
                    case @"text":
                        var textValue = ((CustomFieldStringValueViewModel)CustomFieldControls[i].BindingContext).EntryValue;
                        if (textValue != null && !textValue.Equals(string.Empty))
                        {
                            hasValue = true;
                            jsonValue = Helpers.CustomValueConverter.ConvertCustomValueToJsonText(textValue);
                        }
                        break;
                    case @"textarea":
                        var textAreaValue = ((CustomFieldStringValueViewModel)CustomFieldControls[i].BindingContext).EntryValue;
                        if (textAreaValue != null && !textAreaValue.Equals(string.Empty))
                        {
                            hasValue = true;
                            jsonValue = Helpers.CustomValueConverter.ConvertCustomValueToJsonTextArea(textAreaValue);
                        }
                        break;
                    case @"check_box":
                        var checkBoxValues = ((CustomFieldSwitchArrayView)CustomFieldControls[i]).GetSelectedValuesAsList();
                        if (checkBoxValues.Any())
                        {
                            hasValue = true;
                            jsonValue = Helpers.CustomValueConverter.ConvertCustomValueToJsonCheckBox(checkBoxValues);
                        }
                        break;
                    case @"radio_button":
                        var radioButtonValue = ((CustomFieldPickerViewModel)CustomFieldControls[i].BindingContext).SelectedBindableOption;
                        if (radioButtonValue != null && !radioButtonValue.Equals(string.Empty))
                        {
                            hasValue = true;
                            jsonValue = Helpers.CustomValueConverter.ConvertCustomValueToJsonRadioButton(radioButtonValue);
                        }
                        break;
                    case @"select":
                        var selectValue = ((CustomFieldPickerViewModel)CustomFieldControls[i].BindingContext).SelectedBindableOption;
                        if (selectValue != null && !selectValue.Equals(string.Empty))
                        {
                            hasValue = true;
                            jsonValue = Helpers.CustomValueConverter.ConvertCustomValueToJsonSelect(selectValue);
                        }
                        break;
                    case @"number":
                        var numberValue = ((CustomFieldDoubleValueViewModel)CustomFieldControls[i].BindingContext).GetDoubleValue();
                        if (numberValue != null)
                        {
                            hasValue = true;
                            jsonValue = Helpers.CustomValueConverter.ConvertCustomValueToJsonNumber((double)numberValue);
                        }
                        break;
                    case @"date":
                        var dateValue = ((CustomFieldDateTimeValueViewModel)CustomFieldControls[i].BindingContext).DateValue;
                        if (dateValue != null && !dateValue.ToString().Equals(string.Empty))
                        {
                            hasValue = true;
                            jsonValue = Helpers.CustomValueConverter.ConvertCustomValueToJsonDate((DateTime)dateValue);
                        }
                        break;
                    case @"rank_list":
                        var rankedValues = ((CustomFieldRankListViewModel)CustomFieldControls[i].BindingContext).GetRankedValues();
                        if (!rankedValues.Equals(@""))
                        {
                            hasValue = true;
                            jsonValue = Helpers.CustomValueConverter.ConvertCustomValueToJsonRankList(((CustomFieldRankListViewModel)CustomFieldControls[i].BindingContext).Entries.ToList());
                        }
                        break;
                    default:
                        break;
                }

                var existingValueQuery = ApplicationInstanceData.Data.CustomHouseholdValues
                                                                .Where(a => a.Household.InternalId == Household.InternalId && a.CustomField.InternalId == CustomFields[i].InternalId);

                if (hasValue)
                {
                    // check to see if new needed
                    if (!existingValueQuery.Any())
                    {
                        // add a record
                        var newCustomValue = new CustomHouseholdValue
                        {
                            CreatedAt = now,
                            LastUpdatedAt = now,
                            SoftDeleted = false,
                            CustomField = CustomFields[i],
                            Value = jsonValue,
                            Household = Household,
                            InternalParentId = Household.InternalId
                        };
                        ApplicationInstanceData.Data.CustomHouseholdValues.Add(newCustomValue);
                    }
                    else
                    {
                        // if existing compare and update if necesary
                        if (existingValueQuery.First().Value != jsonValue)
                        {
                            existingValueQuery.First().Value = jsonValue;
                        }
                    }
                }
                else
                {
                    // delete value if it exists
                    if (existingValueQuery.Any()) ApplicationInstanceData.Data.CustomHouseholdValues.Remove(existingValueQuery.First());
                }
            }

            ApplicationInstanceData.Data.SaveChanges();
        }
    }
}
