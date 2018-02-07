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
            IntakeDateValue = person.IntakeDate == null ? @"" : person.IntakeDate.ToString();
            GenderValue = person.Gender == null ? @"" : person.Gender.GenderReadable;
            DateOfBirthValue = person.DateOfBirth == null ? @"" : person.DateOfBirth.ToString();
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

            _person.AddFollowUp(newPersonFollowUp);
            ApplicationInstanceData.Data.SaveChanges();

            IsBusy = false;
            Exit();
        }

        private void Exit()
        {
            ApplicationInstanceData.NavigationPage.PopAsync();

            // reset member search
            var viewModel = (HouseholdMembersSearchViewModel)ApplicationInstanceData.NavigationPage.Pages.First().BindingContext;
            viewModel.RefreshCommand.Execute(null);
        } 

        private void ExecuteNavigateToAddServiceCommand()
        {
            // Modal navigate to assign service
            ApplicationInstanceData.NavigationPage.PushAsync(new HouseholdMemberAssignServiceView
            {
                BindingContext = new HouseholdMemberAssignServiceViewModel(ApplicationInstanceData, _person)
            });
        }
    }
}
