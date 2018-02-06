using System;
using MDPMS.Database.Data.Database;
using MDPMS.Database.Data.Models.Base;

namespace MDPMS.Database.Data.Models
{
    public class Service : EfBaseModel, ISyncableAsChild<Service>
    {
        // service_type_id

        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }

        public int? InternalParentId { get; set; } = null;

        public int? ExternalParentId { get; set; } = null;

        public string GenerateUpdateJsonFromObject(Service updateFrom)
        {
            throw new NotImplementedException();
        }

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

        public string GetJsonFromObject()
        {
            throw new NotImplementedException();
        }

        public DateTime? GetLastUpdatedAt()
        {
            return LastUpdatedAt;
        }

        public Service GetObjectFromJson(dynamic json)
        {
            return new Service
            {
                ExternalId = json.id,
                CreatedAt = json.created_at,
                LastUpdatedAt = json.updated_at,
                SoftDeleted = false,
                ExternalParentId = json.service_type_id,

                Name = json.name,
                StartDate = json.start_date,
                EndDate = json.end_date,
                Description = json.description
            };
        }

        public Tuple<int, Service> GetObjectFromJsonWithParentId(dynamic json, string parentIdPropertyName)
        {
            {
                int id = json.parentIdPropertyName;
                Service service = GetObjectFromJson(json);
                return new Tuple<int, Service>(id, service);
            }
        }

        public bool GetObjectNeedsUpate(Service checkUpdateFrom)
        {
            if (!Name.Equals(checkUpdateFrom.Name)) return true;
            if (!StartDate.Equals(checkUpdateFrom.StartDate)) return true;
            if (!EndDate.Equals(checkUpdateFrom.EndDate)) return true;
            if (!Description.Equals(checkUpdateFrom.Description)) return true;
            if (!ExternalParentId.Equals(checkUpdateFrom.ExternalParentId)) return true;
            return false;
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

        public DateTime? GetCreatedAt()
        {
            return CreatedAt;
        }

        public void SetCreatedAt(DateTime? dateTime)
        {
            CreatedAt = dateTime;
        }

        public void SetMdpmsdbContext(MDPMSDatabaseContext context)
        {            
        }

        public void UpdateObject(Service updateFrom)
        {
            LastUpdatedAt = updateFrom.LastUpdatedAt;
            ExternalParentId = updateFrom.ExternalParentId;
            Name = updateFrom.Name;
            StartDate = updateFrom.StartDate;                                 
            EndDate = updateFrom.EndDate;
            Description = updateFrom.Description;
        }
    }
}
