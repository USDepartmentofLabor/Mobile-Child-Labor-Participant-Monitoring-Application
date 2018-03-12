using System;
using System.Collections.Generic;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;

namespace MDPMS.Shared.ViewModels
{
    public class CustomFieldSwitchArrayViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string HelpText { get; set; }

        // List<Tuple<BoolValue, DisplayText>>
        public List<Tuple<bool, string>> Content { get; set; }

        public CustomFieldSwitchArrayViewModel(string name, string helpText)
        {
            Name = name;
            HelpText = helpText;
        }
    }
}
