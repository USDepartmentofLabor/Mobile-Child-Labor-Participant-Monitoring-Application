using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels
{
    public class HouseholdMemberAssignServiceViewModel : ViewModelBase
    {
        public Command CancelCommand { get; set; }
        public Command SubmitCommand { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Today;
        public DateTime EndDate { get; set; } = DateTime.Today;
        public int Hours { get; set; }
        public string Notes { get; set; }

        private readonly Person _person;
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

        public HouseholdMemberAssignServiceViewModel(ApplicationInstanceData applicationInstanceData, Person person)
        {
            ApplicationInstanceData = applicationInstanceData;

            CancelCommand = new Command(ExecuteCancelCommand);
            SubmitCommand = new Command(ExecuteSubmitCommand);

            _person = new Person();
            _person = person;

            _selectServiceTranslation = ApplicationInstanceData.SelectedLocalization.Translations[@"SelectService"];

            BindableServices = new ObservableCollection<Tuple<string, Service>>
            {
                new Tuple<string, Service>(_selectServiceTranslation, null)
            };
            foreach (var service in ApplicationInstanceData.Data.Services)
            {
                BindableServices.Add(new Tuple<string, Service>(service.Name, service));
            }
            SelectedBindableService = BindableServices.First();
        }

        private void Exit()
        {
            ApplicationInstanceData.NavigationPage.PopAsync();
        }

        private void ExecuteCancelCommand()
        {
            Exit();
        }

        private bool ValidateViewBeforeSubmit()
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

        private void ExecuteSubmitCommand()
        {
            if (!ValidateViewBeforeSubmit()) return;
            var newServiceInstance = new ServiceInstance
            {
                LastUpdatedAt = DateTime.Now,
                Service = SelectedBindableService.Item2,
                StartDate = StartDate,
                EndDate = EndDate,
                Hours = Hours,
                Notes = Notes
            };
            _person.AddServiceInstance(newServiceInstance);
            ApplicationInstanceData.Data.SaveChanges();
            Exit();
        }
    }
}
