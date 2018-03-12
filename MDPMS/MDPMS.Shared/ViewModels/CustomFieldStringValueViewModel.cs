using MDPMS.Shared.ViewModels.Base;

namespace MDPMS.Shared.ViewModels
{
    public class CustomFieldStringValueViewModel : ViewModelBase
    {
        public string EntryValue { get; set; }
        public string Name { get; set; }
        public string HelpText { get; set; }

        public CustomFieldStringValueViewModel(string name, string helpText)
        {
            Name = name;
            HelpText = helpText;
        }
    }
}
