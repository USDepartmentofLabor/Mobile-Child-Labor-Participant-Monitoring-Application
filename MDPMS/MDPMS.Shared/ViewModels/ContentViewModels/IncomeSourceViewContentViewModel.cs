using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;

namespace MDPMS.Shared.ViewModels.ContentViewModels
{
    public class IncomeSourceViewContentViewModel : ViewModelBase
    {
        public IncomeSource IncomeSource { get; set; }

        public IncomeSourceViewContentViewModel(ApplicationInstanceData applicationInstanceData, IncomeSource incomeSource)
        {
            ApplicationInstanceData = applicationInstanceData;
            IncomeSource = incomeSource;
        }
    }
}
