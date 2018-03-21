using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels
{
    public class HouseholdMemberIntakeViewModel : ViewModelBase
    {
        Position GPSPosition;

        public string CalculatedAge { get; set; }

        public Command CancelCommand { get; set; }
        public Command SubmitCommand { get; set; }

        public DateTime IntakeDate { get; set; } = DateTime.Today;
        public string FirstName { get; set; } = @"";
        public string LastName { get; set; } = @"";
        public string MiddleName { get; set; } = @"";
        public Tuple<string, Gender> SelectedBindableGender { get; set; }

        private DateTime? _dateOfBirth = DateTime.Today;
        public DateTime? DateOfBirth
        {
            get
            {
                return _dateOfBirth;
            }
            set
            {
                _dateOfBirth = value;
                // Calulate age for form preview
                if (_dateOfBirth != null)
                {
                    DateTime now = DateTime.Today;
                    DateTime bday = (DateTime)_dateOfBirth;
                    var age = now.Year - bday.Year;
                    if (now < bday.AddYears(age)) age--;
                    CalculatedAge = age.ToString();
                }
                else
                {
                    CalculatedAge = @"";
                }
                OnPropertyChanged(nameof(CalculatedAge));                
            }
        }

        public bool BirthDateIsApproximate { get; set; } = false;

        private Tuple<string, PersonRelationship, bool> _selectedBindablePersonRelationship = new Tuple<string, PersonRelationship, bool>(@"", null, false);

        public Tuple<string, PersonRelationship, bool> SelectedBindablePersonRelationship
        {
            get => _selectedBindablePersonRelationship;
            set
            {
                _selectedBindablePersonRelationship = value;
                SelectedBindablePersonRelationshipIsOther = _selectedBindablePersonRelationship.Item3;
                OnPropertyChanged(nameof(SelectedBindablePersonRelationshipIsOther));                
            }
        }

        public string RelationshipOther { get; set; } = @"";
        public bool WorkActivityNoActivityQuestionAnswer { get; set; } = false;
        public int WorkActivityHoursEngaged { get; set; } = 0;
        public int HouseholdTasksHoursEngaged { get; set; } = 0;
        public bool EnrolledInSchoolCollege { get; set; } = false;

        // Bindables/Observables (Tuple<string, Object> = DisplayName, ObjectInstance)
        public ObservableCollection<Tuple<string, Gender>> BindableGenders { get; set; }
        // Special bindable Tuple<string, PersonRelationship, bool> = DisplayName, PersonRelationship, IsOther
        public ObservableCollection<Tuple<string, PersonRelationship, bool>> BindablePersonRelationships { get; set; }
        
        public bool SelectedBindablePersonRelationshipIsOther { get; set; }

        // value, question
        public ObservableCollection<Tuple<bool, StatusCustomizationWorkActivity, GenericSwitchTextViewModel>> BindableWorkActivities { get; set; }
        public ObservableCollection<Tuple<bool, StatusCustomizationHazardousCondition, GenericSwitchTextViewModel>> BindableHazardousConditions { get; set; }
        public ObservableCollection<Tuple<bool, StatusCustomizationHouseholdTask, GenericSwitchTextViewModel>> BindableHouseholdTasks { get; set; }

        // Custom fields
        public List<CustomField> CustomFields { get; set; }
        public List<ContentView> CustomFieldControls { get; set; }

        public HouseholdMemberIntakeViewModel(ApplicationInstanceData applicationInstanceData)
        {
            ApplicationInstanceData = applicationInstanceData;
            
            BindableGenders = new ObservableCollection<Tuple<string, Gender>>
            {
                new Tuple<string, Gender>(ApplicationInstanceData.SelectedLocalization.Translations[@"SelectGender"], null),
                new Tuple<string, Gender>(@"", null)
            };
            foreach (var gender in ApplicationInstanceData.Data.Genders.OrderBy(a => a.DpmsGenderNumber))
            {
                BindableGenders.Add(new Tuple<string, Gender>(ApplicationInstanceData.SelectedLocalization.Translations[gender.GenderReadable], gender));
            }
            SelectedBindableGender = BindableGenders.First();

            BindablePersonRelationships = new ObservableCollection<Tuple<string, PersonRelationship, bool>>
            {
                new Tuple<string, PersonRelationship, bool>(ApplicationInstanceData.SelectedLocalization.Translations[@"SelectRelationship"], null, false),
                new Tuple<string, PersonRelationship, bool>(@"", null, false)
            };
            foreach (var personRelationship in ApplicationInstanceData.Data.PersonRelationships.OrderBy(a => a.Code))
            {
                BindablePersonRelationships.Add(new Tuple<string, PersonRelationship, bool>(personRelationship.DisplayName, personRelationship, personRelationship.CanonicalName.Equals(@"OTHER")));
            }
            SelectedBindablePersonRelationship = BindablePersonRelationships.First();
            
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
            CustomFields = ApplicationInstanceData.Data.CustomFields.Where(a => a.ModelType.Equals(@"Person")).OrderBy(b => b.SortOrder).ToList();
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
        }

        private void ExecuteCancelCommand()
        {
            Exit();
        }

        private async void ExecuteSubmitCommand()
        {
            if (!NewHouseholdMemberValidation()) return;

            IsBusy = true;
            GPSPosition = await Helper.Gps.GpsHelper.GetGpsPosition();
            ExecutePostSubmitCommand();
        }

        private void ExecutePostSubmitCommand()
        {
            // save
            var person = new Person
            {
                CreatedAt = DateTime.Now,
                LastUpdatedAt = DateTime.Now,
                SoftDeleted = false,
                FirstName = FirstName,
                LastName = LastName,
                MiddleName = MiddleName,
                Gender = SelectedBindableGender.Item2,
                DateOfBirth = DateOfBirth,
                DateOfBirthIsApproximate = BirthDateIsApproximate,
                IntakeDate = IntakeDate,
                RelationshipToHeadOfHousehold = SelectedBindablePersonRelationship.Item2,
                RelationshipIfOther = RelationshipOther,
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
                PeopleHazardousConditions = new List<PersonHazardousCondition>(),
                PeopleWorkActivities = new List<PersonWorkActivity>(),
                PeopleHouseholdTasks = new List<PersonHouseholdTask>()
            };

            foreach (var bindableHazardousCondition in BindableHazardousConditions.Where(a => a.Item3.BoolValue))
            {
                person.PeopleHazardousConditions.Add(new PersonHazardousCondition
                {
                    Person = person,
                    HazardousCondition = bindableHazardousCondition.Item2
                });
            }

            foreach (var bindableWorkActivity in BindableWorkActivities.Where(a => a.Item3.BoolValue))
            {
                person.PeopleWorkActivities.Add(new PersonWorkActivity
                {
                    Person = person,
                    WorkActivity = bindableWorkActivity.Item2
                });
            }

            foreach (var bindableHouseholdTask in BindableHouseholdTasks.Where(a => a.Item3.BoolValue))
            {
                person.PeopleHouseholdTasks.Add(new PersonHouseholdTask
                {
                    Person = person,
                    HouseholdTask = bindableHouseholdTask.Item2
                });
            }

            // get custom field values
            for (var i = 0; i < CustomFields.Count; i++)
            {
                var newCustomValue = new CustomPersonValue
                {
                    CreatedAt = DateTime.Now,
                    LastUpdatedAt = DateTime.Now,
                    SoftDeleted = false,
                    CustomField = CustomFields[i],
                    Value = "",
                    Person = person
                };

                switch (CustomFields[i].FieldType)
                {                    
                    case @"text":     
                        var textValue = ((CustomFieldStringValueViewModel)CustomFieldControls[i].BindingContext).EntryValue;
                        if (textValue!= null && !textValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = textValue;
                            ApplicationInstanceData.Data.CustomPersonValues.Add(newCustomValue);
                        }
                        break;
                    case @"textarea":
                        var textAreaValue = ((CustomFieldStringValueViewModel)CustomFieldControls[i].BindingContext).EntryValue;
                        if (textAreaValue != null && !textAreaValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = textAreaValue;
                            ApplicationInstanceData.Data.CustomPersonValues.Add(newCustomValue);
                        }
                        break;
                    case @"check_box":
                        var checkBoxValue = ((CustomFieldSwitchArrayView)CustomFieldControls[i]).GetSelectedValues();
                        if (!checkBoxValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = checkBoxValue;
                            ApplicationInstanceData.Data.CustomPersonValues.Add(newCustomValue);
                        }
                        break;
                    case @"radio_button":
                        var radioButtonValue = ((CustomFieldPickerViewModel)CustomFieldControls[i].BindingContext).SelectedBindableOption;
                        if (radioButtonValue != null && !radioButtonValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = radioButtonValue;
                            ApplicationInstanceData.Data.CustomPersonValues.Add(newCustomValue);
                        }
                        break;
                    case @"select":
                        var selectValue = ((CustomFieldPickerViewModel)CustomFieldControls[i].BindingContext).SelectedBindableOption;
                        if (selectValue != null && !selectValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = selectValue;
                            ApplicationInstanceData.Data.CustomPersonValues.Add(newCustomValue);
                        }
                        break;
                    case @"number":
                        var numberValue = ((CustomFieldDoubleValueViewModel)CustomFieldControls[i].BindingContext).GetDoubleValue();
                        if (numberValue != null)
                        {
                            newCustomValue.Value = numberValue.ToString();
                            ApplicationInstanceData.Data.CustomPersonValues.Add(newCustomValue);
                        }
                        break;
                    case @"date":
                        var dateValue = ((CustomFieldDateTimeValueViewModel)CustomFieldControls[i].BindingContext).DateValue;
                        if (dateValue != null && !dateValue.ToString().Equals(string.Empty))
                        {
                            newCustomValue.Value = dateValue.ToString();
                            ApplicationInstanceData.Data.CustomPersonValues.Add(newCustomValue);
                        }
                        break;
                    case @"rank_list":
                        var rankedValues = ((CustomFieldRankListViewModel)CustomFieldControls[i].BindingContext).GetRankedValues();
                        newCustomValue.Value = rankedValues;
                        ApplicationInstanceData.Data.CustomPersonValues.Add(newCustomValue);
                        break;
                    default:
                        break;
                }
            }

            if (ApplicationInstanceData.NavigationPage.Pages.First().GetType() == typeof(HouseholdIntakeView))
            {
                var householdsViewModel =
                    (HouseholdIntakeViewModel)ApplicationInstanceData.NavigationPage.Pages.First().BindingContext;
                householdsViewModel.HouseholdMembers.Add(person);
            }

            IsBusy = false;
            Exit();
        }

        private bool NewHouseholdMemberValidation()
        {
            var validationMessages = new List<string>();

            if (SelectedBindableGender.Item2 == null)
            {
                validationMessages.Add(ApplicationInstanceData.SelectedLocalization.Translations[@"ErrorGenderMustBeSelected"]);
            }

            var validateableLastName = LastName.Replace(" ", "");
            if (validateableLastName.Equals(string.Empty))
            {
                validationMessages.Add(ApplicationInstanceData.SelectedLocalization.Translations[@"ErrorLastNameCanNotBeBlank"]);
            }

            var validateableFirstName = FirstName.Replace(" ", "");
            if (validateableFirstName.Equals(string.Empty))
            {
                validationMessages.Add(ApplicationInstanceData.SelectedLocalization.Translations[@"ErrorFirstNameCanNotBeBlank"]);
            }

            if (validationMessages.Any())
            {
                var messageContent = new StringBuilder();
                foreach (var message in validationMessages) messageContent.AppendLine(message);
                ApplicationInstanceData.App.MainPage.DisplayAlert(
                    ApplicationInstanceData.SelectedLocalization.Translations[@"Error"],
                    messageContent.ToString(),
                    ApplicationInstanceData.SelectedLocalization.Translations[@"OK"]);
                return false;
            }
            return true;
        }

        private void Exit()
        {
            ApplicationInstanceData.NavigationPage.PopAsync();
        }
    }
}
