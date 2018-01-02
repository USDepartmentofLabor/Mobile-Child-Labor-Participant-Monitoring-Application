using System;
using System.Collections.Generic;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels
{
    public class HouseholdIntakeViewModel : ViewModelBase
    {
        public Command CancelCommand { get; set; }
        public Command SubmitCommand { get; set; }
        public Command AddIncomeSourceCommand { get; set; }

        public List<IncomeSource> IncomeSources { get; set; } = new List<IncomeSource>();

        public string HouseholdName { get; set; }
        public DateTime IntakeDate { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostalCode { get; set; }
        public string DependentLocality { get; set; }
        public string Locality { get; set; }
        public string AdminvArea { get; set; }
        public string DependentAdminvArea { get; set; }    
        public string Country { get; set; }
        public string AddressInfo { get; set; }

        public HouseholdIntakeViewModel(ApplicationInstanceData applicationInstanceData)
        {
            ApplicationInstanceData = applicationInstanceData;
            CancelCommand = new Command(ExecuteCancelCommand);
            SubmitCommand = new Command(ExecuteSubmitCommand);
            AddIncomeSourceCommand = new Command(ExecuteAddIncomeSourceCommand);
        }

        private void ExecuteCancelCommand()
        {
            Exit();
        }

        private void ExecuteSubmitCommand()
        {
            var newHousehold = new Household
            {
                IntakeDate = IntakeDate,
                HouseholdName = HouseholdName,
                AddressLine1 = AddressLine1,
                AddressLine2 = AddressLine2,
                PostalCode = PostalCode,
                DependentLocality = DependentLocality,
                Locality = Locality,
                AdminvArea = AdminvArea,
                DependentAdminvArea = DependentAdminvArea,
                Country = Country,
                AddressInfo = AddressInfo,
                IncomeSources = new List<IncomeSource>()
            };
            foreach (var incomeSource in IncomeSources)
            {
                newHousehold.IncomeSources.Add(incomeSource);
            }
            ApplicationInstanceData.Data.Households.Add(newHousehold);            
            ApplicationInstanceData.Data.SaveChanges();
            Exit();
        }

        private void Exit()
        {
            ApplicationInstanceData.GoToView(new HouseholdsView { BindingContext = new HouseholdsViewModel(ApplicationInstanceData) });
        }

        private void ExecuteAddIncomeSourceCommand()
        {
            // Modal navigate to add income source and retain household before submit
            ApplicationInstanceData.NavigationPage.PushAsync(new IncomeSourceAddView
            {
                BindingContext = new IncomeSourceAddViewModel(ApplicationInstanceData)
            });            
        }
    }
}
