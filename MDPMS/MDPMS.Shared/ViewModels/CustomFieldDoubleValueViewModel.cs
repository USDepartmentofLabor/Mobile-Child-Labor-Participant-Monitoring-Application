using MDPMS.Shared.ViewModels.Base;

namespace MDPMS.Shared.ViewModels
{
    public class CustomFieldDoubleValueViewModel : ViewModelBase
    {
        public string EntryValue { get; set; }
        public string Name { get; set; }
        public string HelpText { get; set; }

        public CustomFieldDoubleValueViewModel(string name, string helpText)
        {
            Name = name;
            HelpText = helpText;
        }

        public double? GetDoubleValue()
        {
            if (EntryValue == null || EntryValue.Equals(@"")) return null;
            double rtnValue = 0.0;
            if (double.TryParse(EntryValue, out rtnValue)) return rtnValue;
            return null;
        }
    }
}
