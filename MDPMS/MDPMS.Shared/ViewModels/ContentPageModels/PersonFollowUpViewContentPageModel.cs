using System.Linq;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views.ContentPages;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels.ContentPageModels
{
    public class PersonFollowUpViewContentPageModel : ViewModelBase
    {
        public PersonFollowUp PersonFollowUp { get; set; }

        public bool AllowEditDelete { get; set; }
        public GridLength EditDeleteRowHeight { get; set; }

        public Command EditCommand { get; set; }
        public Command DeleteCommand { get; set; }

        public PersonFollowUpViewContentPageModel(ApplicationInstanceData applicationInstanceData, PersonFollowUp personFollowUp)
        {
            ApplicationInstanceData = applicationInstanceData;
            PersonFollowUp = personFollowUp;

            EditCommand = new Command(ExecuteEditCommand);
            DeleteCommand = new Command(ExecuteDeleteCommand);
            AllowEditDelete = !PersonFollowUp.HasExternalId;
            EditDeleteRowHeight = AllowEditDelete ? 80 : 0;
        }

        private void ExecuteEditCommand()
        {
            // is it allowed?
            if (PersonFollowUp.HasExternalId) return;
            ApplicationInstanceData.NavigationPage.PushAsync(new PersonFollowUpEditContentPage { BindingContext = new PersonFollowUpEditContentPageModel(ApplicationInstanceData, PersonFollowUp) });
        }

        private async void ExecuteDeleteCommand()
        {
            // is it allowed?
            if (PersonFollowUp.HasExternalId) return;

            // are you sure?
            var actionDecision = await ApplicationInstanceData.App.MainPage.DisplayAlert(
                ApplicationInstanceData.SelectedLocalization.Translations[@"Confirm"],
                ApplicationInstanceData.SelectedLocalization.Translations[@"ConfirmationMessageDeletePersonFollowUp"],
                ApplicationInstanceData.SelectedLocalization.Translations[@"OK"],
                ApplicationInstanceData.SelectedLocalization.Translations[@"Cancel"]);

            if (actionDecision)
            {
                ApplicationInstanceData.Data.CustomPersonFollowUpValues
                    .RemoveRange(ApplicationInstanceData.Data.CustomPersonFollowUpValues.Where(a => a.PersonFollowUp.InternalId == PersonFollowUp.InternalId));
                ApplicationInstanceData.Data.PersonFollowUps.Remove(PersonFollowUp);
                ApplicationInstanceData.Data.SaveChanges();
                await ApplicationInstanceData.NavigationPage.PopAsync();
            }
        }
    }
}
