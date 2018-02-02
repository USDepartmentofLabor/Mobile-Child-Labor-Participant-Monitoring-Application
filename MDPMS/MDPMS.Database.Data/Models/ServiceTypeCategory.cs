using System;
using System.Collections.Generic;
using MDPMS.Database.Data.Database;
using MDPMS.Database.Data.Models.Base;

namespace MDPMS.Database.Data.Models
{
    public class ServiceTypeCategory : EfBaseModel, ISyncable<ServiceTypeCategory>, ISyncableWithChildren<ServiceTypeCategory>
    {
        public string Name { get; set; }
        public string Definition { get; set; }

        public List<ServiceType> ServiceTypes { get; set; } = new List<ServiceType>();

        public void AddServiceType(ServiceType serviceType)
        {
            if (ServiceTypes == null) ServiceTypes = new List<ServiceType>();
            serviceType.InternalParentId = InternalId;
            ServiceTypes.Add(serviceType);
        }

        public ServiceTypeCategory GetObjectFromJson(dynamic json)
        {
            return new ServiceTypeCategory
            {
                ExternalId = json.id,
                CreatedAt = null,
                LastUpdatedAt = null,
                SoftDeleted = false,
                Name = json.name,
                Definition = json.definition
            };
        }

        public string GetJsonFromObject()
        {
            throw new NotImplementedException();
        }

        public void UpdateObject(ServiceTypeCategory updateFrom)
        {
            Name = updateFrom.Name;
            Definition = updateFrom.Definition;
        }

        public bool GetObjectNeedsUpate(ServiceTypeCategory checkUpdateFrom)
        {
            if (!Name.Equals(checkUpdateFrom.Name)) return true;
            if (!Definition.Equals(checkUpdateFrom.Definition)) return true;
            return false;
        }

        public string GenerateUpdateJsonFromObject(ServiceTypeCategory updateFrom)
        {
            throw new NotImplementedException();
        }

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
        }

        public void SetParentIdsInChildObjects()
        {            
            if (ServiceTypes != null)
            {
                foreach (var serviceType in ServiceTypes)
                {
                    if (serviceType.GetExternalParentId() == null & ExternalId != null)
                    {
                        serviceType.SetExternalParentId(ExternalId);
                    }
                    serviceType.SetInternalParentId(InternalId);
                }
            }
        }
    }
}
