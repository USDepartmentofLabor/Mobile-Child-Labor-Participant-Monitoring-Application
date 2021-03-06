﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.ViewModels.CustomControlViewModels;
using MDPMS.Shared.Views.CustomControls;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels.ContentViewModels
{
    public class PersonEditContentViewModel : ViewModelBase
    {
        Plugin.Geolocator.Abstractions.Position GPSPosition;

        public Person Person { get; set; }

        // temp/locally cached properties
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
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
        public bool? DateOfBirthIsApproximate { get; set; }
        public string RelationshipIfOther { get; set; }
        public DateTime? IntakeDate { get; set; }
        public bool? HaveJobReturningTo { get; set; }
        public int HoursWorked { get; set; }
        public int HoursWorkedOnHousework { get; set; }
        public bool? EnrolledInSchool { get; set; }

        // *** cached look ups and view helper vars ***
        public string CalculatedAge { get; set; }
        // Genders
        public Tuple<string, Gender> SelectedBindableGender { get; set; }
        // Bindables/Observables (Tuple<string, Object> = DisplayName, ObjectInstance)
        public ObservableCollection<Tuple<string, Gender>> BindableGenders { get; set; }

        // Person Relationships
        // Special bindable Tuple<string, PersonRelationship, bool> = DisplayName, PersonRelationship, IsOther
        public GridLength SelectedBindablePersonRelationshipIsOtherRowHeight { get; set; }
        public ObservableCollection<Tuple<string, PersonRelationship, bool>> BindablePersonRelationships { get; set; }
        private Tuple<string, PersonRelationship, bool> _selectedBindablePersonRelationship = new Tuple<string, PersonRelationship, bool>(@"", null, false);
        public Tuple<string, PersonRelationship, bool> SelectedBindablePersonRelationship
        {
            get => _selectedBindablePersonRelationship;
            set
            {
                _selectedBindablePersonRelationship = value;
                SelectedBindablePersonRelationshipIsOtherRowHeight = _selectedBindablePersonRelationship.Item3 ? 65 : 0;
                OnPropertyChanged(nameof(SelectedBindablePersonRelationshipIsOtherRowHeight));
            }
        }

        // value, question
        public ObservableCollection<Tuple<bool, StatusCustomizationWorkActivity, GenericSwitchTextViewModel>> BindableWorkActivities { get; set; }
        public ObservableCollection<Tuple<bool, StatusCustomizationHazardousCondition, GenericSwitchTextViewModel>> BindableHazardousConditions { get; set; }
        public ObservableCollection<Tuple<bool, StatusCustomizationHouseholdTask, GenericSwitchTextViewModel>> BindableHouseholdTasks { get; set; }

        // Custom fields
        public List<CustomField> CustomFields { get; set; }
        public List<ContentView> CustomFieldControls { get; set; }

        private readonly bool isCreate;
        private readonly Household household;

        public PersonEditContentViewModel(ApplicationInstanceData applicationInstanceData, Household parentHousehold)
        {
            isCreate = true;
            household = parentHousehold;
            Init(applicationInstanceData);
        }

        public PersonEditContentViewModel(ApplicationInstanceData applicationInstanceData, Person person)
        {
            isCreate = false;
            Person = person;
            Init(applicationInstanceData);
        }

        private void Init(ApplicationInstanceData applicationInstanceData)
        {
            ApplicationInstanceData = applicationInstanceData;

            // *** Cached Look Ups ***
            // Genders
            BindableGenders = new ObservableCollection<Tuple<string, Gender>>();
            foreach (var gender in ApplicationInstanceData.Data.Genders.OrderBy(a => a.DpmsGenderNumber))
            {
                BindableGenders.Add(new Tuple<string, Gender>(ApplicationInstanceData.SelectedLocalization.Translations[gender.GenderReadable], gender));
            }
            SelectedBindableGender = BindableGenders.First();
            if (!isCreate) if (Person.Gender != null) SelectedBindableGender = BindableGenders.First(a => a.Item2.GenderId == Person.Gender.GenderId);

            // Person Relationships
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
            if (!isCreate) if (Person.RelationshipToHeadOfHousehold != null)
                SelectedBindablePersonRelationship =
                    BindablePersonRelationships.Where(b => b.Item2 != null).First(a => a.Item2.InternalId == Person.RelationshipToHeadOfHousehold.InternalId);

            // Work Activities
            BindableWorkActivities = new ObservableCollection<Tuple<bool, StatusCustomizationWorkActivity, GenericSwitchTextViewModel>>();
            foreach (var workActivity in ApplicationInstanceData.Data.StatusCustomizationWorkActivities)
            {
                var value = false;
                if (!isCreate) value = Person.PeopleWorkActivities.Select(a => a.WorkActivityInternalId).Contains(workActivity.InternalId);
                BindableWorkActivities.Add(new Tuple<bool, StatusCustomizationWorkActivity, GenericSwitchTextViewModel>(value, workActivity,
                    new GenericSwitchTextViewModel(workActivity.DisplayName, value)));
            }

            // Hazardous Conditions
            BindableHazardousConditions = new ObservableCollection<Tuple<bool, StatusCustomizationHazardousCondition, GenericSwitchTextViewModel>>();
            foreach (var hazardousCondition in ApplicationInstanceData.Data.StatusCustomizationHazardousConditions)
            {
                var value = false;
                if (!isCreate) value = Person.PeopleHazardousConditions.Select(a => a.HazardousConditionInternalId).Contains(hazardousCondition.InternalId);
                BindableHazardousConditions.Add(new Tuple<bool, StatusCustomizationHazardousCondition, GenericSwitchTextViewModel>(value, hazardousCondition,
                    new GenericSwitchTextViewModel(hazardousCondition.DisplayName, value)));
            }

            // Household Tasks
            BindableHouseholdTasks = new ObservableCollection<Tuple<bool, StatusCustomizationHouseholdTask, GenericSwitchTextViewModel>>();
            foreach (var householdTask in ApplicationInstanceData.Data.StatusCustomizationHouseholdTasks)
            {
                var value = false;
                if (!isCreate) value = Person.PeopleHouseholdTasks.Select(a => a.HouseholdTaskInternalId).Contains(householdTask.InternalId);
                BindableHouseholdTasks.Add(new Tuple<bool, StatusCustomizationHouseholdTask, GenericSwitchTextViewModel>(value, householdTask,
                    new GenericSwitchTextViewModel(householdTask.DisplayName, value)));
            }

            // Custom Fields
            var customFieldInit = Helpers.CustomFieldInit.InitCustomFields(@"Person", applicationInstanceData);
            CustomFields = customFieldInit.Item1;
            CustomFieldControls = customFieldInit.Item2;

            // *** Person ***
            if (isCreate)
            {
                Person = new Person
                {
                    SoftDeleted = false,
                    LastName = @"",
                    FirstName = @"",
                    MiddleName = @"",
                    Gender = null,
                    DateOfBirth = DateTime.Today,
                    DateOfBirthIsApproximate = false,
                    RelationshipIfOther = @"",
                    IntakeDate = DateTime.Today,
                    HaveJobReturningTo = false,
                    HoursWorked = null,
                    HouseWorkedOnHousework = null,
                    EnrolledInSchool = false,
                    PeopleHazardousConditions = new List<PersonHazardousCondition>(),
                    PeopleWorkActivities = new List<PersonWorkActivity>(),
                    PeopleHouseholdTasks = new List<PersonHouseholdTask>()
                };
            }

            // set temp/locally cached properties to support save/cancel
            LastName = Person.LastName;
            FirstName = Person.FirstName;
            MiddleName = Person.MiddleName;
            DateOfBirth = Person.DateOfBirth;
            DateOfBirthIsApproximate = Person.DateOfBirthIsApproximate;
            RelationshipIfOther = Person.RelationshipIfOther;
            IntakeDate = Person.IntakeDate;
            HaveJobReturningTo = Person.HaveJobReturningTo;
            HoursWorked = (Person.HoursWorked == null) ? 0 : (int)Person.HoursWorked;
            HoursWorkedOnHousework = (Person.HouseWorkedOnHousework == null) ? 0 : (int)Person.HouseWorkedOnHousework;
            EnrolledInSchool = Person.EnrolledInSchool;
        }

        public bool ValidatePerson()
        {
            // Required { DOB, Gender, First Name, Last Name }
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

        public async Task SavePerson()
        {
            if (isCreate) GPSPosition = await Helper.Gps.GpsHelper.GetGpsPosition();
            SavePersonCommon();
        }

        private void SavePersonCommon()
        {
            // Create vs. Edit

            // Common
            var now = DateTime.UtcNow;
            Person.LastUpdatedAt = now;
            Person.FirstName = FirstName;
            Person.LastName = LastName;
            Person.MiddleName = MiddleName;
            Person.Gender = SelectedBindableGender.Item2;
            Person.DateOfBirth = DateOfBirth;
            Person.DateOfBirthIsApproximate = DateOfBirthIsApproximate;
            Person.IntakeDate = IntakeDate;
            if (SelectedBindablePersonRelationship != null) Person.RelationshipToHeadOfHousehold = SelectedBindablePersonRelationship.Item2;
            Person.RelationshipIfOther = RelationshipIfOther;
            Person.HaveJobReturningTo = HaveJobReturningTo;
            Person.HoursWorked = HoursWorked;
            Person.HouseWorkedOnHousework = HoursWorkedOnHousework;
            Person.EnrolledInSchool = EnrolledInSchool;

            if (isCreate)
            {
                Person.CreatedAt = now;
                SaveNew(now);
            }
            else
            {
                UpdateExisting(now);
            }
        }

        private void SaveNew(DateTime now)
        {
            // GPS location
            Person.GpsLatitude = GPSPosition?.Latitude;
            Person.GpsLongitude = GPSPosition?.Longitude;
            Person.GpsPositionAccuracy = GPSPosition?.Accuracy;
            Person.GpsAltitude = GPSPosition?.Altitude;
            Person.GpsAltitudeAccuracy = GPSPosition?.AltitudeAccuracy;
            Person.GpsHeading = GPSPosition?.Heading;
            Person.GpsSpeed = GPSPosition?.Speed;
            Person.GpsPositionTime = now;

            // Status Customizations
            foreach (var bindableHazardousCondition in BindableHazardousConditions.Where(a => a.Item3.BoolValue))
            {
                Person.PeopleHazardousConditions.Add(new PersonHazardousCondition
                {
                    Person = Person,
                    HazardousCondition = bindableHazardousCondition.Item2
                });
            }

            foreach (var bindableWorkActivity in BindableWorkActivities.Where(a => a.Item3.BoolValue))
            {
                Person.PeopleWorkActivities.Add(new PersonWorkActivity
                {
                    Person = Person,
                    WorkActivity = bindableWorkActivity.Item2
                });
            }

            foreach (var bindableHouseholdTask in BindableHouseholdTasks.Where(a => a.Item3.BoolValue))
            {
                Person.PeopleHouseholdTasks.Add(new PersonHouseholdTask
                {
                    Person = Person,
                    HouseholdTask = bindableHouseholdTask.Item2
                });
            }

            household.AddMember(Person);
            ApplicationInstanceData.Data.SaveChanges();

            // Custom Values
            for (var i = 0; i < CustomFields.Count; i++)
            {
                var newCustomValue = new CustomPersonValue
                {
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow,
                    SoftDeleted = false,
                    CustomField = CustomFields[i],
                    Value = "",
                    Person = Person,
                    InternalParentId = Person.InternalId
                };

                switch (CustomFields[i].FieldType)
                {
                    case @"text":
                        var textValue = ((CustomFieldStringValueViewModel)CustomFieldControls[i].BindingContext).EntryValue;
                        if (textValue != null && !textValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = Helpers.CustomValueConverter.ConvertCustomValueToJsonText(textValue);
                            ApplicationInstanceData.Data.CustomPersonValues.Add(newCustomValue);
                        }
                        break;
                    case @"textarea":
                        var textAreaValue = ((CustomFieldStringValueViewModel)CustomFieldControls[i].BindingContext).EntryValue;
                        if (textAreaValue != null && !textAreaValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = Helpers.CustomValueConverter.ConvertCustomValueToJsonTextArea(textAreaValue);
                            ApplicationInstanceData.Data.CustomPersonValues.Add(newCustomValue);
                        }
                        break;
                    case @"check_box":
                        var checkBoxValues = ((CustomFieldSwitchArrayView)CustomFieldControls[i]).GetSelectedValuesAsList();
                        if (checkBoxValues.Any())
                        {
                            newCustomValue.Value = Helpers.CustomValueConverter.ConvertCustomValueToJsonCheckBox(checkBoxValues);
                            ApplicationInstanceData.Data.CustomPersonValues.Add(newCustomValue);
                        }
                        break;
                    case @"radio_button":
                        var radioButtonValue = ((CustomFieldPickerViewModel)CustomFieldControls[i].BindingContext).SelectedBindableOption;
                        if (radioButtonValue != null && !radioButtonValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = Helpers.CustomValueConverter.ConvertCustomValueToJsonRadioButton(radioButtonValue);
                            ApplicationInstanceData.Data.CustomPersonValues.Add(newCustomValue);
                        }
                        break;
                    case @"select":
                        var selectValue = ((CustomFieldPickerViewModel)CustomFieldControls[i].BindingContext).SelectedBindableOption;
                        if (selectValue != null && !selectValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = Helpers.CustomValueConverter.ConvertCustomValueToJsonSelect(selectValue);
                            ApplicationInstanceData.Data.CustomPersonValues.Add(newCustomValue);
                        }
                        break;
                    case @"number":
                        var numberValue = ((CustomFieldDoubleValueViewModel)CustomFieldControls[i].BindingContext).GetDoubleValue();
                        if (numberValue != null)
                        {
                            newCustomValue.Value = Helpers.CustomValueConverter.ConvertCustomValueToJsonNumber((double)numberValue);
                            ApplicationInstanceData.Data.CustomPersonValues.Add(newCustomValue);
                        }
                        break;
                    case @"date":
                        var dateValue = ((CustomFieldDateTimeValueViewModel)CustomFieldControls[i].BindingContext).DateValue;
                        if (dateValue != null && !dateValue.ToString().Equals(string.Empty))
                        {
                            newCustomValue.Value = Helpers.CustomValueConverter.ConvertCustomValueToJsonDate((DateTime)dateValue);
                            ApplicationInstanceData.Data.CustomPersonValues.Add(newCustomValue);
                        }
                        break;
                    case @"rank_list":
                        var rankedValues = ((CustomFieldRankListViewModel)CustomFieldControls[i].BindingContext).GetRankedValues();
                        if (!rankedValues.Equals(@""))
                        {
                            newCustomValue.Value = Helpers.CustomValueConverter.ConvertCustomValueToJsonRankList(((CustomFieldRankListViewModel)CustomFieldControls[i].BindingContext).Entries.ToList());
                            ApplicationInstanceData.Data.CustomPersonValues.Add(newCustomValue);
                        }
                        break;
                    default:
                        break;
                }
            }

            ApplicationInstanceData.Data.SaveChanges();
        }

        private void UpdateExisting(DateTime now)
        {
            // Status Customizations edit
            foreach (var bindableHazardousCondition in BindableHazardousConditions)
            {
                if (bindableHazardousCondition.Item3.BoolValue)
                {
                    // true
                    if (!Person.PeopleHazardousConditions.Any(a => a.HazardousConditionInternalId == bindableHazardousCondition.Item2.InternalId))
                    {
                        // add value
                        Person.PeopleHazardousConditions.Add(new PersonHazardousCondition
                        {
                            Person = Person,
                            HazardousCondition = bindableHazardousCondition.Item2
                        });
                    }
                }
                else
                {
                    // false
                    var query = Person.PeopleHazardousConditions.Where(a => a.HazardousConditionInternalId == bindableHazardousCondition.Item2.InternalId);
                    if (query.Any())
                    {
                        // delete the value
                        var ids = query.Select(a => a.HazardousConditionInternalId).ToList();
                        foreach (var x in ids)
                        {
                            Person.PeopleHazardousConditions.Remove(Person.PeopleHazardousConditions.First(a => a.HazardousConditionInternalId == x));
                        }
                    }
                }
            }

            foreach (var bindableWorkActivity in BindableWorkActivities)
            {
                if (bindableWorkActivity.Item3.BoolValue)
                {
                    if (!Person.PeopleWorkActivities.Any(a => a.WorkActivityInternalId == bindableWorkActivity.Item2.InternalId))
                    {
                        Person.PeopleWorkActivities.Add(new PersonWorkActivity
                        {
                            Person = Person,
                            WorkActivity = bindableWorkActivity.Item2
                        });
                    }
                }
                else
                {
                    var query = Person.PeopleWorkActivities.Where(a => a.WorkActivityInternalId == bindableWorkActivity.Item2.InternalId);
                    if (query.Any())
                    {
                        var ids = query.Select(a => a.WorkActivityInternalId).ToList();
                        foreach (var x in ids)
                        {
                            Person.PeopleWorkActivities.Remove(Person.PeopleWorkActivities.First(a => a.WorkActivityInternalId == x));
                        }
                    }
                }
            }

            foreach (var bindableHouseholdTask in BindableHouseholdTasks)
            {
                if (bindableHouseholdTask.Item3.BoolValue)
                {
                    if (!Person.PeopleHouseholdTasks.Any(a => a.HouseholdTaskInternalId == bindableHouseholdTask.Item2.InternalId))
                    {
                        Person.PeopleHouseholdTasks.Add(new PersonHouseholdTask
                        {
                            Person = Person,
                            HouseholdTask = bindableHouseholdTask.Item2
                        });
                    }
                }
                else
                {
                    var query = Person.PeopleHouseholdTasks.Where(a => a.HouseholdTaskInternalId == bindableHouseholdTask.Item2.InternalId);
                    if (query.Any())
                    {
                        var ids = query.Select(a => a.HouseholdTaskInternalId).ToList();
                        foreach (var x in ids)
                        {
                            Person.PeopleHouseholdTasks.Remove(Person.PeopleHouseholdTasks.First(a => a.HouseholdTaskInternalId == x));
                        }
                    }
                }
            }

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

                var existingValueQuery = ApplicationInstanceData.Data.CustomPersonValues
                                                                .Where(a => a.Person.InternalId == Person.InternalId && a.CustomField.InternalId == CustomFields[i].InternalId);
                
                if (hasValue)
                {
                    // check to see if new needed
                    if (!existingValueQuery.Any())
                    {
                        // add a record
                        var newCustomValue = new CustomPersonValue
                        {
                            CreatedAt = now,
                            LastUpdatedAt = now,
                            SoftDeleted = false,
                            CustomField = CustomFields[i],
                            Value = jsonValue,
                            Person = Person,
                            InternalParentId = Person.InternalId
                        };
                        ApplicationInstanceData.Data.CustomPersonValues.Add(newCustomValue);
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
                    if (existingValueQuery.Any()) ApplicationInstanceData.Data.CustomPersonValues.Remove(existingValueQuery.First());
                }
            }                

            ApplicationInstanceData.Data.SaveChanges();
        }

    }
}
