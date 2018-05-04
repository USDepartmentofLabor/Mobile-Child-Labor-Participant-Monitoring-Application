using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels.ContentViewModels
{
    public class PersonFollowUpEditContentViewModel : ViewModelBase
    {
        Plugin.Geolocator.Abstractions.Position GPSPosition;

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

        public bool Validate()
        {            
            return true;
        }

        public async void Save()
        {
            if (isCreate) GPSPosition = await Helper.Gps.GpsHelper.GetGpsPosition();
            SaveCommon();
        }

        private void SaveCommon()
        {
            // Create vs. Edit

            // Common
            var now = DateTime.UtcNow;
            PersonFollowUp.LastUpdatedAt = now;
            PersonFollowUp.FollowUpDate = FollowUpDate;


            PersonFollowUp.HaveJobReturningTo = HaveJobReturningTo;
            PersonFollowUp.HoursWorked = HoursWorked;
            PersonFollowUp.HouseWorkedOnHousework = HouseWorkedOnHousework;
            PersonFollowUp.EnrolledInSchool = EnrolledInSchool;

            if (isCreate)
            {
                PersonFollowUp.CreatedAt = now;
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
            PersonFollowUp.GpsLatitude = GPSPosition?.Latitude;
            PersonFollowUp.GpsLongitude = GPSPosition?.Longitude;
            PersonFollowUp.GpsPositionAccuracy = GPSPosition?.Accuracy;
            PersonFollowUp.GpsAltitude = GPSPosition?.Altitude;
            PersonFollowUp.GpsAltitudeAccuracy = GPSPosition?.AltitudeAccuracy;
            PersonFollowUp.GpsHeading = GPSPosition?.Heading;
            PersonFollowUp.GpsSpeed = GPSPosition?.Speed;
            PersonFollowUp.GpsPositionTime = now;

            // Status Customizations
            foreach (var bindableHazardousCondition in BindableHazardousConditions.Where(a => a.Item3.BoolValue))
            {
                PersonFollowUp.PeopleFollowUpHazardousConditions.Add(new PersonFollowUpHazardousCondition
                {
                    PersonFollowUp = PersonFollowUp,
                    HazardousCondition = bindableHazardousCondition.Item2
                });
            }

            foreach (var bindableWorkActivity in BindableWorkActivities.Where(a => a.Item3.BoolValue))
            {
                PersonFollowUp.PeopleFollowUpWorkActivities.Add(new PersonFollowUpWorkActivity
                {
                    PersonFollowUp = PersonFollowUp,
                    WorkActivity = bindableWorkActivity.Item2
                });
            }

            foreach (var bindableHouseholdTask in BindableHouseholdTasks.Where(a => a.Item3.BoolValue))
            {
                PersonFollowUp.PeopleFollowUpHouseholdTasks.Add(new PersonFollowUpHouseholdTask
                {
                    PersonFollowUp = PersonFollowUp,
                    HouseholdTask = bindableHouseholdTask.Item2
                });
            }

            // Custom Values
            for (var i = 0; i < CustomFields.Count; i++)
            {
                var newCustomValue = new CustomPersonFollowUpValue
                {
                    CreatedAt = DateTime.Now,
                    LastUpdatedAt = DateTime.Now,
                    SoftDeleted = false,
                    CustomField = CustomFields[i],
                    Value = "",
                    PersonFollowUp = PersonFollowUp
                };

                switch (CustomFields[i].FieldType)
                {
                    case @"text":
                        var textValue = ((CustomFieldStringValueViewModel)CustomFieldControls[i].BindingContext).EntryValue;
                        if (textValue != null && !textValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = Helpers.CustomValueConverter.ConvertCustomValueToJsonText(textValue);
                            ApplicationInstanceData.Data.CustomPersonFollowUpValues.Add(newCustomValue);
                        }
                        break;
                    case @"textarea":
                        var textAreaValue = ((CustomFieldStringValueViewModel)CustomFieldControls[i].BindingContext).EntryValue;
                        if (textAreaValue != null && !textAreaValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = Helpers.CustomValueConverter.ConvertCustomValueToJsonTextArea(textAreaValue);
                            ApplicationInstanceData.Data.CustomPersonFollowUpValues.Add(newCustomValue);
                        }
                        break;
                    case @"check_box":
                        var checkBoxValues = ((CustomFieldSwitchArrayView)CustomFieldControls[i]).GetSelectedValuesAsList();
                        if (checkBoxValues.Any())
                        {
                            newCustomValue.Value = Helpers.CustomValueConverter.ConvertCustomValueToJsonCheckBox(checkBoxValues);
                            ApplicationInstanceData.Data.CustomPersonFollowUpValues.Add(newCustomValue);
                        }
                        break;
                    case @"radio_button":
                        var radioButtonValue = ((CustomFieldPickerViewModel)CustomFieldControls[i].BindingContext).SelectedBindableOption;
                        if (radioButtonValue != null && !radioButtonValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = Helpers.CustomValueConverter.ConvertCustomValueToJsonRadioButton(radioButtonValue);
                            ApplicationInstanceData.Data.CustomPersonFollowUpValues.Add(newCustomValue);
                        }
                        break;
                    case @"select":
                        var selectValue = ((CustomFieldPickerViewModel)CustomFieldControls[i].BindingContext).SelectedBindableOption;
                        if (selectValue != null && !selectValue.Equals(string.Empty))
                        {
                            newCustomValue.Value = Helpers.CustomValueConverter.ConvertCustomValueToJsonSelect(selectValue);
                            ApplicationInstanceData.Data.CustomPersonFollowUpValues.Add(newCustomValue);
                        }
                        break;
                    case @"number":
                        var numberValue = ((CustomFieldDoubleValueViewModel)CustomFieldControls[i].BindingContext).GetDoubleValue();
                        if (numberValue != null)
                        {
                            newCustomValue.Value = Helpers.CustomValueConverter.ConvertCustomValueToJsonNumber((double)numberValue);
                            ApplicationInstanceData.Data.CustomPersonFollowUpValues.Add(newCustomValue);
                        }
                        break;
                    case @"date":
                        var dateValue = ((CustomFieldDateTimeValueViewModel)CustomFieldControls[i].BindingContext).DateValue;
                        if (dateValue != null && !dateValue.ToString().Equals(string.Empty))
                        {
                            newCustomValue.Value = Helpers.CustomValueConverter.ConvertCustomValueToJsonDate((DateTime)dateValue);
                            ApplicationInstanceData.Data.CustomPersonFollowUpValues.Add(newCustomValue);
                        }
                        break;
                    case @"rank_list":
                        var rankedValues = ((CustomFieldRankListViewModel)CustomFieldControls[i].BindingContext).GetRankedValues();
                        if (!rankedValues.Equals(@""))
                        {
                            newCustomValue.Value = Helpers.CustomValueConverter.ConvertCustomValueToJsonRankList(((CustomFieldRankListViewModel)CustomFieldControls[i].BindingContext).Entries.ToList());
                            ApplicationInstanceData.Data.CustomPersonFollowUpValues.Add(newCustomValue);
                        }
                        break;
                    default:
                        break;
                }
            }

            person.AddFollowUp(PersonFollowUp);
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
                    if (!PersonFollowUp.PeopleFollowUpHazardousConditions.Any(a => a.HazardousConditionInternalId == bindableHazardousCondition.Item2.InternalId))
                    {
                        // add value
                        PersonFollowUp.PeopleFollowUpHazardousConditions.Add(new PersonFollowUpHazardousCondition
                        {
                            PersonFollowUp = PersonFollowUp,
                            HazardousCondition = bindableHazardousCondition.Item2
                        });
                    }
                }
                else
                {
                    // false
                    var query = PersonFollowUp.PeopleFollowUpHazardousConditions.Where(a => a.HazardousConditionInternalId == bindableHazardousCondition.Item2.InternalId);
                    if (query.Any())
                    {
                        // delete the value
                        var ids = query.Select(a => a.HazardousConditionInternalId).ToList();
                        foreach (var x in ids)
                        {
                            PersonFollowUp.PeopleFollowUpHazardousConditions.Remove(PersonFollowUp.PeopleFollowUpHazardousConditions.First(a => a.HazardousConditionInternalId == x));
                        }
                    }
                }
            }

            foreach (var bindableWorkActivity in BindableWorkActivities)
            {
                if (bindableWorkActivity.Item3.BoolValue)
                {
                    if (!PersonFollowUp.PeopleFollowUpWorkActivities.Any(a => a.WorkActivityInternalId == bindableWorkActivity.Item2.InternalId))
                    {
                        PersonFollowUp.PeopleFollowUpWorkActivities.Add(new PersonFollowUpWorkActivity
                        {
                            PersonFollowUp = PersonFollowUp,
                            WorkActivity = bindableWorkActivity.Item2
                        });
                    }
                }
                else
                {
                    var query = PersonFollowUp.PeopleFollowUpWorkActivities.Where(a => a.WorkActivityInternalId == bindableWorkActivity.Item2.InternalId);
                    if (query.Any())
                    {
                        var ids = query.Select(a => a.WorkActivityInternalId).ToList();
                        foreach (var x in ids)
                        {
                            PersonFollowUp.PeopleFollowUpWorkActivities.Remove(PersonFollowUp.PeopleFollowUpWorkActivities.First(a => a.WorkActivityInternalId == x));
                        }
                    }
                }
            }

            foreach (var bindableHouseholdTask in BindableHouseholdTasks)
            {
                if (bindableHouseholdTask.Item3.BoolValue)
                {
                    if (!PersonFollowUp.PeopleFollowUpHouseholdTasks.Any(a => a.HouseholdTaskInternalId == bindableHouseholdTask.Item2.InternalId))
                    {
                        PersonFollowUp.PeopleFollowUpHouseholdTasks.Add(new PersonFollowUpHouseholdTask
                        {
                            PersonFollowUp = PersonFollowUp,
                            HouseholdTask = bindableHouseholdTask.Item2
                        });
                    }
                }
                else
                {
                    var query = PersonFollowUp.PeopleFollowUpHouseholdTasks.Where(a => a.HouseholdTaskInternalId == bindableHouseholdTask.Item2.InternalId);
                    if (query.Any())
                    {
                        var ids = query.Select(a => a.HouseholdTaskInternalId).ToList();
                        foreach (var x in ids)
                        {
                            PersonFollowUp.PeopleFollowUpHouseholdTasks.Remove(PersonFollowUp.PeopleFollowUpHouseholdTasks.First(a => a.HouseholdTaskInternalId == x));
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

                var existingValueQuery = ApplicationInstanceData.Data.CustomPersonFollowUpValues
                                                                .Where(a => a.PersonFollowUp.InternalId == PersonFollowUp.InternalId && a.CustomField.InternalId == CustomFields[i].InternalId);

                if (hasValue)
                {
                    // check to see if new needed
                    if (!existingValueQuery.Any())
                    {
                        // add a record
                        var newCustomValue = new CustomPersonFollowUpValue
                        {
                            CreatedAt = now,
                            LastUpdatedAt = now,
                            SoftDeleted = false,
                            CustomField = CustomFields[i],
                            Value = jsonValue,
                            PersonFollowUp = PersonFollowUp
                        };
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
                    if (existingValueQuery.Any()) ApplicationInstanceData.Data.CustomPersonFollowUpValues.Remove(existingValueQuery.First());
                }
            }

            ApplicationInstanceData.Data.SaveChanges();
        }

    }
}
