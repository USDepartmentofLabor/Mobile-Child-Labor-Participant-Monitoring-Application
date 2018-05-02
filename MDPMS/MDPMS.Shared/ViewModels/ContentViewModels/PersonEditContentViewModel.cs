using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views;
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
                if (!isCreate) value = Person.PeopleWorkActivities.Any(a => a.WorkActivity.InternalId == workActivity.InternalId);
                BindableWorkActivities.Add(new Tuple<bool, StatusCustomizationWorkActivity, GenericSwitchTextViewModel>(value, workActivity,
                    new GenericSwitchTextViewModel(workActivity.DisplayName, value)));
            }

            // Hazardous Conditions
            BindableHazardousConditions = new ObservableCollection<Tuple<bool, StatusCustomizationHazardousCondition, GenericSwitchTextViewModel>>();
            foreach (var hazardousCondition in ApplicationInstanceData.Data.StatusCustomizationHazardousConditions)
            {
                var value = false;
                if (!isCreate) value = Person.PeopleHazardousConditions.Any(a => a.HazardousCondition.InternalId == hazardousCondition.InternalId);
                BindableHazardousConditions.Add(new Tuple<bool, StatusCustomizationHazardousCondition, GenericSwitchTextViewModel>(value, hazardousCondition,
                    new GenericSwitchTextViewModel(hazardousCondition.DisplayName, value)));
            }

            // Household Tasks
            BindableHouseholdTasks = new ObservableCollection<Tuple<bool, StatusCustomizationHouseholdTask, GenericSwitchTextViewModel>>();
            foreach (var householdTask in ApplicationInstanceData.Data.StatusCustomizationHouseholdTasks)
            {
                var value = false;
                if (!isCreate) value = Person.PeopleHouseholdTasks.Any(a => a.HouseholdTask.InternalId == householdTask.InternalId);
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

        public async void SavePerson()
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
                UpdateExisting();
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

            // Custom Values
            for (var i = 0; i < CustomFields.Count; i++)
            {
                var newCustomValue = new CustomPersonValue
                {
                    CreatedAt = DateTime.Now,
                    LastUpdatedAt = DateTime.Now,
                    SoftDeleted = false,
                    CustomField = CustomFields[i],
                    Value = "",
                    Person = Person
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

            household.AddMember(Person);
            ApplicationInstanceData.Data.SaveChanges();
        }

        private void UpdateExisting()
        {
            // Status Customizations edit

            // Custom Values edit
        }

    }
}
