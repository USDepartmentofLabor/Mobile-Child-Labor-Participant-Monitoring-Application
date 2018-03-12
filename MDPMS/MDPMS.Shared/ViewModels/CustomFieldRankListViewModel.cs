using System;
using System.Collections.ObjectModel;
using System.Text;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views;

namespace MDPMS.Shared.ViewModels
{
    public class CustomFieldRankListViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string HelpText { get; set; }

        // Tuple<OriginalOrdering, DisplayValue>
        public ObservableCollection<Tuple<int, string>> Entries { get; set; } = new ObservableCollection<Tuple<int, string>>();

        public CustomFieldRankListView View { get; set; }

        public CustomFieldRankListViewModel(string name, string helpText)
        {
            Name = name;
            HelpText = helpText;
        }

        public string GetRankedValues()
        {
            var rtn = new StringBuilder();
            for (var i = 0; i < Entries.Count; i++)
            {
                if (i == (Entries.Count - 1)) rtn.Append(Entries[i].Item2);
                else rtn.AppendLine(Entries[i].Item2);
            }
            return rtn.ToString();
        }
    }
}
