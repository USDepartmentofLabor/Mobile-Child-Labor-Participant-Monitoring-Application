using System;
using System.Collections.Generic;
using MDPMS.Database.Data.Models;
using Plugin.Geolocator.Abstractions;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;

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

        // Custom fields
        public List<CustomField> CustomFields { get; set; }
        public List<ContentView> CustomFieldControls { get; set; }

        public HouseholdIntakeViewModel(ApplicationInstanceData applicationInstanceData)
        {
            ApplicationInstanceData = applicationInstanceData;

            // Custom Fields
            var customFieldInit = Helpers.CustomFieldInit.InitCustomFields(@"Household", applicationInstanceData);
            CustomFields = customFieldInit.Item1;
            CustomFieldControls = customFieldInit.Item2;

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

            IsBusy = true;
            GPSPosition = await Helper.Gps.GpsHelper.GetGpsPosition();
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
                GpsLatitude = GPSPosition?.Latitude,
                GpsLongitude = GPSPosition?.Longitude,
                GpsPositionAccuracy = GPSPosition?.Accuracy,
                GpsAltitude = GPSPosition?.Altitude,
                GpsAltitudeAccuracy = GPSPosition?.AltitudeAccuracy,
                GpsHeading = GPSPosition?.Heading,
                GpsSpeed = GPSPosition?.Speed,
                GpsPositionTime = DateTime.Now,
                IncomeSources = new List<IncomeSource>()
            };

            // get custom field values
            for (var i = 0; i < CustomFields.Count; i++)
            {
                var newCustomValue = new CustomHouseholdValue
                {
                    CreatedAt = DateTime.Now,
                    LastUpdatedAt = DateTime.Now,
                    SoftDeleted = false,
                    CustomField = CustomFields[i],
                    Value = "",
                    Household = newHousehold
                };

                switch (CustomFields[i].FieldType)
                {
                    case @"text":
                        var textValue = ((CustomFieldStringValueViewModel)CustomFieldControls[i].BindingContext).EntryValue;
                        if (textValue != null && !textValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = Helpers.CustomValueConverter.ConvertCustomValueToJsonText(textValue);
                            ApplicationInstanceData.Data.CustomHouseholdValues.Add(newCustomValue);
                        }
                        break;
                    case @"textarea":
                        var textAreaValue = ((CustomFieldStringValueViewModel)CustomFieldControls[i].BindingContext).GetEntryValueWithCrlf();
                        if (textAreaValue != null && !textAreaValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = Helpers.CustomValueConverter.ConvertCustomValueToJsonTextArea(textAreaValue);
                            ApplicationInstanceData.Data.CustomHouseholdValues.Add(newCustomValue);
                        }
                        break;
                    case @"check_box":
                        var checkBoxValues = ((CustomFieldSwitchArrayView)CustomFieldControls[i]).GetSelectedValuesAsList();
                        if (checkBoxValues.Any())
                        {
                            newCustomValue.Value = Helpers.CustomValueConverter.ConvertCustomValueToJsonCheckBox(checkBoxValues);
                            ApplicationInstanceData.Data.CustomHouseholdValues.Add(newCustomValue);
                        }
                        break;
                    case @"radio_button":
                        var radioButtonValue = ((CustomFieldPickerViewModel)CustomFieldControls[i].BindingContext).SelectedBindableOption;
                        if (radioButtonValue != null && !radioButtonValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = Helpers.CustomValueConverter.ConvertCustomValueToJsonRadioButton(radioButtonValue);
                            ApplicationInstanceData.Data.CustomHouseholdValues.Add(newCustomValue);
                        }
                        break;
                    case @"select":
                        var selectValue = ((CustomFieldPickerViewModel)CustomFieldControls[i].BindingContext).SelectedBindableOption;
                        if (selectValue != null && !selectValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = Helpers.CustomValueConverter.ConvertCustomValueToJsonSelect(selectValue);
                            ApplicationInstanceData.Data.CustomHouseholdValues.Add(newCustomValue);
                        }
                        break;
                    case @"number":
                        var numberValue = ((CustomFieldDoubleValueViewModel)CustomFieldControls[i].BindingContext).GetDoubleValue();
                        if (numberValue != null)
                        {
                            newCustomValue.Value = Helpers.CustomValueConverter.ConvertCustomValueToJsonNumber((double)numberValue);
                            ApplicationInstanceData.Data.CustomHouseholdValues.Add(newCustomValue);
                        }
                        break;
                    case @"date":
                        var dateValue = ((CustomFieldDateTimeValueViewModel)CustomFieldControls[i].BindingContext).DateValue;
                        if (dateValue != null && !dateValue.ToString().Equals(string.Empty))
                        {
                            newCustomValue.Value = Helpers.CustomValueConverter.ConvertCustomValueToJsonDate((DateTime)dateValue);
                            ApplicationInstanceData.Data.CustomHouseholdValues.Add(newCustomValue);
                        }
                        break;
                    case @"rank_list":
                        var rankedValues = ((CustomFieldRankListViewModel)CustomFieldControls[i].BindingContext).GetRankedValues();
                        if (!rankedValues.Equals(@""))
                        {
                            newCustomValue.Value = Helpers.CustomValueConverter.ConvertCustomValueToJsonRankList(((CustomFieldRankListViewModel)CustomFieldControls[i].BindingContext).Entries.ToList());
                            ApplicationInstanceData.Data.CustomHouseholdValues.Add(newCustomValue);
                        }
                        break;
                    default:
                        break;
                }
            }

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

            IsBusy = false;
            Exit();
        }

        private void Exit()
        {
            ApplicationInstanceData.GoToView(new HouseholdsSearchView { BindingContext = new HouseholdsSearchViewModel(ApplicationInstanceData) });
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
                    ApplicationInstanceData.SelectedLocalization.Translations[@"ErrorHouseholdNameCanNotBeBlank"],
                    ApplicationInstanceData.SelectedLocalization.Translations[@"OK"]);
                return false;
            }            
            return true;
        }        
    }
}
