using MDPMS.Shared.ViewModels.Base;

namespace MDPMS.Shared.ViewModels
{
    public class GenericSwitchTextViewModel : ViewModelBase
    {
        public bool BoolValue { get; set; }
        public string TextValue { get; set; }

        public GenericSwitchTextViewModel(string textValue)
        {
            TextValue = textValue;
            BoolValue = false;
        }

        public GenericSwitchTextViewModel(string textValue, bool boolValue)
        {
            TextValue = textValue;
            BoolValue = boolValue;
        }
    }
}
