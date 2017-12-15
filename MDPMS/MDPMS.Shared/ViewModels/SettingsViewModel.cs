using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;

namespace MDPMS.Shared.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel(ApplicationInstanceData applicationInstanceData)
        {
            ApplicationInstanceData = applicationInstanceData;
        }
    }
}
