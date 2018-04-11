using MDPMS.Database.Data.Models.Base;
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
        /// First name (given name) (REQUIRED)
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name (family name) (REQUIRED)
        /// </summary>
        public string LastName { get; set; }
        
        /// <summary>
        /// Middle name (OPTIONAL)
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Gender
        /// </summary>
        // ForeignKey to Gender (REQUIRED)
        [ForeignKey("Genders")]
        public virtual Gender Gender { get; set; }

        /// <summary>
        /// Date of birth, DateTime (REQUIRED)
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Date of birth is approximate date?, true = yes, false = no (OPTIONAL)
        /// </summary>
        public bool? DateOfBirthIsApproximate { get; set; }

        /// <summary>
        /// Relationship to head of household (OPTIONAL)
        /// </summary>      
        [ForeignKey("PersonRelationships")]
        public virtual PersonRelationship RelationshipToHeadOfHousehold { get; set; }

        /// <summary>
        /// Optional, relationship to head of household if other (OPTIONAL)
        /// </summary>
        public string RelationshipIfOther { get; set; }

        /// <summary>
        /// Date of intake  (REQUIRED)
        /// </summary>
        /// <value>The intake date.</value>
        public DateTime? IntakeDate { get; set; }

        /// <summary>
        /// Have jop returning to (OPTIONAL)
        /// </summary>
        /// <value>The have job returning to.</value>
        public bool? HaveJobReturningTo { get; set; }

        /// <summary>
        /// Hours worked (OPTIONAL)
        /// </summary>
        /// <value>The hours worked.</value>
        public int? HoursWorked { get; set; }

        /// <summary>
        /// House worked on housework (OPTIONAL)
        /// </summary>
        /// <value>The house worked on housework.</value>
        public int? HouseWorkedOnHousework { get; set; }

        /// <summary>
        /// Enrolled in school (OPTIONAL)
        /// </summary>
        /// <value>The enrolled in school.</value>
        public bool? EnrolledInSchool { get; set; }

        /// <summary>
        /// GPS position latitude at time of submission on the mobile view
        /// </summary>
        public double? GpsLatitude { get; set; }

        /// <summary>
        /// GPS position longitude at time of submission on the mobile view
        /// </summary>
        public double? GpsLongitude { get; set; }

        /// <summary>
        /// GPS position potential position error radius in meters
        /// </summary>
        public double? GpsPositionAccuracy { get; set; }

        /// <summary>
        /// GPS position altitude in meters relative to sea level
        /// </summary>
        public double? GpsAltitude { get; set; }

        /// <summary>
        /// GPS position potential altitude error range in meters
        /// </summary>
        public double? GpsAltitudeAccuracy { get; set; }

        /// <summary>
        /// GPS position heading in degrees relative to true North
        /// </summary>
        public double? GpsHeading { get; set; }

        /// <summary>
        /// GPS position speed in meters per second
        /// </summary>
        public double? GpsSpeed { get; set; }

        /// <summary>
        /// GPS position date time recorded
        /// </summary>
        public DateTime? GpsPositionTime { get; set; }

        public bool IsBeneficiary()
        {
            // Beneficiary is defined as age range either at the current time or time of intake
            var isBeneficiaryNow = IsInAgeRaneBasedOnDate(DateTime.Now, 5, 17);
            if (IntakeDate != null) return WasBeneficiaryAtTimeOfIntake() | isBeneficiaryNow;
            return isBeneficiaryNow;
        }

        public bool WasBeneficiaryAtTimeOfIntake()
        {
            if (IntakeDate == null) return false;
            return IsInAgeRaneBasedOnDate((DateTime)IntakeDate, 5, 17);
        }

        public bool IsYouthNow(DateTime now)
        {
            if (DateOfBirth == null) return false;
            return DateOfBirth >= new DateTime(now.Year - 17, now.Month, now.Day);
        }

        public bool IsInAgeRaneBasedOnDate(DateTime dateTime, int startAgeRange, int endAgeRange)
        {
            // aged 5 to 17 based on a date
            if (DateOfBirth == null) return false;
            return (DateOfBirth >= new DateTime(dateTime.Year - endAgeRange, dateTime.Month, dateTime.Day)
                    & DateOfBirth <= new DateTime(dateTime.Year - startAgeRange, dateTime.Month, dateTime.Day));
        }

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

        public virtual ICollection<ServiceInstance> ServiceInstances { get; set; } = new List<ServiceInstance>();

        public void AddServiceInstance(ServiceInstance serviceInstance)
        {
            if (ServiceInstances == null) ServiceInstances = new List<ServiceInstance>();
            serviceInstance.InternalParentId = InternalId;
            ServiceInstances.Add(serviceInstance);
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

        public DateTime? GetCreatedAt()
        {
            return CreatedAt;
        }

        public void SetCreatedAt(DateTime? dateTime)
        {
            CreatedAt = dateTime;
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

            var person = new Person
            {
                ExternalId = json.id,
                CreatedAt = json.created_at,
                LastUpdatedAt = json.updated_at,
                SoftDeleted = false,
                ExternalParentId = json.household_id,            
                Gender = selectedGender,
                FirstName = json.first_name ?? @"",
                LastName = json.last_name ?? @"",
                MiddleName = json.middle_name ?? @"",
                DateOfBirth = json.dob ?? null,
                DateOfBirthIsApproximate = json.is_birthdate_approximate ?? null,
                RelationshipIfOther = json.relationship_other ?? null,
                IntakeDate = json.intake_date ?? null,
                HaveJobReturningTo = json.have_job_returning_to ?? false,
                HoursWorked = json.hours_worked ?? null,
                HouseWorkedOnHousework = json.hours_worked_on_housework ?? null,
                EnrolledInSchool = json.enrolled_in_school ?? false,
                GpsLatitude = json.latitude ?? null,
                GpsLongitude = json.longitude ?? null,
                GpsPositionAccuracy = json.position_accuracy ?? null,
                GpsAltitude = json.altitude ?? null,
                GpsAltitudeAccuracy = json.altitude_accuracy ?? null,
                GpsHeading = json.heading ?? null,
                GpsSpeed = json.speed ?? null,
                GpsPositionTime = json.gps_recorded_at ?? null,
                PeopleHazardousConditions = new List<PersonHazardousCondition>(),
                PeopleWorkActivities = new List<PersonWorkActivity>(),
                PeopleHouseholdTasks = new List<PersonHouseholdTask>(),
                PeopleFollowUps = new List<PersonFollowUp>()
            };

            if (json.relationship_id.Value is null)
            {
                person.RelationshipToHeadOfHousehold = null;
            }
            else
            {
                var selectedPersonRelationship = MdpmsDatabaseContext.FindPersonRelationship((int)json.relationship_id.Value);            
                if (selectedPersonRelationship == null) throw new Exception(@"Person Relationship Search Error");
            }

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
                writer.WriteValue(FirstName ?? @"");
                writer.WritePropertyName("last_name");
                writer.WriteValue(LastName ?? @"");

                if (MiddleName != null)
                {
                    writer.WritePropertyName("middle_name");
                    writer.WriteValue(MiddleName);
                }

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

                if (RelationshipToHeadOfHousehold != null)
                {
                    writer.WritePropertyName("relationship_id");
                    writer.WriteValue(RelationshipToHeadOfHousehold.GetExternalId());
                }

                if (RelationshipIfOther != null)
                {
                    writer.WritePropertyName("relationship_other");
                    writer.WriteValue(RelationshipIfOther);
                }

                if (HaveJobReturningTo != null)
                {
                    writer.WritePropertyName("have_job_returning_to");
                    writer.WriteValue(HaveJobReturningTo);
                }

                if (HoursWorked != null)
                {
                    writer.WritePropertyName("hours_worked");                
                    writer.WriteValue(HoursWorked);                
                }

                if (HouseWorkedOnHousework != null)
                {
                    writer.WritePropertyName("hours_worked_on_housework");
                    writer.WriteValue(HouseWorkedOnHousework);
                }

                if (EnrolledInSchool != null)
                {
                    writer.WritePropertyName("enrolled_in_school");
                    writer.WriteValue(EnrolledInSchool);
                }

                if (DateOfBirthIsApproximate != null)
                {
                    writer.WritePropertyName("is_birthdate_approximate");
                    writer.WriteValue(DateOfBirthIsApproximate);   
                }

                writer.WritePropertyName("household_id");
                writer.WriteValue(ExternalParentId);

                if (GpsLatitude != null)
                {
                    writer.WritePropertyName("latitude");
                    writer.WriteValue(GpsLatitude);
                }

                if (GpsLongitude != null)
                {
                    writer.WritePropertyName("longitude");
                    writer.WriteValue(GpsLongitude);
                }

                if (GpsPositionAccuracy != null)
                {
                    writer.WritePropertyName("position_accuracy");
                    writer.WriteValue(GpsPositionAccuracy);
                }

                if (GpsAltitude != null)
                {
                    writer.WritePropertyName("altitude");
                    writer.WriteValue(GpsAltitude);
                }

                if (GpsAltitudeAccuracy != null)
                {
                    writer.WritePropertyName("altitude_accuracy");
                    writer.WriteValue(GpsAltitudeAccuracy);
                }

                if (GpsHeading != null)
                {
                    writer.WritePropertyName("heading");
                    writer.WriteValue(GpsHeading);
                }

                if (GpsSpeed != null)
                {
                    writer.WritePropertyName("speed");
                    writer.WriteValue(GpsSpeed);
                }

                if (GpsPositionTime != null)
                {
                    writer.WritePropertyName("gps_recorded_at");
                    writer.WriteValue(GpsPositionTime);
                }

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
                return @"[]";
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
            GpsLatitude = updateFrom.GpsLatitude;
            GpsLongitude = updateFrom.GpsLongitude;
            GpsPositionAccuracy = updateFrom.GpsPositionAccuracy;
            GpsAltitude = updateFrom.GpsAltitude;
            GpsAltitudeAccuracy = updateFrom.GpsAltitudeAccuracy;
            GpsHeading = updateFrom.GpsHeading;
            GpsSpeed = updateFrom.GpsSpeed;
            GpsPositionTime = updateFrom.GpsPositionTime;
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
            if (!GpsLatitude.Equals(checkUpdateFrom.GpsLatitude)) return true;
            if (!GpsLongitude.Equals(checkUpdateFrom.GpsLongitude)) return true;
            if (!GpsPositionAccuracy.Equals(checkUpdateFrom.GpsPositionAccuracy)) return true;
            if (!GpsAltitude.Equals(checkUpdateFrom.GpsAltitude)) return true;
            if (!GpsAltitudeAccuracy.Equals(checkUpdateFrom.GpsAltitudeAccuracy)) return true;
            if (!GpsHeading.Equals(checkUpdateFrom.GpsHeading)) return true;
            if (!GpsSpeed.Equals(checkUpdateFrom.GpsSpeed)) return true;
            if (!GpsPositionTime.Equals(checkUpdateFrom.GpsPositionTime)) return true;
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

            if (!GpsLatitude.Equals(updateFrom.GpsLatitude))
            {
                writer.WritePropertyName("latitude");
                writer.WriteValue(updateFrom.GpsLatitude);
            }

            if (!GpsLongitude.Equals(updateFrom.GpsLongitude))
            {
                writer.WritePropertyName("longitude");
                writer.WriteValue(updateFrom.GpsLongitude);
            }

            if (!GpsPositionAccuracy.Equals(updateFrom.GpsPositionAccuracy))
            {
                writer.WritePropertyName("position_accuracy");
                writer.WriteValue(updateFrom.GpsPositionAccuracy);
            }

            if (!GpsAltitude.Equals(updateFrom.GpsAltitude))
            {
                writer.WritePropertyName("altitude");
                writer.WriteValue(updateFrom.GpsAltitude);
            }

            if (!GpsAltitudeAccuracy.Equals(updateFrom.GpsAltitudeAccuracy))
            {
                writer.WritePropertyName("altitude_accuracy");
                writer.WriteValue(updateFrom.GpsAltitudeAccuracy);
            }

            if (!GpsHeading.Equals(updateFrom.GpsHeading))
            {
                writer.WritePropertyName("heading");
                writer.WriteValue(updateFrom.GpsHeading);
            }

            if (!GpsSpeed.Equals(updateFrom.GpsSpeed))
            {
                writer.WritePropertyName("speed");
                writer.WriteValue(updateFrom.GpsSpeed);
            }

            if (!GpsPositionTime.Equals(updateFrom.GpsPositionTime))
            {
                writer.WritePropertyName("gps_recorded_at");
                writer.WriteValue(updateFrom.GpsPositionTime);
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
            if (ServiceInstances != null)
            {
                foreach (var serviceInstance in ServiceInstances)
                {
                    if (serviceInstance.GetExternalParentId() == null & ExternalId != null)
                    {
                        serviceInstance.SetExternalParentId(ExternalId);
                    }
                    serviceInstance.SetInternalParentId(InternalId);
                }
            }
        }
    }
}
