using System.Linq;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views.ContentPages;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels.ContentPageModels
{
    public class PersonViewContentPageModel : ViewModelBase
    {
        public Person Person { get; set; }

        public bool AllowEditDelete { get; set; }
        public GridLength EditDeleteRowHeight { get; set; }

        public Command EditCommand { get; set; }
        public Command DeleteCommand { get; set; }

        public PersonViewContentPageModel(ApplicationInstanceData applicationInstanceData, Person person)
        {
            ApplicationInstanceData = applicationInstanceData;
            Person = person;
            EditCommand = new Command(ExecuteEditCommand);
            DeleteCommand = new Command(ExecuteDeleteCommand);
            AllowEditDelete = !person.HasExternalId;
            EditDeleteRowHeight = AllowEditDelete ? 80 : 0;
        }

        private void ExecuteEditCommand()
        {
            // is it allowed?
            if (Person.HasExternalId) return;
            ApplicationInstanceData.NavigationPage.PushAsync(new PersonEditContentPage { BindingContext = new PersonEditContentPageModel(ApplicationInstanceData, Person) });
        }

        private async void ExecuteDeleteCommand()
        {
            // is it allowed?
            if (Person.HasExternalId) return;

            // are you sure?
            var actionDecision = await ApplicationInstanceData.App.MainPage.DisplayAlert(
                ApplicationInstanceData.SelectedLocalization.Translations[@"Confirm"],
                ApplicationInstanceData.SelectedLocalization.Translations[@"ConfirmationMessageDeleteHouseholdMember"],
                ApplicationInstanceData.SelectedLocalization.Translations[@"OK"],
                ApplicationInstanceData.SelectedLocalization.Translations[@"Cancel"]);

            if (actionDecision)
            {
                ApplicationInstanceData.Data.DeletePerson(Person.InternalId);
                ApplicationInstanceData.Data.SaveChanges();
                await ApplicationInstanceData.NavigationPage.PopAsync();
            }
        }
    }
}
