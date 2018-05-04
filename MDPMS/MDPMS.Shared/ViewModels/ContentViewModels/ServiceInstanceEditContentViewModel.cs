using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;

namespace MDPMS.Shared.ViewModels.ContentViewModels
{
    public class ServiceInstanceEditContentViewModel : ViewModelBase
    {
        public ServiceInstance ServiceInstance { get; set; }

        // temp/locally cached properties
        public Service Service { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Hours { get; set; }
        public string Notes { get; set; }

        private readonly bool isCreate;
        private readonly Person person;

        private string _selectServiceTranslation;

        // Bindable Tuple<string, Service> = DisplayName, Service
        private Tuple<string, Service> _selectedBindableService = new Tuple<string, Service>(@"", null);

        public Tuple<string, Service> SelectedBindableService
        {
            get => _selectedBindableService;
            set
            {
                _selectedBindableService = value;
                OnPropertyChanged(nameof(SelectedBindableService));
            }
        }

        public ObservableCollection<Tuple<string, Service>> BindableServices { get; set; }

        public ServiceInstanceEditContentViewModel(ApplicationInstanceData applicationInstanceData, Person parentPerson)
        {
            isCreate = true;
            person = parentPerson;
            Init(applicationInstanceData);
        }

        public ServiceInstanceEditContentViewModel(ApplicationInstanceData applicationInstanceData, ServiceInstance serviceInstance)
        {
            ServiceInstance = serviceInstance;
            isCreate = false;
            Init(applicationInstanceData);
        }

        private void Init(ApplicationInstanceData applicationInstanceData)
        {            
            ApplicationInstanceData = applicationInstanceData;

            _selectServiceTranslation = ApplicationInstanceData.SelectedLocalization.Translations[@"SelectService"];

            BindableServices = new ObservableCollection<Tuple<string, Service>>
            {
                new Tuple<string, Service>(_selectServiceTranslation, null)
            };
            foreach (var service in ApplicationInstanceData.Data.Services)
            {
                BindableServices.Add(new Tuple<string, Service>(service.Name, service));
            }
            if (isCreate || ServiceInstance == null || ServiceInstance.Service == null)
            {
                SelectedBindableService = BindableServices.First();
            }
            else
            {
                var query = BindableServices.Where(b => b.Item2 != null).Where(a => a.Item2.InternalId == ServiceInstance.Service.InternalId);
                if (query.Any())
                {
                    SelectedBindableService = query.First();
                }
                else
                {
                    SelectedBindableService = BindableServices.First();
                }
            }

            if (isCreate)
            {
                ServiceInstance = new ServiceInstance
                {
                    Service = null,
                    Hours = 0,
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today,
                    Notes = @""
                };
            }

            // set temp/locally cached properties to support save/cancel
            Hours = (ServiceInstance.Hours == null) ? 0 : (int)ServiceInstance.Hours;
            StartDate = ServiceInstance.StartDate;
            EndDate = ServiceInstance.EndDate;
            Notes = ServiceInstance.Notes;
        }

        public bool Validate()
        {
            {
                // Accumulate multiple messages if logically possible
                var validation = true;
                var messages = new List<string>();

                // A service must be selected for successful submission
                if (SelectedBindableService == null | SelectedBindableService == BindableServices.First())
                {
                    messages.Add(ApplicationInstanceData.SelectedLocalization.Translations[@"ValidationErrorMessageNoServiceSelected"]);
                    validation = false;
                }

                // EndDate must be >= StartDate
                if (StartDate > EndDate)
                {
                    messages.Add(ApplicationInstanceData.SelectedLocalization.Translations[@"ValidationErrorMessageStartDateIsAfterEndDate"]);
                    validation = false;
                }

                // Hours >= 0
                if (Hours < 0)
                {
                    messages.Add(ApplicationInstanceData.SelectedLocalization.Translations[@"ValidationErrorMessageHoursInvalid"]);
                    validation = false;
                }

                // Display messages if form is not valid
                if (!validation)
                {
                    var consolidateValidationMessage = new StringBuilder();
                    foreach (var message in messages) consolidateValidationMessage.AppendLine(message);
                    ApplicationInstanceData.App.MainPage.DisplayAlert(
                        ApplicationInstanceData.SelectedLocalization.Translations[@"Validation"],
                        consolidateValidationMessage.ToString(),
                        ApplicationInstanceData.SelectedLocalization.Translations[@"OK"]);
                }

                return validation;
            }
        }

        public void Save()
        {
            // existing
            ServiceInstance.Service = SelectedBindableService.Item2;
            ServiceInstance.Hours = Hours;
            ServiceInstance.StartDate = StartDate;
            ServiceInstance.EndDate = EndDate;
            ServiceInstance.Notes = Notes;

            var now = DateTime.UtcNow;
            ServiceInstance.LastUpdatedAt = now;

            if (isCreate)
            {
                ServiceInstance.ExternalId = null;
                ServiceInstance.CreatedAt = null;
                ServiceInstance.SoftDeleted = false;
                person.AddServiceInstance(ServiceInstance);
            }

            ApplicationInstanceData.Data.SaveChanges();
        }
    }
}
