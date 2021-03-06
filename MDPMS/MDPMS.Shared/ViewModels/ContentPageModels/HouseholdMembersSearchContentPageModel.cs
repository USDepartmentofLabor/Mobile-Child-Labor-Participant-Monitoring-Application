﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views.ContentPages;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels.ContentPageModels
{
    public class HouseholdMembersSearchContentPageModel : ViewModelBase
    {
        public string HouseholdMemberNoun { get; set; }
        public string SearchText { get; set; } = @"";
        public ObservableCollection<HouseholdMemberSearchResultCellModel> HouseholdMembers { get; set; }

        private HouseholdMemberSearchResultCellModel _selectedHouseholdMember;
        public HouseholdMemberSearchResultCellModel SelectedHouseholdMember
        {
            get => _selectedHouseholdMember;
            set
            {
                _selectedHouseholdMember = value;
                if (_selectedHouseholdMember == null) return;
                // open another view
                ApplicationInstanceData.NavigationPage.PushAsync(new PersonViewContentPage
                {
                    BindingContext = new PersonViewContentPageModel(ApplicationInstanceData, value.Person)
                });
            }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public Command RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    await Task.Run(() => { LoadHouseholdMembers(); });
                    IsRefreshing = false;
                });
            }
        }

        public Command SearchCommand { get; set; }

        public HouseholdMembersSearchContentPageModel(ApplicationInstanceData applicationInstanceData)
        {
            ApplicationInstanceData = applicationInstanceData;
            SearchCommand = new Command(LoadHouseholdMembers);
            LoadHouseholdMembers();
        }

        private void LoadHouseholdMembers()
        {            
            // Search people youths aged 5 - 17 based on current age and age at time of intake
            // Search on { HH_id, HH_name, HHM_id, HHM_name }            
            var today = DateTime.Today;            
            var youthsAsOfToday =
                ApplicationInstanceData.Data.People.Include(a => a.Gender).Join(ApplicationInstanceData.Data.Households,
                    ppl => ppl.InternalParentId,
                    hh => hh.InternalId,
                    (ppl, hh) => new { Person = ppl, Household = hh,
                        PersonId = ppl.HasExternalId ? ppl.GetExternalId().ToString() : @"",
                        HouseholdId = hh != null ? (hh.HasExternalId ? hh.GetExternalId().ToString() : @"") : @"" }).ToList()
                .Where(a =>
                    (a.Person.DateOfBirth >= new DateTime(today.Year - 17, today.Month, today.Day)
                    & a.Person.DateOfBirth <= new DateTime(today.Year - 5, today.Month, today.Day)) |
                    (a.Person.IntakeDate != null &
                    a.Person.DateOfBirth >= new DateTime(((DateTime)a.Person.IntakeDate).Year - 17, ((DateTime)a.Person.IntakeDate).Month, ((DateTime)a.Person.IntakeDate).Day) &
                    a.Person.DateOfBirth <= new DateTime(((DateTime)a.Person.IntakeDate).Year - 5, ((DateTime)a.Person.IntakeDate).Month, ((DateTime)a.Person.IntakeDate).Day)));            
            HouseholdMembers = new ObservableCollection<HouseholdMemberSearchResultCellModel>();
            var query = SearchText.Equals(string.Empty)
                ? youthsAsOfToday
                : youthsAsOfToday
                    .Where(a => a.Person.LastName.ToUpper().Contains(SearchText.ToUpper()) |
                                a.Person.FirstName.ToUpper().Contains(SearchText.ToUpper()) |
                                a.Person.MiddleName.ToUpper().Contains(SearchText.ToUpper()) |
                                a.Household.HouseholdName.ToUpper().Contains(SearchText.ToUpper()) |
                                a.PersonId.Contains(SearchText) |
                                (a.Household != null && a.HouseholdId.Contains(SearchText)));            
            foreach (var person in query.OrderBy(a => a.Person.LastName)) HouseholdMembers.Add(new HouseholdMemberSearchResultCellModel(person.Person, person.Household));            
            OnPropertyChanged(nameof(HouseholdMembers));
            OnPropertyChanged(nameof(SelectedHouseholdMember));

            HouseholdMemberNoun = HouseholdMembers.Count().Equals(1) ?
                ApplicationInstanceData.SelectedLocalization.Translations[@"Participant"] :
                ApplicationInstanceData.SelectedLocalization.Translations[@"Participants"];
        }

        public void ExecuteAppearingCommand()
        {
            RefreshCommand.Execute(null);
        }
    }

    public class HouseholdMemberSearchResultCellModel
    {
        public Person Person { get; set; }
        public Household Household { get; set; }
        
        public string HouseholdMemberId { get; set; }
        public string HouseholdMemberFirstName { get; set; }
        public string HouseholdMemberLastName { get; set; }
        public string HouseholdMemberAge { get; set; }
        public string HouseholdId { get; set; }
        public string HouseholdName { get; set; }
        public string HouseholdMemberGender { get; set; }
        
        public HouseholdMemberSearchResultCellModel(Person person, Household household)
        {
            Person = person;
            Household = household;
            HouseholdMemberId = person.HasExternalId ? HouseholdMemberId = person.GetExternalId().ToString() : @"";
            HouseholdMemberFirstName = person.FirstName;
            HouseholdMemberLastName = person.LastName;
            if (person.DateOfBirth != null) HouseholdMemberAge = (DateTime.UtcNow.Year - ((DateTime)person.DateOfBirth).Year).ToString();
            HouseholdId = @"";
            if (Household.HasExternalId) HouseholdId = Household.GetExternalId().ToString();
            HouseholdName = Household.HouseholdName;            
            if (person.Gender != null) HouseholdMemberGender = person.Gender.GenderReadable;                       
        }
    }
}
