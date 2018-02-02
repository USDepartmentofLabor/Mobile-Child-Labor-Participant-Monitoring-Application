using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MDPMS.Database.Data.Database;
using MDPMS.Database.Data.Models.Base;

namespace MDPMS.Database.Data.Models
{
    public class ServiceType : EfBaseModel, ISyncable<ServiceType>, ISyncableAsChild<ServiceType>, ISyncableWithChildren<ServiceType>
    {
        public string Name { get; set; }
        public string Definition { get; set; }

        public List<Service> Services { get; set; } = new List<Service>();

        public void AddService(Service service)
        {
            if (Services == null) Services = new List<Service>();
            service.InternalParentId = InternalId;
            Services.Add(service);
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

        [NotMapped]
        public MDPMSDatabaseContext MdpmsDatabaseContext { get; set; }

        public void SetMdpmsdbContext(MDPMSDatabaseContext context)
        {
            MdpmsDatabaseContext = context;
        }

        public ServiceType GetObjectFromJson(dynamic json)
        {            
            return new ServiceType
            {
                ExternalId = json.id,
                CreatedAt = json.created_at,
                LastUpdatedAt = json.updated_at,
                SoftDeleted = false,
                ExternalParentId = json.service_type_category_id,

                Name = json.name,
                Definition = json.definition
            };
        }

        public string GetJsonFromObject()
        {
            throw new NotImplementedException();
        }

        public bool GetObjectNeedsUpate(ServiceType checkUpdateFrom)
        {
            if (!Name.Equals(checkUpdateFrom.Name)) return true;
            if (!Definition.Equals(checkUpdateFrom.Definition)) return true;
            if (!ExternalParentId.Equals(checkUpdateFrom.ExternalParentId)) return true;
            return false;
        }

        public string GenerateUpdateJsonFromObject(ServiceType updateFrom)
        {
            throw new NotImplementedException();
        }

        public int? GetExternalParentId()
        {
            return ExternalParentId;
        }

        public int? GetInternalParentId()
        {
            return InternalParentId;
        }

        public Tuple<int, ServiceType> GetObjectFromJsonWithParentId(dynamic json, string parentIdPropertyName)
        {
            int id = json.parentIdPropertyName;
            ServiceType serviceType = GetObjectFromJson(json);
            return new Tuple<int, ServiceType>(id, serviceType);
        }

        public void SetExternalParentId(int? id)
        {
            ExternalParentId = id;
        }

        public void SetInternalParentId(int? id)
        {
            InternalParentId = id;
        }

        public void UpdateObject(ServiceType updateFrom)
        {
            LastUpdatedAt = updateFrom.LastUpdatedAt;
            ExternalParentId = updateFrom.ExternalParentId;
            Name = updateFrom.Name;
            Definition = updateFrom.Definition;
        }

        public void SetParentIdsInChildObjects()
        {
            if (Services != null)
            {
                foreach (var service in Services)
                {
                    if (service.GetExternalParentId() == null & ExternalId != null)
                    {
                        service.SetExternalParentId(ExternalId);
                    }
                    service.SetInternalParentId(InternalId);
                }
            }
        }
    }
}
