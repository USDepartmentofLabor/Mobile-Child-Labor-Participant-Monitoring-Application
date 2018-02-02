using MDPMS.Database.Data.Models.Base;
using System;
using MDPMS.Database.Data.Database;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace MDPMS.Database.Data.Models
{
    public class ServiceInstance : EfBaseModel, ISyncableAsChild<ServiceInstance>
    {
        public int Hours { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }
        public Service Service { get; set; }

        public int? InternalParentId { get; set; } = null;

        public int? ExternalParentId { get; set; } = null;

        public int? GetExternalId()
        {
            return ExternalId;
        }

        public int? GetExternalParentId()
        {
            return ExternalParentId;
        }

        public int? GetInternalId()
        {
            return InternalId;
        }

        public int? GetInternalParentId()
        {
            return InternalParentId;
        }

        public DateTime? GetLastUpdatedAt()
        {
            return LastUpdatedAt;
        }

        public void SetExternalId(int? id)
        {
            ExternalId = id;
        }

        public void SetExternalParentId(int? id)
        {
            ExternalParentId = id;
        }

        public void SetInternalParentId(int? id)
        {
            InternalParentId = id;
        }

        public void SetLastUpdatedAt(DateTime? dateTime)
        {
            LastUpdatedAt = dateTime;
        }

        [NotMapped]
        public MDPMSDatabaseContext MdpmsDatabaseContext { get; set; }

        public void SetMdpmsdbContext(MDPMSDatabaseContext context)
        {
            MdpmsDatabaseContext = context;
        }

        public ServiceInstance GetObjectFromJson(dynamic json)
        {
            // External objects
            var selectedService = MdpmsDatabaseContext.FindService((int)json.service_id.Value);
            if (selectedService == null) throw new Exception(@"Service Search Error");

            return new ServiceInstance
            {
                ExternalId = json.id,
                CreatedAt = json.created_at,
                LastUpdatedAt = json.updated_at,
                SoftDeleted = false,
                ExternalParentId = json.person_id,

                Service = selectedService,
                StartDate = json.start_date,
                EndDate = json.end_date,
                Hours = json.hours,
                Notes = json.notes
            };
        }

        public Tuple<int, ServiceInstance> GetObjectFromJsonWithParentId(dynamic json, string parentIdPropertyName)
        {
            int id = json.parentIdPropertyName;
            ServiceInstance serviceInstance = GetObjectFromJson(json);
            return new Tuple<int, ServiceInstance>(id, serviceInstance);
        }

        public string GetJsonFromObject()
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.None;
                writer.WriteStartObject();
                writer.WritePropertyName(@"service_instance");
                writer.WriteStartObject();
                writer.WritePropertyName("start_date");
                writer.WriteValue(StartDate.ToString("yyyy-MM-dd"));
                writer.WritePropertyName("end_date");
                writer.WriteValue(EndDate.ToString("yyyy-MM-dd"));
                writer.WritePropertyName("service_id");
                writer.WriteValue(Service.ExternalId);
                writer.WritePropertyName("person_id");
                writer.WriteValue(ExternalParentId);
                writer.WritePropertyName("hours");
                writer.WriteValue(Hours);
                writer.WritePropertyName("notes");
                writer.WriteValue(Notes);
                writer.WriteEndObject();
                writer.WriteEndObject();
            }
            return sw.ToString();
        }

        public bool GetObjectNeedsUpate(ServiceInstance checkUpdateFrom)
        {
            if (!StartDate.Equals(checkUpdateFrom.StartDate)) return true;
            if (!EndDate.Equals(checkUpdateFrom.EndDate)) return true;
            if (!Hours.Equals(checkUpdateFrom.Hours)) return true;
            if (!Notes.Equals(checkUpdateFrom.Notes)) return true;
            if (!Service.Equals(checkUpdateFrom.Service)) return true;
            if (!ExternalParentId.Equals(checkUpdateFrom.ExternalParentId)) return true;
            return false;
        }

        public void UpdateObject(ServiceInstance updateFrom)
        {
            LastUpdatedAt = updateFrom.LastUpdatedAt;
            ExternalParentId = updateFrom.ExternalParentId;

            StartDate = updateFrom.StartDate;
            EndDate = updateFrom.EndDate;
            Hours = updateFrom.Hours;
            Notes = updateFrom.Notes;
            Service = updateFrom.Service;           
        }

        public string GenerateUpdateJsonFromObject(ServiceInstance updateFrom)
        {
            // form the json (determine the fields that need to be updated)
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            var writer = new JsonTextWriter(sw) { Formatting = Formatting.None };
            writer.WriteStartObject();
            writer.WritePropertyName(@"person");
            writer.WriteStartObject();

            if (!StartDate.Equals(updateFrom.StartDate))
            {
                writer.WritePropertyName("start_date");
                writer.WriteValue(updateFrom.StartDate);
            }

            if (!EndDate.Equals(updateFrom.EndDate))
            {
                writer.WritePropertyName("end_date");
                writer.WriteValue(updateFrom.EndDate);
            }

            if (!Hours.Equals(updateFrom.Hours))
            {
                writer.WritePropertyName("hours");
                writer.WriteValue(updateFrom.Hours);
            }

            if (!Notes.Equals(updateFrom.Notes))
            {
                writer.WritePropertyName("notes");
                writer.WriteValue(updateFrom.Notes);
            }

            if (!Service.Equals(updateFrom.Service))
            {
                writer.WritePropertyName("service_id");
                writer.WriteValue(updateFrom.Service.ExternalId);
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
