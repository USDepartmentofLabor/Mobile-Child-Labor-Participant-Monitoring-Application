using MDPMS.Shared.ViewModels.Base;

namespace MDPMS.Shared.ViewModels
{
    public class CustomFieldDoubleValueViewModel : ViewModelBase
    {
        public double EntryValue { get; set; }
        public string Name { get; set; }
        public string HelpText { get; set; }

        public CustomFieldDoubleValueViewModel(string name, string helpText)
        {
            Name = name;
            HelpText = helpText;
        }
    }
}
