using MDPMS.Shared.ViewModels.CustomControlViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MDPMS.Shared.Views.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
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
