using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;

namespace MDPMS.Shared.ViewModels
{
    public class SyncViewModel : ViewModelBase
    {        
        private string _statusMessage { get; set; }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (value == _statusMessage) return;
                _statusMessage = value;
                RaisePropertyChanged(() => StatusMessage);
            }
        }

        public SyncViewModel(ApplicationInstanceData applicationInstanceData)
        {
            ApplicationInstanceData = applicationInstanceData;
        }        
    }
}
