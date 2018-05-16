using System.Collections.ObjectModel;
using MDPMS.Shared.ViewModels.Base;

namespace MDPMS.Shared.ViewModels.CustomControlViewModels
{
    public class CustomFieldPickerViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string HelpText { get; set; }

        public ObservableCollection<string> BindableOptions { get; set; }

        public string SelectedBindableOption { get; set; }

        public CustomFieldPickerViewModel(string name, string helpText)
        {
            Name = name;
            HelpText = helpText;
        }
    }
}
