using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;

namespace MDPMS.Shared.ViewModels.ContentPageModels
{
    public class SyncContentPageModel : ViewModelBase
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

        public SyncContentPageModel(ApplicationInstanceData applicationInstanceData)
        {
            ApplicationInstanceData = applicationInstanceData;
        }        
    }
}
