using MDPMS.Shared.ViewModels.CustomControlViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MDPMS.Shared.Views.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomFieldRankListView : ContentView
    {
        Button AddAnswerButton;
        Button ClearAnswerButton;

        public bool IsParticipating;

        public CustomFieldRankListView()
        {
            InitializeComponent();
        }

        public void ShowNonParticipatingViewContent()
        {
            var vm = (CustomFieldRankListViewModel)BindingContext;
            ContentArea.Children.Clear();
            ContentArea.Children.Add(AddAnswerButton);
        }

        public void ShowParticipatingViewContent()
        {
            var vm = (CustomFieldRankListViewModel)BindingContext;
            ContentArea.Children.Clear();
            ContentArea.Children.Add(ClearAnswerButton);
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

        public void OnAppearing()
        {
            IsParticipating = false;

            var vm = (CustomFieldRankListViewModel)BindingContext;

            AddAnswerButton = new Button
            {
                Text = vm.ApplicationInstanceData.SelectedLocalization.Translations[@"AddAnswer"],
                HeightRequest = 100,
                Command = vm.AddAnswerCommand
            };

            ClearAnswerButton = new Button
            {
                Text = vm.ApplicationInstanceData.SelectedLocalization.Translations[@"RemoveAnswer"],
                HeightRequest = 100,
                Command = vm.ClearAnswerCommand
            };

            Refresh();
        }

        public void Refresh()
        {
            if (IsParticipating) ShowParticipatingViewContent();
            else ShowNonParticipatingViewContent();
        }
    }
}
