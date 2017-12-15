using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;

namespace MDPMS.Shared.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        public AboutViewModel(ApplicationInstanceData applicationInstanceData)
        {
            ApplicationInstanceData = applicationInstanceData;
        }
    }
}
