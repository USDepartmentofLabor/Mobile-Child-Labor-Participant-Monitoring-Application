using System;
using System.Collections.ObjectModel;
using System.Text;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views.CustomControls;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels.CustomControlViewModels
{
    public class CustomFieldRankListViewModel : ViewModelBase
    {
        public Command AddAnswerCommand { get; set; }
        public Command ClearAnswerCommand { get; set; }

        public string Name { get; set; }
        public string HelpText { get; set; }

        // Tuple<OriginalOrdering, DisplayValue>
        public ObservableCollection<Tuple<int, string>> Entries { get; set; } = new ObservableCollection<Tuple<int, string>>();

        public CustomFieldRankListView View { get; set; }

        public CustomFieldRankListViewModel(ApplicationInstanceData applicationInstanceData, string name, string helpText)
        {
            ApplicationInstanceData = applicationInstanceData;

            Name = name;
            HelpText = helpText;

            AddAnswerCommand = new Command(ExecuteAddAnswerCommand);
            ClearAnswerCommand = new Command(ExecuteClearAnswerCommand);
        }

        public string GetRankedValues()
        {
            if (!View.IsParticipating) return @"";

            var rtn = new StringBuilder();
            for (var i = 0; i < Entries.Count; i++)
            {
                if (i == (Entries.Count - 1)) rtn.Append(Entries[i].Item2);
                else rtn.AppendLine(Entries[i].Item2);
            }
            return rtn.ToString();
        }

        private void ExecuteAddAnswerCommand()
        {
            View.IsParticipating = true;
            View.Refresh();
        }

        private void ExecuteClearAnswerCommand()
        {
            View.IsParticipating = false;
            View.Refresh();
        }
    }
}
