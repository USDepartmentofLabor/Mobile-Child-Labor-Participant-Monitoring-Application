﻿using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.ViewModels.ContentViewModels;
using MDPMS.Shared.Views.ContentViews;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels.ContentPageModels
{
    public class PersonEditContentPageModel : ViewModelBase
    {
        public string Title { get; set; }
        public string SaveCommandVerb { get; set; }

        public Command CancelCommand { get; set; }
        public Command SaveCommand { get; set; }

        public PersonEditContentView PersonEditContentView { get; set; }
        public PersonEditContentViewModel PersonEditContentViewModel { get; set; }

        public Person Person { get; set; }

        public bool IsCreate { get; set; }
        public Household ParentHousehold { get; set; }

        public PersonEditContentPageModel(ApplicationInstanceData applicationInstanceData, Household parentHousehold)
        {
            IsCreate = true;
            SaveCommandVerb = applicationInstanceData.SelectedLocalization.Translations[@"Submit"];
            Title = applicationInstanceData.SelectedLocalization.Translations[@"AddHouseholdMember"];
            ParentHousehold = parentHousehold;
            Init(applicationInstanceData);
        }

        public PersonEditContentPageModel(ApplicationInstanceData applicationInstanceData, Person person)
        {
            IsCreate = false;
            SaveCommandVerb = applicationInstanceData.SelectedLocalization.Translations[@"Save"];
            Title = applicationInstanceData.SelectedLocalization.Translations[@"EditHouseholdMember"];
            Person = person;
            Init(applicationInstanceData);
        }

        private void Init(ApplicationInstanceData applicationInstanceData)
        {
            ApplicationInstanceData = applicationInstanceData;

            CancelCommand = new Command(ExecuteCancelCommand);
            SaveCommand = new Command(ExecuteSaveCommand);
        }

        private void ExecuteCancelCommand()
        {
            CloseView();
        }

        private async void ExecuteSaveCommand()
        {
            IsBusy = true;
            var validation = PersonEditContentViewModel.ValidatePerson();
            if (validation)
            {
                await PersonEditContentViewModel.SavePerson();
                CloseView();
            }
            IsBusy = false;
        }

        private void CloseView()
        {
            ApplicationInstanceData.NavigationPage.PopAsync();
        }
    }
}
