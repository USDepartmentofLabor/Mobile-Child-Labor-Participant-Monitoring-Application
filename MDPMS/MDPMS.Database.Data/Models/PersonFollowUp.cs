using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using MDPMS.Database.Data.Database;
using MDPMS.Database.Data.Models.Base;
using Newtonsoft.Json;

namespace MDPMS.Database.Data.Models
{
    /// <summary>
    /// Follow up record for person
    /// </summary>
    public class PersonFollowUp : EfBaseModel, ISyncableAsChild<PersonFollowUp>
    {
        /// <summary>
        /// Follow up date, ror_name: follow_date (REQUIRED)
        /// </summary>
        public DateTime? FollowUpDate { get; set; }

        /// <summary>
        /// Have job returning to, ror_name: have_job_returning_to (OPTIONAL)
        /// </summary>
        public bool? HaveJobReturningTo { get; set; }

        /// <summary>
        /// Hours Worked, ror_name: hours_worked (OPTIONAL)
        /// </summary>
        public int? HoursWorked { get; set; }

        /// <summary>
        /// Hours Worked on housework, ror_name: hours_worked_on_housework (OPTIONAL)
        /// </summary>
        public int? HouseWorkedOnHousework { get; set; }

        /// <summary>
        /// Enrolled in school, ror_name: enrolled_in_school (OPTIONAL)
        /// </summary>
        public bool? EnrolledInSchool { get; set; }

        /// <summary>
        /// GPS position latitude at time of submission on the mobile view (OPTIONAL)
        /// </summary>
        public double? GpsLatitude { get; set; }

        /// <summary>
        /// GPS position longitude at time of submission on the mobile view (OPTIONAL)
        /// </summary>
        public double? GpsLongitude { get; set; }

        /// <summary>
        /// GPS position potential position error radius in meters (OPTIONAL)
        /// </summary>
        public double? GpsPositionAccuracy { get; set; }

        /// <summary>
        /// GPS position altitude in meters relative to sea level (OPTIONAL)
        /// </summary>
        public double? GpsAltitude { get; set; }

        /// <summary>
        /// GPS position potential altitude error range in meters (OPTIONAL)
        /// </summary>
        public double? GpsAltitudeAccuracy { get; set; }

        /// <summary>
        /// GPS position heading in degrees relative to true North (OPTIONAL)
        /// </summary>
        public double? GpsHeading { get; set; }

        /// <summary>
        /// GPS position speed in meters per second (OPTIONAL)
        /// </summary>
        public double? GpsSpeed { get; set; }

        /// <summary>
        /// GPS position date time recorded (OPTIONAL)
        /// </summary>
        public DateTime? GpsPositionTime { get; set; }

        /// <summary>
        /// Person follow up hazardous conditions, ror_name: work_activity_ids
        /// </summary>
        public virtual ICollection<PersonFollowUpHazardousCondition> PeopleFollowUpHazardousConditions { get; set; } = new List<PersonFollowUpHazardousCondition>();

        /// <summary>
        /// Person follow up work activities, ror_name: hazardous_condition_ids
        /// </summary>
        public virtual ICollection<PersonFollowUpWorkActivity> PeopleFollowUpWorkActivities { get; set; } = new List<PersonFollowUpWorkActivity>();

        /// <summary>
        /// Person follow up household tasks, ror_name: household_task_ids
        /// </summary>
        public virtual ICollection<PersonFollowUpHouseholdTask> PeopleFollowUpHouseholdTasks { get; set; } = new List<PersonFollowUpHouseholdTask>();

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
        
        public PersonFollowUp GetObjectFromJson(dynamic json)
        {
            var personFollowUp = new PersonFollowUp
            {
                ExternalId = json.id,
                CreatedAt = json.created_at,
                LastUpdatedAt = json.updated_at,
                SoftDeleted = false,
                ExternalParentId = json.person_id,
                FollowUpDate = json.follow_date ?? null,
                HaveJobReturningTo = json.have_job_returning_to ?? null,
                HoursWorked = json.hours_worked ?? null,
                HouseWorkedOnHousework = json.hours_worked_on_housework ?? null,
                EnrolledInSchool = json.enrolled_in_school ?? null,
                GpsLatitude = json.latitude ?? null,
                GpsLongitude = json.longitude ?? null,
                GpsPositionAccuracy = json.position_accuracy ?? null,
                GpsAltitude = json.altitude ?? null,
                GpsAltitudeAccuracy = json.altitude_accuracy ?? null,
                GpsHeading = json.heading ?? null,
                GpsSpeed = json.speed ?? null,
                GpsPositionTime = json.gps_recorded_at ?? null,
                PeopleFollowUpHazardousConditions = new List<PersonFollowUpHazardousCondition>(),
                PeopleFollowUpWorkActivities = new List<PersonFollowUpWorkActivity>(),
                PeopleFollowUpHouseholdTasks = new List<PersonFollowUpHouseholdTask>()
            };

            var hazardousConditionIds = json.hazardous_condition_ids.ToObject<List<int>>();
            foreach (var hazardousConditionId in hazardousConditionIds)
            {
                var searchResult = MdpmsDatabaseContext.FindStatusCustomizationHazardousCondition(hazardousConditionId);
                if (searchResult == null) throw new Exception(@"Hazardous Condition Search Error");
                personFollowUp.PeopleFollowUpHazardousConditions.Add(new PersonFollowUpHazardousCondition
                {
                    PersonFollowUp = this,
                    HazardousCondition = searchResult
                });
            }

            var workActivities = json.work_activity_ids.ToObject<List<int>>();
            foreach (var workActivity in workActivities)
            {
                var searchResult = MdpmsDatabaseContext.FindStatusCustomizationWorkActivity(workActivity);
                if (searchResult == null) throw new Exception(@"Work Activity Search Error");
                personFollowUp.PeopleFollowUpWorkActivities.Add(new PersonFollowUpWorkActivity
                {
                    PersonFollowUp = this,
                    WorkActivity = searchResult
                });
            }

            var householdTasks = json.household_task_ids.ToObject<List<int>>();
            foreach (var householdTask in householdTasks)
            {
                var searchResult = MdpmsDatabaseContext.FindStatusCustomizationHouseholdTask(householdTask);
                if (searchResult == null) throw new Exception(@"Household Task Search Error");
                personFollowUp.PeopleFollowUpHouseholdTasks.Add(new PersonFollowUpHouseholdTask
                {
                    PersonFollowUp = this,
                    HouseholdTask = searchResult
                });
            }

            return personFollowUp;
        }

        public Tuple<int, PersonFollowUp> GetObjectFromJsonWithParentId(dynamic json, string parentIdPropertyName)
        {
            int id = json.parentIdPropertyName;
            PersonFollowUp personFollowUp = GetObjectFromJson(json);
            return new Tuple<int, PersonFollowUp>(id, personFollowUp);
        }

        public string GetJsonFromObject()
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.None;
                writer.WriteStartObject();
                writer.WritePropertyName(@"follow_up");
                writer.WriteStartObject();

                if (FollowUpDate != null)
                {
                    var followDate = (DateTime)FollowUpDate;
                    writer.WritePropertyName("follow_date");
                    writer.WriteValue(followDate.ToString("yyyy-MM-dd"));
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

                writer.WritePropertyName("person_id");
                writer.WriteValue(ExternalParentId);
                writer.WritePropertyName("hazardous_condition_ids");
                writer.WriteRawValue(GetStatusArrayAsJsonString(PeopleFollowUpHazardousConditions.Select(a => a.HazardousCondition)));
                writer.WritePropertyName("work_activity_ids");
                writer.WriteRawValue(GetStatusArrayAsJsonString(PeopleFollowUpWorkActivities.Select(a => a.WorkActivity)));
                writer.WritePropertyName("household_task_ids");
                writer.WriteRawValue(GetStatusArrayAsJsonString(PeopleFollowUpHouseholdTasks.Select(a => a.HouseholdTask)));
                writer.WriteEndObject();
                writer.WriteEndObject();
            }
            return sw.ToString();
        }

        private string GetStatusArrayAsJsonString<T>(IEnumerable<T> data) where T : ISyncable<T>
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
        
        public void UpdateObject(PersonFollowUp updateFrom)
        {
            LastUpdatedAt = updateFrom.LastUpdatedAt;
            ExternalParentId = updateFrom.ExternalParentId;
            FollowUpDate = updateFrom.FollowUpDate;
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
            PeopleFollowUpHazardousConditions = updateFrom.PeopleFollowUpHazardousConditions;
            PeopleFollowUpWorkActivities = updateFrom.PeopleFollowUpWorkActivities;
            PeopleFollowUpHouseholdTasks = updateFrom.PeopleFollowUpHouseholdTasks;
        }

        public bool GetObjectNeedsUpate(PersonFollowUp checkUpdateFrom)
        {
            if (!FollowUpDate.Equals(checkUpdateFrom.FollowUpDate)) return true;
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
            if (!PeopleFollowUpHazardousConditions.Select(a => a.HazardousCondition).SequenceEqual(checkUpdateFrom.PeopleFollowUpHazardousConditions.Select(a => a.HazardousCondition))) return true;
            if (!PeopleFollowUpWorkActivities.Select(a => a.WorkActivity).SequenceEqual(checkUpdateFrom.PeopleFollowUpWorkActivities.Select(a => a.WorkActivity))) return true;
            if (!PeopleFollowUpHouseholdTasks.Select(a => a.HouseholdTask).SequenceEqual(checkUpdateFrom.PeopleFollowUpHouseholdTasks.Select(a => a.HouseholdTask))) return true;
            if (!ExternalParentId.Equals(checkUpdateFrom.ExternalParentId)) return true;
            return false;
        }
        
        public string GenerateUpdateJsonFromObject(PersonFollowUp updateFrom)
        {
            // form the json (determine the fields that need to be updated)
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            var writer = new JsonTextWriter(sw) { Formatting = Formatting.None };
            writer.WriteStartObject();
            writer.WritePropertyName(@"follow_up");
            writer.WriteStartObject();

            if (!FollowUpDate.Equals(updateFrom.FollowUpDate))
            {
                writer.WritePropertyName("follow_date");
                writer.WriteValue(updateFrom.FollowUpDate);
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

            if (!PeopleFollowUpHazardousConditions.Select(a => a.HazardousCondition).SequenceEqual(updateFrom.PeopleFollowUpHazardousConditions.Select(a => a.HazardousCondition)))
            {
                writer.WritePropertyName("hazardous_condition_ids");
                writer.WriteRawValue(GetStatusArrayAsJsonString(PeopleFollowUpHazardousConditions.Select(a => a.HazardousCondition)));
            }

            if (!PeopleFollowUpWorkActivities.Select(a => a.WorkActivity).SequenceEqual(updateFrom.PeopleFollowUpWorkActivities.Select(a => a.WorkActivity)))
            {
                writer.WritePropertyName("work_activity_ids");
                writer.WriteRawValue(GetStatusArrayAsJsonString(PeopleFollowUpWorkActivities.Select(a => a.WorkActivity)));
            }

            if (!PeopleFollowUpHouseholdTasks.Select(a => a.HouseholdTask).SequenceEqual(updateFrom.PeopleFollowUpHouseholdTasks.Select(a => a.HouseholdTask)))
            {
                writer.WritePropertyName("household_task_ids");
                writer.WriteRawValue(GetStatusArrayAsJsonString(PeopleFollowUpHouseholdTasks.Select(a => a.HouseholdTask)));
            }

            if (!ExternalParentId.Equals(updateFrom.ExternalParentId))
            {
                writer.WritePropertyName("person_id");
                writer.WriteValue(updateFrom.ExternalParentId);
            }

            writer.WriteEndObject();
            writer.WriteEndObject();
            return sw.ToString();
        }        
    }
}
