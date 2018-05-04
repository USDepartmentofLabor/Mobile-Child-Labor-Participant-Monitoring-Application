using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;

namespace MDPMS.Shared.ViewModels.ContentViewModels
{
    public class ServiceInstanceViewContentViewModel : ViewModelBase
    {
        public ServiceInstance ServiceInstance { get; set; }

        public ServiceInstanceViewContentViewModel(ApplicationInstanceData applicationInstanceData, ServiceInstance serviceInstance)
        {
            ApplicationInstanceData = applicationInstanceData;
            ServiceInstance = serviceInstance;
        }
    }
}
