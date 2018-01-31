﻿using MDPMS.Database.Data.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using MDPMS.Database.Data.Database;
using Newtonsoft.Json;

namespace MDPMS.Database.Data.Models
{
    /// <summary>
    /// Person, adult or child
    /// </summary>
    public class Person : EfBaseModel, ISyncableAsChild<Person>, ISyncableWithChildren<Person>
    {
        /// <summary>
        /// First name (given name)
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name (family name)
        /// </summary>
        public string LastName { get; set; }
        
        /// <summary>
        /// Middle name
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Gender
        /// </summary>
        // ForeignKey to Gender     
        [ForeignKey("Genders")]
        public virtual Gender Gender { get; set; }

        /// <summary>
        /// Date of birth, DateTime
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Date of birth is approximate date?, true = yes, false = no
        /// </summary>
        public bool DateOfBirthIsApproximate { get; set; }

        /// <summary>
        /// Relationship to head of household
        /// </summary>      
        [ForeignKey("PersonRelationships")]
        public virtual PersonRelationship RelationshipToHeadOfHousehold { get; set; }

        /// <summary>
        /// Optional, relationship to head of household if other
        /// </summary>
        public string RelationshipIfOther { get; set; }

        public DateTime? IntakeDate { get; set; }

        public bool HaveJobReturningTo { get; set; }

        public int HoursWorked { get; set; }

        public int HouseWorkedOnHousework { get; set; }

        public bool EnrolledInSchool { get; set; }

        public virtual ICollection<PersonHazardousCondition> PeopleHazardousConditions { get; set; } = new List<PersonHazardousCondition>();
        public virtual ICollection<PersonWorkActivity> PeopleWorkActivities { get; set; } = new List<PersonWorkActivity>();
        public virtual ICollection<PersonHouseholdTask> PeopleHouseholdTasks { get; set; } = new List<PersonHouseholdTask>();

        public virtual ICollection<PersonFollowUp> PeopleFollowUps { get; set; } = new List<PersonFollowUp>();

        public void AddFollowUp(PersonFollowUp personFollowUp)
        {
            if (PeopleFollowUps == null) PeopleFollowUps = new List<PersonFollowUp>();
            personFollowUp.InternalParentId = InternalId;
            PeopleFollowUps.Add(personFollowUp);
        }

        public int? InternalParentId { get; set; } = null;

        public int? ExternalParentId { get; set; } = null;

        public int? GetExternalId()
        {
            return ExternalId;
        }

        public void SetExternalId(int? id)
        {
            ExternalId = id;
        }

        public DateTime? GetLastUpdatedAt()
        {
            return LastUpdatedAt;
        }

        public void SetLastUpdatedAt(DateTime? dateTime)
        {
            LastUpdatedAt = dateTime;
        }
        
        public int? GetInternalId()
        {
            return InternalId;
        }

        public void SetMdpmsdbContext(MDPMSDatabaseContext context)
        {
            MdpmsDatabaseContext = context;
        }

        public int? GetExternalParentId()
        {
            return ExternalParentId;
        }

        public void SetExternalParentId(int? id)
        {
            ExternalParentId = id;
        }

        public int? GetInternalParentId()
        {
            return InternalParentId;
        }

        public void SetInternalParentId(int? id)
        {
            InternalParentId = id;
        }
        
        [NotMapped]
        public MDPMSDatabaseContext MdpmsDatabaseContext { get; set; }

        public Person GetObjectFromJson(dynamic json)
        {
            // external objects
            var genderJson = (int)json.sex.Value;
            Gender selectedGender = null;
            if (genderJson.Equals(1) | genderJson.Equals(2))
            {
                if (genderJson.Equals(1)) selectedGender = MdpmsDatabaseContext.GetMaleGender();
                if (genderJson.Equals(2)) selectedGender = MdpmsDatabaseContext.GetFemaleGender();
                if (selectedGender == null) throw new Exception(@"Gender Search Error");
            }
            else { throw new Exception(@"Gender Search Error"); }

            var selectedPersonRelationship = MdpmsDatabaseContext.FindPersonRelationship((int)json.relationship_id.Value);
            if (selectedPersonRelationship == null) throw new Exception(@"Person Relationship Search Error");
            
            var person = new Person
            {
                ExternalId = json.id,
                CreatedAt = json.created_at,
                LastUpdatedAt = json.updated_at,
                SoftDeleted = false,
                ExternalParentId = json.household_id,

                FirstName = json.first_name,
                LastName = json.last_name,
                MiddleName = json.middle_name,
                Gender = selectedGender,
                DateOfBirth = json.dob,
                DateOfBirthIsApproximate = json.is_birthdate_approximate,
                RelationshipToHeadOfHousehold = selectedPersonRelationship,
                RelationshipIfOther = json.relationship_other,
                IntakeDate = json.intake_date,
                HaveJobReturningTo = json.have_job_returning_to,
                HoursWorked = json.hours_worked,
                HouseWorkedOnHousework = json.hours_worked_on_housework,
                EnrolledInSchool = json.enrolled_in_school,
                PeopleHazardousConditions = new List<PersonHazardousCondition>(),
                PeopleWorkActivities = new List<PersonWorkActivity>(),
                PeopleHouseholdTasks = new List<PersonHouseholdTask>(),
                PeopleFollowUps = new List<PersonFollowUp>()
            };
            
            var hazardousConditionIds = json.hazardous_condition_ids.ToObject<List<int>>();
            foreach (var hazardousConditionId in hazardousConditionIds)
            {
                var searchResult = MdpmsDatabaseContext.FindStatusCustomizationHazardousCondition(hazardousConditionId);
                if (searchResult == null) throw new Exception(@"Hazardous Condition Search Error");
                person.PeopleHazardousConditions.Add(new PersonHazardousCondition
                {
                    Person = this,
                    HazardousCondition = searchResult
                });
            }

            var workActivities = json.work_activity_ids.ToObject<List<int>>();
            foreach (var workActivity in workActivities)
            {
                var searchResult = MdpmsDatabaseContext.FindStatusCustomizationWorkActivity(workActivity);
                if (searchResult == null) throw new Exception(@"Work Activity Search Error");
                person.PeopleWorkActivities.Add(new PersonWorkActivity
                {
                    Person = this,
                    WorkActivity = searchResult
                });
            }

            var householdTasks = json.household_task_ids.ToObject<List<int>>();
            foreach (var householdTask in householdTasks)
            {
                var searchResult = MdpmsDatabaseContext.FindStatusCustomizationHouseholdTask(householdTask);
                if (searchResult == null) throw new Exception(@"Household Task Search Error");
                person.PeopleHouseholdTasks.Add(new PersonHouseholdTask
                {
                    Person = this,
                    HouseholdTask = searchResult
                });
            }

            return person;
        }
        
        public Tuple<int, Person> GetObjectFromJsonWithParentId(dynamic json, string parentIdPropertyName)
        {
            int id = json.parentIdPropertyName;
            Person person = GetObjectFromJson(json);
            return new Tuple<int, Person>(id, person);
        }
        
        public string GetJsonFromObject()
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.None;
                writer.WriteStartObject();
                writer.WritePropertyName(@"person");
                writer.WriteStartObject();
                writer.WritePropertyName("first_name");
                writer.WriteValue(FirstName);
                writer.WritePropertyName("last_name");
                writer.WriteValue(LastName);
                writer.WritePropertyName("middle_name");
                writer.WriteValue(MiddleName);
                writer.WritePropertyName("sex");
                writer.WriteValue(Gender.DpmsGenderNumber);                
                if (DateOfBirth != null)
                {
                    var dob = (DateTime) DateOfBirth;
                    writer.WritePropertyName("dob");
                    writer.WriteValue(dob.ToString("yyyy-MM-dd"));
                }
                if (IntakeDate != null)
                {
                    var intakeDate = (DateTime)IntakeDate;
                    writer.WritePropertyName("intake_date");
                    writer.WriteValue(intakeDate.ToString("yyyy-MM-dd"));
                }
                writer.WritePropertyName("relationship_id");
                writer.WriteValue(RelationshipToHeadOfHousehold.GetExternalId());
                writer.WritePropertyName("relationship_other");
                writer.WriteValue(RelationshipIfOther);
                writer.WritePropertyName("have_job_returning_to");
                writer.WriteValue(HaveJobReturningTo);
                writer.WritePropertyName("hours_worked");
                writer.WriteValue(HoursWorked);                
                writer.WritePropertyName("hours_worked_on_housework");
                writer.WriteValue(HouseWorkedOnHousework);
                writer.WritePropertyName("enrolled_in_school");
                writer.WriteValue(EnrolledInSchool);
                writer.WritePropertyName("is_birthdate_approximate");
                writer.WriteValue(DateOfBirthIsApproximate);                
                writer.WritePropertyName("household_id");
                writer.WriteValue(ExternalParentId);
                
                writer.WritePropertyName("hazardous_condition_ids");
                writer.WriteRawValue(GetStatusArrayAsJsonString(PeopleHazardousConditions.Select(a => a.HazardousCondition)));

                writer.WritePropertyName("work_activity_ids");
                writer.WriteRawValue(GetStatusArrayAsJsonString(PeopleWorkActivities.Select(a => a.WorkActivity)));
                
                writer.WritePropertyName("household_task_ids");
                writer.WriteRawValue(GetStatusArrayAsJsonString(PeopleHouseholdTasks.Select(a => a.HouseholdTask)));
                
                writer.WriteEndObject();
                writer.WriteEndObject();
            }
            return sw.ToString();
        }

        private string GetStatusArrayAsJsonString<T>(IEnumerable<T> data) where T: ISyncable<T>
        {
            if (!data.Any())
            {
                return @"[" + '"' + '"' + ']';
            }
            var sb = new StringBuilder(@"[");
            foreach (var x in data)
            {
                sb.Append(x.GetExternalId());
                if (!x.Equals(data.Last())) sb.Append(@",");
            }
            sb.Append(@"]");
            return sb.ToString();
        }

        public void UpdateObject(Person updateFrom)
        {
            LastUpdatedAt = updateFrom.LastUpdatedAt;
            ExternalParentId = updateFrom.ExternalParentId;
            LastName = updateFrom.LastName;
            FirstName = updateFrom.FirstName;
            MiddleName = updateFrom.MiddleName;
            Gender = updateFrom.Gender;
            DateOfBirth = updateFrom.DateOfBirth;
            DateOfBirthIsApproximate = updateFrom.DateOfBirthIsApproximate;
            RelationshipToHeadOfHousehold = updateFrom.RelationshipToHeadOfHousehold;
            RelationshipIfOther = updateFrom.RelationshipIfOther;
            IntakeDate = updateFrom.IntakeDate;
            HaveJobReturningTo = updateFrom.HaveJobReturningTo;
            HoursWorked = updateFrom.HoursWorked;
            HouseWorkedOnHousework = updateFrom.HouseWorkedOnHousework;
            EnrolledInSchool = updateFrom.EnrolledInSchool;
            PeopleHazardousConditions = updateFrom.PeopleHazardousConditions;
            PeopleWorkActivities = updateFrom.PeopleWorkActivities;
            PeopleHouseholdTasks = updateFrom.PeopleHouseholdTasks; 
        }

        public bool GetObjectNeedsUpate(Person checkUpdateFrom)
        {
            if (!LastName.Equals(checkUpdateFrom.LastName)) return true;
            if (!FirstName.Equals(checkUpdateFrom.FirstName)) return true;
            if (!MiddleName.Equals(checkUpdateFrom.MiddleName)) return true;
            if (!Gender.Equals(checkUpdateFrom.Gender)) return true;
            if (!DateOfBirth.Equals(checkUpdateFrom.DateOfBirth)) return true;
            if (!DateOfBirthIsApproximate.Equals(checkUpdateFrom.DateOfBirthIsApproximate)) return true;
            if (!RelationshipToHeadOfHousehold.Equals(checkUpdateFrom.RelationshipToHeadOfHousehold)) return true;
            if (!RelationshipIfOther.Equals(checkUpdateFrom.RelationshipIfOther)) return true;
            if (!IntakeDate.Equals(checkUpdateFrom.IntakeDate)) return true;
            if (!HaveJobReturningTo.Equals(checkUpdateFrom.HaveJobReturningTo)) return true;
            if (!HoursWorked.Equals(checkUpdateFrom.HoursWorked)) return true;
            if (!HouseWorkedOnHousework.Equals(checkUpdateFrom.HouseWorkedOnHousework)) return true;
            if (!EnrolledInSchool.Equals(checkUpdateFrom.EnrolledInSchool)) return true;
            if (!PeopleHazardousConditions.Select(a => a.HazardousCondition).SequenceEqual(checkUpdateFrom.PeopleHazardousConditions.Select(a => a.HazardousCondition))) return true;
            if (!PeopleWorkActivities.Select(a => a.WorkActivity).SequenceEqual(checkUpdateFrom.PeopleWorkActivities.Select(a => a.WorkActivity))) return true;
            if (!PeopleHouseholdTasks.Select(a => a.HouseholdTask).SequenceEqual(checkUpdateFrom.PeopleHouseholdTasks.Select(a => a.HouseholdTask))) return true;            
            if (!ExternalParentId.Equals(checkUpdateFrom.ExternalParentId)) return true;
            return false;
        }

        public string GenerateUpdateJsonFromObject(Person updateFrom)
        {
            // form the json (determine the fields that need to be updated)
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            var writer = new JsonTextWriter(sw) { Formatting = Formatting.None };
            writer.WriteStartObject();
            writer.WritePropertyName(@"person");
            writer.WriteStartObject();

            if (!LastName.Equals(updateFrom.LastName))
            {
                writer.WritePropertyName("last_name");
                writer.WriteValue(updateFrom.LastName);
            }

            if (!FirstName.Equals(updateFrom.FirstName))
            {
                writer.WritePropertyName("first_name");
                writer.WriteValue(updateFrom.FirstName);
            }

            if (!MiddleName.Equals(updateFrom.MiddleName))
            {
                writer.WritePropertyName("middle_name");
                writer.WriteValue(updateFrom.MiddleName);
            }

            if (!Gender.Equals(updateFrom.Gender))
            {
                writer.WritePropertyName("sex");
                writer.WriteValue(updateFrom.Gender);
            }

            if (!DateOfBirth.Equals(updateFrom.DateOfBirth))
            {
                writer.WritePropertyName("dob");
                writer.WriteValue(updateFrom.DateOfBirth);
            }

            if (!DateOfBirthIsApproximate.Equals(updateFrom.DateOfBirthIsApproximate))
            {
                writer.WritePropertyName("is_birthdate_approximate");
                writer.WriteValue(updateFrom.DateOfBirthIsApproximate);
            }

            if (!RelationshipToHeadOfHousehold.Equals(updateFrom.RelationshipToHeadOfHousehold))
            {
                writer.WritePropertyName("relationship_id");
                writer.WriteValue(updateFrom.RelationshipToHeadOfHousehold.ExternalId);
            }

            if (!RelationshipIfOther.Equals(updateFrom.RelationshipIfOther))
            {
                writer.WritePropertyName("relationship_other");
                writer.WriteValue(updateFrom.RelationshipIfOther);
            }

            if (!IntakeDate.Equals(updateFrom.IntakeDate))
            {
                writer.WritePropertyName("intake_date");
                writer.WriteValue(updateFrom.IntakeDate);
            }

            if (!HaveJobReturningTo.Equals(updateFrom.HaveJobReturningTo))
            {
                writer.WritePropertyName("have_job_returning_to");
                writer.WriteValue(updateFrom.HaveJobReturningTo);
            }

            if (!HoursWorked.Equals(updateFrom.HoursWorked))
            {
                writer.WritePropertyName("hours_worked");
                writer.WriteValue(updateFrom.HoursWorked);
            }

            if (!HouseWorkedOnHousework.Equals(updateFrom.HouseWorkedOnHousework))
            {
                writer.WritePropertyName("hours_worked_on_housework");
                writer.WriteValue(updateFrom.HouseWorkedOnHousework);
            }

            if (!EnrolledInSchool.Equals(updateFrom.EnrolledInSchool))
            {
                writer.WritePropertyName("enrolled_in_school");
                writer.WriteValue(updateFrom.EnrolledInSchool);
            }

            if (!PeopleHazardousConditions.Select(a => a.HazardousCondition).SequenceEqual(updateFrom.PeopleHazardousConditions.Select(a => a.HazardousCondition)))
            {
                writer.WritePropertyName("hazardous_condition_ids");
                writer.WriteRawValue(GetStatusArrayAsJsonString(PeopleHazardousConditions.Select(a => a.HazardousCondition)));
            }

            if (!PeopleWorkActivities.Select(a => a.WorkActivity).SequenceEqual(updateFrom.PeopleWorkActivities.Select(a => a.WorkActivity)))
            {
                writer.WritePropertyName("work_activity_ids");
                writer.WriteRawValue(GetStatusArrayAsJsonString(PeopleWorkActivities.Select(a => a.WorkActivity)));
            }

            if (!PeopleHouseholdTasks.Select(a => a.HouseholdTask).SequenceEqual(updateFrom.PeopleHouseholdTasks.Select(a => a.HouseholdTask)))
            {
                writer.WritePropertyName("household_task_ids");
                writer.WriteRawValue(GetStatusArrayAsJsonString(PeopleHouseholdTasks.Select(a => a.HouseholdTask)));
            }

            if (!ExternalParentId.Equals(updateFrom.ExternalParentId))
            {
                writer.WritePropertyName("household_id");
                writer.WriteValue(updateFrom.ExternalParentId);
            }

            writer.WriteEndObject();
            writer.WriteEndObject();
            return sw.ToString();
        }

        public void SetParentIdsInChildObjects()
        {
            if (PeopleFollowUps != null)
            {
                foreach (var personFollowUp in PeopleFollowUps)
                {
                    if (personFollowUp.GetExternalParentId() == null & ExternalId != null)
                    {
                        personFollowUp.SetExternalParentId(ExternalId);
                    }
                    personFollowUp.SetInternalParentId(InternalId);
                }
            }
        }
    }
}
