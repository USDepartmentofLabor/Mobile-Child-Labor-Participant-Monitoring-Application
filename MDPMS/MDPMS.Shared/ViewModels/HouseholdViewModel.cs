using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;

namespace MDPMS.Shared.ViewModels
{
    public class HouseholdViewModel : ViewModelBase
    {
        public Household Household { get; set; }

        public bool AllowEditDelete { get; set; }
        public Xamarin.Forms.GridLength EditDeleteRowHeight { get; set; }

        public HouseholdViewModel(ApplicationInstanceData applicationInstanceData, Household household)
        {
            ApplicationInstanceData = applicationInstanceData;
            Household = household;
            AllowEditDelete = !household.HasExternalId;
            EditDeleteRowHeight = AllowEditDelete ? 80 : 0;
        }
    }
}
