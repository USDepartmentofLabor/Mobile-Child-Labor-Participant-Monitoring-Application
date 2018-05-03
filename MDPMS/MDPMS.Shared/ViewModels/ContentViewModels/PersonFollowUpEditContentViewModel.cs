using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels.ContentViewModels
{
    public class PersonFollowUpEditContentViewModel : ViewModelBase
    {
        public PersonFollowUp PersonFollowUp { get; set; }

        private readonly bool isCreate;
        private readonly Person person;

        // temp/locally cached properties
        public DateTime? FollowUpDate { get; set; }
        public bool? HaveJobReturningTo { get; set; }
        public int HoursWorked { get; set; }
        public int HouseWorkedOnHousework { get; set; }
        public bool? EnrolledInSchool { get; set; }

        // value, question
        public ObservableCollection<Tuple<bool, StatusCustomizationWorkActivity, GenericSwitchTextViewModel>> BindableWorkActivities { get; set; }
        public ObservableCollection<Tuple<bool, StatusCustomizationHazardousCondition, GenericSwitchTextViewModel>> BindableHazardousConditions { get; set; }
        public ObservableCollection<Tuple<bool, StatusCustomizationHouseholdTask, GenericSwitchTextViewModel>> BindableHouseholdTasks { get; set; }

        // Custom fields
        public List<CustomField> CustomFields { get; set; }
        public List<ContentView> CustomFieldControls { get; set; }

        public PersonFollowUpEditContentViewModel(ApplicationInstanceData applicationInstanceData, Person parentPerson)
        {
            isCreate = true;
            person = parentPerson;
            Init(applicationInstanceData);
        }

        public PersonFollowUpEditContentViewModel(ApplicationInstanceData applicationInstanceData, PersonFollowUp personFollowUp)
        {
            isCreate = false;
            PersonFollowUp = personFollowUp;
            Init(applicationInstanceData);
        }

        private void Init(ApplicationInstanceData applicationInstanceData)
        {
            ApplicationInstanceData = applicationInstanceData;

            // Work Activities
            BindableWorkActivities = new ObservableCollection<Tuple<bool, StatusCustomizationWorkActivity, GenericSwitchTextViewModel>>();
            foreach (var workActivity in ApplicationInstanceData.Data.StatusCustomizationWorkActivities)
            {
                var value = false;
                if (!isCreate) value = PersonFollowUp.PeopleFollowUpWorkActivities.Select(a => a.WorkActivityInternalId).Contains(workActivity.InternalId);
                BindableWorkActivities.Add(new Tuple<bool, StatusCustomizationWorkActivity, GenericSwitchTextViewModel>(value, workActivity,
                    new GenericSwitchTextViewModel(workActivity.DisplayName, value)));
            }

            // Hazardous Conditions
            BindableHazardousConditions = new ObservableCollection<Tuple<bool, StatusCustomizationHazardousCondition, GenericSwitchTextViewModel>>();
            foreach (var hazardousCondition in ApplicationInstanceData.Data.StatusCustomizationHazardousConditions)
            {
                var value = false;
                if (!isCreate) value = PersonFollowUp.PeopleFollowUpHazardousConditions.Select(a => a.HazardousConditionInternalId).Contains(hazardousCondition.InternalId);
                BindableHazardousConditions.Add(new Tuple<bool, StatusCustomizationHazardousCondition, GenericSwitchTextViewModel>(value, hazardousCondition,
                    new GenericSwitchTextViewModel(hazardousCondition.DisplayName, value)));
            }

            // Household Tasks
            BindableHouseholdTasks = new ObservableCollection<Tuple<bool, StatusCustomizationHouseholdTask, GenericSwitchTextViewModel>>();
            foreach (var householdTask in ApplicationInstanceData.Data.StatusCustomizationHouseholdTasks)
            {
                var value = false;
                if (!isCreate) value = PersonFollowUp.PeopleFollowUpHouseholdTasks.Select(a => a.HouseholdTaskInternalId).Contains(householdTask.InternalId);
                BindableHouseholdTasks.Add(new Tuple<bool, StatusCustomizationHouseholdTask, GenericSwitchTextViewModel>(value, householdTask,
                    new GenericSwitchTextViewModel(householdTask.DisplayName, value)));
            }

            // Custom Fields
            var customFieldInit = Helpers.CustomFieldInit.InitCustomFields(@"FollowUp", applicationInstanceData);
            CustomFields = customFieldInit.Item1;
            CustomFieldControls = customFieldInit.Item2;

            // *** PersonFollowUp ***
            if (isCreate)
            {
                PersonFollowUp = new PersonFollowUp
                {
                    SoftDeleted = false,
                    FollowUpDate = DateTime.Today,
                    HaveJobReturningTo = false,
                    HoursWorked = null,
                    HouseWorkedOnHousework = null,
                    EnrolledInSchool = false,

                    PeopleFollowUpHazardousConditions = new List<PersonFollowUpHazardousCondition>(),
                    PeopleFollowUpWorkActivities = new List<PersonFollowUpWorkActivity>(),
                    PeopleFollowUpHouseholdTasks = new List<PersonFollowUpHouseholdTask>()
                };
            }

            // set temp/locally cached properties to support save/cancel
            FollowUpDate = PersonFollowUp.FollowUpDate;
            HaveJobReturningTo = PersonFollowUp.HaveJobReturningTo;
            HoursWorked = (PersonFollowUp.HoursWorked == null) ? 0 : (int)PersonFollowUp.HoursWorked;
            HouseWorkedOnHousework = (PersonFollowUp.HouseWorkedOnHousework == null) ? 0 : (int)PersonFollowUp.HouseWorkedOnHousework;
            EnrolledInSchool = PersonFollowUp.EnrolledInSchool;
        }
    }
}
