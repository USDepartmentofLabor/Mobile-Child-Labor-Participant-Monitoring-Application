using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels
{
    public class HouseholdMemberFollowUpViewModel : ViewModelBase
    {
        Position GPSPosition;

        private readonly Person _person;

        // Person Read Only Labels and Values
        public string HouseholdMemberIdLabel { get; set; }
        public string HouseholdMemberIdValue { get; set; }
        
        public string FirstNameGivenNameLabel { get; set; }
        public string FirstNameGivenNameValue { get; set; }

        public string MiddleNameLabel { get; set; }
        public string MiddleNameValue { get; set; }

        public string LastNameFamilyNameLabel { get; set; }
        public string LastNameFamilyNameValue { get; set; }

        public string IntakeDateLabel { get; set; }
        public string IntakeDateValue { get; set; }

        public string GenderLabel { get; set; }
        public string GenderValue { get; set; }

        public string DateOfBirthLabel { get; set; }
        public string DateOfBirthValue { get; set; }

        public string IsTheBirthdayAnApproximateDateLabel { get; set; }
        public string IsTheBirthdayAnApproximateDateValue { get; set; }

        public string AgeLabel { get; set; }
        public string AgeValue { get; set; }
        
        // Commands
        public Command CancelCommand { get; set; }
        public Command SubmitCommand { get; set; }
        public Command NavigateToAddServiceCommand { get; set; }

        public DateTime FollowUpDate { get; set; } = DateTime.Today;
        
        public bool WorkActivityNoActivityQuestionAnswer { get; set; } = false;
        public int WorkActivityHoursEngaged { get; set; } = 0;
        public int HouseholdTasksHoursEngaged { get; set; } = 0;
        public bool EnrolledInSchoolCollege { get; set; } = false;

        // value, question
        public ObservableCollection<Tuple<bool, StatusCustomizationWorkActivity, GenericSwitchTextViewModel>> BindableWorkActivities { get; set; }
        public ObservableCollection<Tuple<bool, StatusCustomizationHazardousCondition, GenericSwitchTextViewModel>> BindableHazardousConditions { get; set; }
        public ObservableCollection<Tuple<bool, StatusCustomizationHouseholdTask, GenericSwitchTextViewModel>> BindableHouseholdTasks { get; set; }

        // Custom fields
        public List<CustomField> CustomFields { get; set; }
        public List<ContentView> CustomFieldControls { get; set; }

        public HouseholdMemberFollowUpViewModel(ApplicationInstanceData applicationInstanceData, Person person)
        {
            ApplicationInstanceData = applicationInstanceData;
            _person = new Person();
            _person = person;

            // Labels
            HouseholdMemberIdLabel = ApplicationInstanceData.SelectedLocalization.Translations[@"HouseholdMemberId"] + @":";
            FirstNameGivenNameLabel = ApplicationInstanceData.SelectedLocalization.Translations[@"FirstNameGivenName"] + @":";
            MiddleNameLabel = ApplicationInstanceData.SelectedLocalization.Translations[@"MiddleName"] + @":";
            LastNameFamilyNameLabel = ApplicationInstanceData.SelectedLocalization.Translations[@"LastNameFamilyName"] + @":";
            IntakeDateLabel = ApplicationInstanceData.SelectedLocalization.Translations[@"IntakeDate"] + @":";
            GenderLabel = ApplicationInstanceData.SelectedLocalization.Translations[@"Gender"] + @":";
            DateOfBirthLabel = ApplicationInstanceData.SelectedLocalization.Translations[@"DateOfBirth"] + @":";
            IsTheBirthdayAnApproximateDateLabel = ApplicationInstanceData.SelectedLocalization.Translations[@"IsTheBirthdayAnApproximateDate"] + @":";
            AgeLabel = ApplicationInstanceData.SelectedLocalization.Translations[@"Age"] + @":";
            
            // Set Person Read Only Attributes for Display
            HouseholdMemberIdValue = person.HasExternalId ? person.GetExternalId().ToString() : @"";
            FirstNameGivenNameValue = person.FirstName;
            MiddleNameValue = person.MiddleName;
            LastNameFamilyNameValue = person.LastName;
            IntakeDateValue = person.IntakeDate == null ? @"" : ((DateTime)person.IntakeDate).ToShortDateString();
            GenderValue = person.Gender == null ? @"" : person.Gender.GenderReadable;
            DateOfBirthValue = person.DateOfBirth == null ? @"" : ((DateTime)person.DateOfBirth).ToShortDateString();
            IsTheBirthdayAnApproximateDateValue = person.DateOfBirthIsApproximate.ToString();
            AgeValue = person.DateOfBirth == null ? @"" : (DateTime.UtcNow.Year - ((DateTime)person.DateOfBirth).Year).ToString();
            
            // Work Activities
            BindableWorkActivities = new ObservableCollection<Tuple<bool, StatusCustomizationWorkActivity, GenericSwitchTextViewModel>>();
            foreach (var workActivity in ApplicationInstanceData.Data.StatusCustomizationWorkActivities)
            {
                BindableWorkActivities.Add(new Tuple<bool, StatusCustomizationWorkActivity, GenericSwitchTextViewModel>(false, workActivity,
                    new GenericSwitchTextViewModel(workActivity.DisplayName)));
            }

            // Hazardous Conditions
            BindableHazardousConditions = new ObservableCollection<Tuple<bool, StatusCustomizationHazardousCondition, GenericSwitchTextViewModel>>();
            foreach (var hazardousCondition in ApplicationInstanceData.Data.StatusCustomizationHazardousConditions)
            {
                BindableHazardousConditions.Add(new Tuple<bool, StatusCustomizationHazardousCondition, GenericSwitchTextViewModel>(false, hazardousCondition,
                    new GenericSwitchTextViewModel(hazardousCondition.DisplayName)));
            }

            // Household Tasks
            BindableHouseholdTasks = new ObservableCollection<Tuple<bool, StatusCustomizationHouseholdTask, GenericSwitchTextViewModel>>();
            foreach (var householdTask in ApplicationInstanceData.Data.StatusCustomizationHouseholdTasks)
            {
                BindableHouseholdTasks.Add(new Tuple<bool, StatusCustomizationHouseholdTask, GenericSwitchTextViewModel>(false, householdTask,
                    new GenericSwitchTextViewModel(householdTask.DisplayName)));
            }

            // Custom Fields
            CustomFields = new List<CustomField>();
            CustomFieldControls = new List<ContentView>();
            CustomFields = ApplicationInstanceData.Data.CustomFields.Where(a => a.ModelType.Equals(@"FollowUp")).OrderBy(b => b.SortOrder).ToList();
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
                        var customFieldRankListViewModel = new CustomFieldRankListViewModel(customField.Name, customField.HelpText);
                        customFieldRankListViewModel.Entries = new ObservableCollection<Tuple<int, string>>();
                        var rankValues = customField.GetOptions();
                        for (var j = 0; j < rankValues.Count; j++)
                        {
                            customFieldRankListViewModel.Entries.Add(new Tuple<int, string>(j, rankValues[j]));
                        }
                        var rankListView = new CustomFieldRankListView
                        {
                            BindingContext = customFieldRankListViewModel,
                            Margin = new Thickness(15, 5)
                        };
                        customFieldRankListViewModel.View = rankListView;
                        CustomFieldControls.Add(rankListView);
                        rankListView.OnAppearing();
                        break;
                    default:
                        // TODO: error log but for now just fail silently
                        break;
                }
            }

            CancelCommand = new Command(ExecuteCancelCommand);
            SubmitCommand = new Command(ExecuteSubmitCommand);
            NavigateToAddServiceCommand = new Command(ExecuteNavigateToAddServiceCommand);
        }

        private void ExecuteCancelCommand()
        {
            Exit();
        }

        private async void ExecuteSubmitCommand()
        {
            IsBusy = true;
            GPSPosition = await Helper.Gps.GpsHelper.GetGpsPosition();
            ExecutePostSubmitCommand();
        }

        private void ExecutePostSubmitCommand()
        {
            var newPersonFollowUp = new PersonFollowUp
            {
                LastUpdatedAt = DateTime.Now,
                FollowUpDate = FollowUpDate,
                HaveJobReturningTo = WorkActivityNoActivityQuestionAnswer,
                HoursWorked = WorkActivityHoursEngaged,
                HouseWorkedOnHousework = HouseholdTasksHoursEngaged,
                EnrolledInSchool = EnrolledInSchoolCollege,
                GpsLatitude = GPSPosition?.Latitude,
                GpsLongitude = GPSPosition?.Longitude,
                GpsPositionAccuracy = GPSPosition?.Accuracy,
                GpsAltitude = GPSPosition?.Altitude,
                GpsAltitudeAccuracy = GPSPosition?.AltitudeAccuracy,
                GpsHeading = GPSPosition?.Heading,
                GpsSpeed = GPSPosition?.Speed,
                GpsPositionTime = DateTime.Now,
                PeopleFollowUpHazardousConditions = new List<PersonFollowUpHazardousCondition>(),
                PeopleFollowUpWorkActivities = new List<PersonFollowUpWorkActivity>(),
                PeopleFollowUpHouseholdTasks = new List<PersonFollowUpHouseholdTask>()
            };

            foreach (var bindableHazardousCondition in BindableHazardousConditions.Where(a => a.Item3.BoolValue))
            {
                newPersonFollowUp.PeopleFollowUpHazardousConditions.Add(new PersonFollowUpHazardousCondition
                {
                    PersonFollowUp = newPersonFollowUp,
                    HazardousCondition = bindableHazardousCondition.Item2
                });
            }

            foreach (var bindableWorkActivity in BindableWorkActivities.Where(a => a.Item3.BoolValue))
            {
                newPersonFollowUp.PeopleFollowUpWorkActivities.Add(new PersonFollowUpWorkActivity
                {
                    PersonFollowUp = newPersonFollowUp,
                    WorkActivity = bindableWorkActivity.Item2
                });
            }

            foreach (var bindableHouseholdTask in BindableHouseholdTasks.Where(a => a.Item3.BoolValue))
            {
                newPersonFollowUp.PeopleFollowUpHouseholdTasks.Add(new PersonFollowUpHouseholdTask
                {
                    PersonFollowUp = newPersonFollowUp,
                    HouseholdTask = bindableHouseholdTask.Item2
                });
            }

            // get custom field values
            for (var i = 0; i < CustomFields.Count; i++)
            {
                var newCustomValue = new CustomPersonFollowUpValue
                {
                    CreatedAt = DateTime.Now,
                    LastUpdatedAt = DateTime.Now,
                    SoftDeleted = false,
                    CustomField = CustomFields[i],
                    Value = "",
                    PersonFollowUp = newPersonFollowUp
                };

                switch (CustomFields[i].FieldType)
                {
                    case @"text":
                        var textValue = ((CustomFieldStringValueViewModel)CustomFieldControls[i].BindingContext).EntryValue;
                        if (textValue != null && !textValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = textValue;
                            ApplicationInstanceData.Data.CustomPersonFollowUpValues.Add(newCustomValue);
                        }
                        break;
                    case @"textarea":
                        var textAreaValue = ((CustomFieldStringValueViewModel)CustomFieldControls[i].BindingContext).EntryValue;
                        if (textAreaValue != null && !textAreaValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = textAreaValue;
                            ApplicationInstanceData.Data.CustomPersonFollowUpValues.Add(newCustomValue);
                        }
                        break;
                    case @"check_box":
                        var checkBoxValue = ((CustomFieldSwitchArrayView)CustomFieldControls[i]).GetSelectedValues();
                        if (!checkBoxValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = checkBoxValue;
                            ApplicationInstanceData.Data.CustomPersonFollowUpValues.Add(newCustomValue);
                        }
                        break;
                    case @"radio_button":
                        var radioButtonValue = ((CustomFieldPickerViewModel)CustomFieldControls[i].BindingContext).SelectedBindableOption;
                        if (radioButtonValue != null && !radioButtonValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = radioButtonValue;
                            ApplicationInstanceData.Data.CustomPersonFollowUpValues.Add(newCustomValue);
                        }
                        break;
                    case @"select":
                        var selectValue = ((CustomFieldPickerViewModel)CustomFieldControls[i].BindingContext).SelectedBindableOption;
                        if (selectValue != null && !selectValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = selectValue;
                            ApplicationInstanceData.Data.CustomPersonFollowUpValues.Add(newCustomValue);
                        }
                        break;
                    case @"number":
                        var numberValue = ((CustomFieldDoubleValueViewModel)CustomFieldControls[i].BindingContext).GetDoubleValue();
                        if (numberValue != null)
                        {
                            newCustomValue.Value = numberValue.ToString();
                            ApplicationInstanceData.Data.CustomPersonFollowUpValues.Add(newCustomValue);
                        }
                        break;
                    case @"date":
                        var dateValue = ((CustomFieldDateTimeValueViewModel)CustomFieldControls[i].BindingContext).EntryValue;
                        var dateValueString = dateValue.ToString();
                        if (dateValueString != null && !dateValueString.Equals(string.Empty))
                        {
                            newCustomValue.Value = dateValueString;
                            ApplicationInstanceData.Data.CustomPersonFollowUpValues.Add(newCustomValue);
                        }
                        break;
                    case @"rank_list":
                        var rankedValues = ((CustomFieldRankListViewModel)CustomFieldControls[i].BindingContext).GetRankedValues();
                        newCustomValue.Value = rankedValues;
                        ApplicationInstanceData.Data.CustomPersonFollowUpValues.Add(newCustomValue);
                        break;
                    default:
                        break;
                }
            }

            _person.AddFollowUp(newPersonFollowUp);
            ApplicationInstanceData.Data.SaveChanges();

            IsBusy = false;
            Exit();
        }

        private void Exit()
        {
            ApplicationInstanceData.NavigationPage.PopAsync();
        } 

        private void ExecuteNavigateToAddServiceCommand()
        {
            // First check that at least 1 service exists in the local database
            if (!ApplicationInstanceData.Data.Services.Any())
            {
                ApplicationInstanceData.App.MainPage.DisplayAlert(
                    ApplicationInstanceData.SelectedLocalization.Translations[@"Alert"],
                    ApplicationInstanceData.SelectedLocalization.Translations[@"AlertMessageActionNotAllowedNoServices"],
                    ApplicationInstanceData.SelectedLocalization.Translations[@"OK"]);                
                return;
            }

            // Modal navigate to assign service
            ApplicationInstanceData.NavigationPage.PushAsync(new HouseholdMemberAssignServiceView
            {
                BindingContext = new HouseholdMemberAssignServiceViewModel(ApplicationInstanceData, _person)
            });
        }
    }
}
