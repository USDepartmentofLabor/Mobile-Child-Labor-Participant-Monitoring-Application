using MDPMS.Shared.ViewModels;
using Xamarin.Forms;

namespace MDPMS.Shared.Views
{
    public partial class CustomFieldDateTimeView : ContentView
    {
        public CustomFieldDateTimeView()
        {
            InitializeComponent();
        }

        void DateSelectedEventHandler(object sender, DateChangedEventArgs e)
        {
            var vm = (CustomFieldDateTimeValueViewModel)BindingContext;
            vm.SetDateValue(DatePickerControl.Date);
        }

        void DateValueButtonClicked(object sender, System.EventArgs e)
        {
            DatePickerControl.Focus();
        }
    }
}
