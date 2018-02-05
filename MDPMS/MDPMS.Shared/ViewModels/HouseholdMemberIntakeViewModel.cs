using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels
{
    public class HouseholdMemberIntakeViewModel : ViewModelBase
    {
        Position GPSPosition;


        public Command CancelCommand { get; set; }
        public Command SubmitCommand { get; set; }

        public DateTime IntakeDate { get; set; } = DateTime.Today;
        public string FirstName { get; set; } = @"";
        public string LastName { get; set; } = @"";
        public string MiddleName { get; set; } = @"";
        public Tuple<string, Gender> SelectedBindableGender { get; set; }
        public DateTime? DateOfBirth { get; set; } = DateTime.Today;
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

        public HouseholdMemberIntakeViewModel(ApplicationInstanceData applicationInstanceData)
        {
            ApplicationInstanceData = applicationInstanceData;
            
            BindableGenders = new ObservableCollection<Tuple<string, Gender>>
            {
                new Tuple<string, Gender>(@"Select Gender", null),
                new Tuple<string, Gender>(@"", null)
            };
            foreach (var gender in ApplicationInstanceData.Data.Genders)
            {
                BindableGenders.Add(new Tuple<string, Gender>(gender.GenderReadable, gender));
            }
            SelectedBindableGender = BindableGenders.First();

            BindablePersonRelationships = new ObservableCollection<Tuple<string, PersonRelationship, bool>>
            {
                new Tuple<string, PersonRelationship, bool>(@"Select Relationship", null, false),
                new Tuple<string, PersonRelationship, bool>(@"", null, false)
            };
            foreach (var personRelationship in ApplicationInstanceData.Data.PersonRelationships)
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

            CancelCommand = new Command(ExecuteCancelCommand);
            SubmitCommand = new Command(ExecuteSubmitCommand);
        }

        private void ExecuteCancelCommand()
        {
            Exit();
        }

        private async void ExecuteSubmitCommand()
        {
            var position = await CrossGeolocator.Current.GetPositionAsync(new TimeSpan(0, 0, 0, 0, 1000));
            GPSPosition = position;
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
                GpsLatitude = GPSPosition.Latitude,
                GpsLongitude = GPSPosition.Longitude,
                GpsPositionAccuracy = GPSPosition.Accuracy,
                GpsAltitude = GPSPosition.Altitude,
                GpsAltitudeAccuracy = GPSPosition.AltitudeAccuracy,
                GpsHeading = GPSPosition.Heading,
                GpsSpeed = GPSPosition.Speed,
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

            if (ApplicationInstanceData.NavigationPage.Pages.First().GetType() == typeof(HouseholdIntakeView))
            {
                var householdsViewModel =
                    (HouseholdIntakeViewModel)ApplicationInstanceData.NavigationPage.Pages.First().BindingContext;
                householdsViewModel.HouseholdMembers.Add(person);
            }

            Exit();
        }

        private void Exit()
        {
            ApplicationInstanceData.NavigationPage.PopAsync();
        }
    }
}
