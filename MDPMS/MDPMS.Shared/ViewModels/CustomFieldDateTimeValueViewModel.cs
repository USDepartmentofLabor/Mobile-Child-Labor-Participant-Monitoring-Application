using System;
using MDPMS.Shared.ViewModels.Base;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels
{
    public class CustomFieldDateTimeValueViewModel : ViewModelBase
    {
        public Command SetDateTodayCommand { get; set; }
        public Command ClearDateCommand { get; set; }

        public string Name { get; set; }
        public string HelpText { get; set; }
        public DateTime? DateValue { get; set; } = null;

        public string DateValueReadable => GetDateValueReadable();

        private string GetDateValueReadable()
        {
            if (DateValue == null) return @"";
            return ((DateTime)DateValue).ToShortDateString();
        }

        public CustomFieldDateTimeValueViewModel(string name, string helpText)
        {
            Name = name;
            HelpText = helpText;

            SetDateTodayCommand = new Command(ExecuteSetDateTodayCommand);
            ClearDateCommand = new Command(ExecuteClearDateCommand);
        }

        public void SetDateValue(DateTime dateTimeValue)
        {
            DateValue = dateTimeValue;
            OnPropertyChanged(nameof(DateValueReadable));
        }

        private void ExecuteSetDateTodayCommand()
        {
            DateValue = DateTime.Today;
            OnPropertyChanged(nameof(DateValueReadable));
        }

        private void ExecuteClearDateCommand()
        {
            DateValue = null;
            OnPropertyChanged(nameof(DateValueReadable));
        }
    }
}
