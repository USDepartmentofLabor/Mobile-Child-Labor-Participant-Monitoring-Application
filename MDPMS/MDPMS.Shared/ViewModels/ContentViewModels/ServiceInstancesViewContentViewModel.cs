using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.ViewModels.ContentPageModels;
using MDPMS.Shared.Views.ContentPages;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels.ContentViewModels
{
    public class ServiceInstancesViewContentViewModel : ViewModelBase
    {
        public Command AddServiceInstanceCommand { get; set; }

        public Person Person { get; set; }

        private ServiceInstance _selectedServiceInstance;
        public ServiceInstance SelectedServiceInstance
        {
            get => _selectedServiceInstance;
            set
            {
                _selectedServiceInstance = value;
                if (_selectedServiceInstance == null) return;
                ApplicationInstanceData.NavigationPage.PushAsync(new ServiceInstanceViewContentPage
                {
                    BindingContext = new ServiceInstanceViewContentPageModel(ApplicationInstanceData, value)
                });
            }
        }

        public ServiceInstancesViewContentViewModel(ApplicationInstanceData applicationInstanceData, Person person)
        {
            ApplicationInstanceData = applicationInstanceData;
            Person = person;
            AddServiceInstanceCommand = new Command(ExecuteAddServiceInstanceCommand);
        }

        private void ExecuteAddServiceInstanceCommand()
        {
            ApplicationInstanceData.NavigationPage.PushAsync(new ServiceInstanceEditContentPage { BindingContext = new ServiceInstanceEditContentPageModel(ApplicationInstanceData, Person) });
        }
    }
}
