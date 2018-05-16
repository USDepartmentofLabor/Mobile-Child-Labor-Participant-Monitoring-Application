using MDPMS.Shared.ViewModels.CustomControlViewModels;
using Xamarin.Forms;

namespace MDPMS.Shared.Views.CustomControls
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
