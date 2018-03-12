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
            CustomFields = new List<CustomField>();
            CustomFieldControls = new List<ContentView>();
            CustomFields = ApplicationInstanceData.Data.CustomFields.Where(a => a.ModelType.Equals(@"Household")).OrderBy(b => b.SortOrder).ToList();
            foreach (var customField in CustomFields)
            {
                switch (customField.FieldType)
                {
                    case @"text":
                        CustomFieldControls.Add(new CustomFieldEntryView
                        {
                            BindingContext = new CustomFieldStringValueViewModel(customField.Name, customField.HelpText),
                            Margin = new Thickness(15, 5)
                        });
                        break;
                    case @"textarea":
                        CustomFieldControls.Add(new CustomFieldEditorView
                        {
                            BindingContext = new CustomFieldStringValueViewModel(customField.Name, customField.HelpText),
                            Margin = new Thickness(15, 5)
                        });
                        break;
                    case @"check_box":
                        var checkBoxViewModel = new CustomFieldSwitchArrayViewModel(customField.Name, customField.HelpText);
                        checkBoxViewModel.Content = new List<Tuple<bool, string>>();
                        var checkBoxValues = customField.GetOptions();
                        foreach (var option in checkBoxValues)
                        {
                            checkBoxViewModel.Content.Add(new Tuple<bool, string>(false, option));
                        }
                        var checkBoxView = new CustomFieldSwitchArrayView
                        {
                            BindingContext = checkBoxViewModel,
                            Margin = new Thickness(15, 5)
                        };
                        CustomFieldControls.Add(checkBoxView);
                        checkBoxView.OnAppearing();
                        break;
                    case @"radio_button":
                        var bindableOptions = new ObservableCollection<string>();
                        bindableOptions.Add(@"");
                        var pickerValues = customField.GetOptions();
                        foreach (var option in pickerValues)
                        {
                            bindableOptions.Add(option);
                        }
                        CustomFieldControls.Add(new CustomFieldPickerView
                        {
                            BindingContext = new CustomFieldPickerViewModel(customField.Name, customField.HelpText)
                            {
                                BindableOptions = bindableOptions,
                                SelectedBindableOption = bindableOptions.First()
                            },
                            Margin = new Thickness(15, 5)
                        });
                        break;
                    case @"select":
                        var bindableSelectOptions = new ObservableCollection<string>();
                        bindableSelectOptions.Add(@"");
                        var selectValues = customField.GetOptions();
                        foreach (var option in selectValues)
                        {
                            bindableSelectOptions.Add(option);
                        }
                        CustomFieldControls.Add(new CustomFieldPickerView
                        {
                            BindingContext = new CustomFieldPickerViewModel(customField.Name, customField.HelpText)
                            {
                                BindableOptions = bindableSelectOptions,
                                SelectedBindableOption = bindableSelectOptions.First()
                            },
                            Margin = new Thickness(15, 5)
                        });
                        break;
                    case @"number":
                        CustomFieldControls.Add(new CustomFieldNumericView
                        {
                            BindingContext = new CustomFieldDoubleValueViewModel(customField.Name, customField.HelpText),
                            Margin = new Thickness(15, 5)
                        });
                        break;
                    case @"date":
                        CustomFieldControls.Add(new CustomFieldDateTimeView
                        {
                            BindingContext = new CustomFieldDateTimeValueViewModel(customField.Name, customField.HelpText),
                            Margin = new Thickness(15, 5)
                        });
                        break;
                    case @"rank_list":
                        CustomFieldControls.Add(new CustomFieldRankListView
                        {
                            Margin = new Thickness(15, 5)
                        });
                        break;
                    default:
                        // TODO: error log but for now just fail silently
                        break;
                }
            }

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
                            newCustomValue.Value = textValue;
                            ApplicationInstanceData.Data.CustomHouseholdValues.Add(newCustomValue);
                        }
                        break;
                    case @"textarea":
                        var textAreaValue = ((CustomFieldStringValueViewModel)CustomFieldControls[i].BindingContext).EntryValue;
                        if (textAreaValue != null && !textAreaValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = textAreaValue;
                            ApplicationInstanceData.Data.CustomHouseholdValues.Add(newCustomValue);
                        }
                        break;
                    case @"check_box":
                        var checkBoxValue = @"";
                        var checkBoxValues = ((CustomFieldSwitchArrayView)CustomFieldControls[i]).GetValues();
                        for (var j = 0; j < checkBoxValues.Count; j++)
                        {
                            if (checkBoxValues[j].Item2)
                            {
                                checkBoxValue += checkBoxValues[j].Item1;
                                if (j != checkBoxValues.Count - 1) checkBoxValue += "\r\n";
                            }
                        }
                        if (!checkBoxValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = checkBoxValue;
                            ApplicationInstanceData.Data.CustomHouseholdValues.Add(newCustomValue);
                        }
                        break;
                    case @"radio_button":
                        var radioButtonValue = ((CustomFieldPickerViewModel)CustomFieldControls[i].BindingContext).SelectedBindableOption;
                        if (radioButtonValue != null && !radioButtonValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = radioButtonValue;
                            ApplicationInstanceData.Data.CustomHouseholdValues.Add(newCustomValue);
                        }
                        break;
                    case @"select":
                        var selectValue = ((CustomFieldPickerViewModel)CustomFieldControls[i].BindingContext).SelectedBindableOption;
                        if (selectValue != null && !selectValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = selectValue;
                            ApplicationInstanceData.Data.CustomHouseholdValues.Add(newCustomValue);
                        }
                        break;
                    case @"number":
                        // TODO: saves zero values on no data entered, add value was changed bool to see if value is needed?
                        var numberValue = ((CustomFieldDoubleValueViewModel)CustomFieldControls[i].BindingContext).EntryValue;
                        var numberValueString = numberValue.ToString();
                        if (numberValueString != null && !numberValueString.Equals(string.Empty))
                        {
                            newCustomValue.Value = numberValueString;
                            ApplicationInstanceData.Data.CustomHouseholdValues.Add(newCustomValue);
                        }
                        break;
                    case @"date":
                        var dateValue = ((CustomFieldDateTimeValueViewModel)CustomFieldControls[i].BindingContext).EntryValue;
                        var dateValueString = dateValue.ToString();
                        if (dateValueString != null && !dateValueString.Equals(string.Empty))
                        {
                            newCustomValue.Value = dateValueString;
                            ApplicationInstanceData.Data.CustomHouseholdValues.Add(newCustomValue);
                        }
                        break;
                    case @"rank_list":
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
                    ApplicationInstanceData.SelectedLocalization.Translations[@"ErrorHouseholdNameCanNotBeBlank"],
                    ApplicationInstanceData.SelectedLocalization.Translations[@"OK"]);
                return false;
            }            
            return true;
        }        
    }
}
