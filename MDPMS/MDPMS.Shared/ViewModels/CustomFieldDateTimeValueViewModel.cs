using System;
using MDPMS.Shared.ViewModels.Base;

namespace MDPMS.Shared.ViewModels
{
    public class CustomFieldDateTimeValueViewModel : ViewModelBase
    {
        public DateTime EntryValue { get; set; } = DateTime.Now;
        public string Name { get; set; }
        public string HelpText { get; set; }

        public CustomFieldDateTimeValueViewModel(string name, string helpText)
        {
            Name = name;
            HelpText = helpText;
        }
    }
}
