﻿using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.ViewModels.ContentPageModels;
using MDPMS.Shared.Views.ContentPages;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels.ContentViewModels
{
    public class HouseholdMembersViewContentViewModel : ViewModelBase
    {
        public Command AddHouseholdMemberCommand { get; set; }

        public Household Household { get; set; }

        private Person _selectedHouseholdMember;
        public Person SelectedHouseholdMember
        {
            get => _selectedHouseholdMember;
            set
            {
                _selectedHouseholdMember = value;
                if (_selectedHouseholdMember  == null) return;
                ApplicationInstanceData.NavigationPage.PushAsync(new PersonViewContentPage
                {
                    BindingContext = new PersonViewContentPageModel(ApplicationInstanceData, value)
                });
            }
        }

        public HouseholdMembersViewContentViewModel(ApplicationInstanceData applicationInstanceData, Household household)
        {
            ApplicationInstanceData = applicationInstanceData;
            Household = household;
            AddHouseholdMemberCommand = new Command(ExecuteAddHouseholdMemberCommand);
        }

        private void ExecuteAddHouseholdMemberCommand()
        {
            ApplicationInstanceData.NavigationPage.PushAsync(new PersonEditContentPage { BindingContext = new PersonEditContentPageModel(ApplicationInstanceData, Household) });
        }
    }
}
