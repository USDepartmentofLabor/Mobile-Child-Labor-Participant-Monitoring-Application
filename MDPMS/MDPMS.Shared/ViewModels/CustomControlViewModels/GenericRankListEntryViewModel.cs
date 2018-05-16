using System;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels.CustomControlViewModels
{
    public class GenericRankListEntryViewModel
    {
        public Command MoveEntryUpCommand { get; set; }
        public Command MoveEntryDownCommand { get; set; }

        public CustomFieldRankListViewModel ParentViewModel { get; set; }
        public Tuple<int, string> Entry { get; set; }

        public GenericRankListEntryViewModel(CustomFieldRankListViewModel parentViewModel)
        {
            ParentViewModel = parentViewModel;
            MoveEntryUpCommand = new Command(ExecuteMoveEntryUpCommand);
            MoveEntryDownCommand = new Command(ExecuteMoveEntryDownCommand);
        }

        private void ExecuteMoveEntryUpCommand()
        {            
            var indexOfObject = ParentViewModel.Entries.IndexOf(Entry);
            if (indexOfObject > 0)
            {
                ParentViewModel.Entries.Move(indexOfObject, indexOfObject - 1);
            }
            ParentViewModel.View.Refresh();
        }

        private void ExecuteMoveEntryDownCommand()
        {            
            var indexOfObject = ParentViewModel.Entries.IndexOf(Entry);
            if (indexOfObject < ParentViewModel.Entries.Count - 1)
            {
                ParentViewModel.Entries.Move(indexOfObject, indexOfObject + 1);
            }
            ParentViewModel.View.Refresh();
        }
    }
}
