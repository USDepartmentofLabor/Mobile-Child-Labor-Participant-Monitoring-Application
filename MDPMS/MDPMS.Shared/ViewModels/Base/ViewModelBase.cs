using MDPMS.Shared.Models;

namespace MDPMS.Shared.ViewModels.Base
{
    public class ViewModelBase : ExtendedBindableObject
    {
        private bool _isBusy { get; set; }

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                if (value != _isBusy)
                {
                    _isBusy = value;
                    RaisePropertyChanged(() => IsBusy);
                }
            }
        }

        private ApplicationInstanceData _applicationInstanceData;

        public ApplicationInstanceData ApplicationInstanceData
        {
            get
            {
                return _applicationInstanceData;
            }
            set
            {
                _applicationInstanceData = value;
                RaisePropertyChanged(() => ApplicationInstanceData);
            }
        }

        public void NotifyPropertyChange(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }
    }
}
