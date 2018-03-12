using MDPMS.Shared.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MDPMS.Shared.Views
{
    public partial class CustomFieldRankListView : ContentView
    {
        public CustomFieldRankListView()
        {
            InitializeComponent();
        }

        public void OnAppearing()
        {
            var vm = (CustomFieldRankListViewModel)BindingContext;
            ContentArea.Children.Clear();
            for (var i = 0; i < vm.Entries.Count; i++)
            {
                ContentArea.Children.Add(new GenericRankListEntryView
                {
                    BindingContext = new GenericRankListEntryViewModel(vm)
                    {
                        Entry = vm.Entries[i]
                    }
                });
            }
        }
    }
}
