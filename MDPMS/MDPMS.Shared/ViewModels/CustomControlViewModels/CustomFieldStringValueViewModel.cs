﻿using MDPMS.Shared.ViewModels.Base;

namespace MDPMS.Shared.ViewModels.CustomControlViewModels
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

        public string GetEntryValueWithCrlf()
        {
            if (EntryValue == null) return @"";
            var rtn = @"";
            foreach (var line in EntryValue.Split('\n'))
            {
                rtn += line + @"\r\n";
            }
            return rtn;
        }
    }
}
