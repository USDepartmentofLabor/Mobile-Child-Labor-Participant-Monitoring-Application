using System.Collections.ObjectModel;
using System.Linq;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.Base;
using MDPMS.Shared.Views.ContentPages;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels.ContentPageModels
{
    public class LocalizationSelectionContentPageModel : ViewModelBase
    {                
        public ObservableCollection<Localization> Localizations { get; set; }

        public Localization _selectedLocalization { get; set; }

        public Localization SelectedLocalization
        {
            get
            {
                return _selectedLocalization;
            }
            set
            {
                _selectedLocalization = value;
                if (_selectedLocalization == null || ApplicationInstanceData.SelectedLocalization == null) return;
                SetLocalizationSelectionChanged();
            }
        }

        private void SetLocalizationSelectionChanged()
        {
            if (!SelectedLocalization.Abbreviation.Equals(ApplicationInstanceData.SelectedLocalization.Abbreviation))
            {
                LocalizationSelectionChanged = true;
                OnPropertyChanged(nameof(LocalizationSelectionChanged));
                SetShowCancel();
            }
            else
            {
                if (LocalizationSelectionChanged)
                {
                    LocalizationSelectionChanged = false;
                    OnPropertyChanged(nameof(LocalizationSelectionChanged));
                    SetShowCancel();
                }
            }
        }

        private void SetShowCancel()
        {
            if (!isFromMainMenu) return;
            ShowCancel = LocalizationSelectionChanged;
            OnPropertyChanged(nameof(ShowCancel));
        }

        public Command NavigateToLandingPageCommand { get; set; }
        public Command NavigateToLandingPageCheckSelectionCommand { get; set; }

        public bool LocalizationSelectionChanged { get; set; }

        public bool ShowCancel { get; set; }

        private bool isFromMainMenu = false;

        public LocalizationSelectionContentPageModel(ApplicationInstanceData applicationInstanceData)
        {
            isFromMainMenu = false;
            ShowCancel = true;
            Init(applicationInstanceData);
        }

        public LocalizationSelectionContentPageModel(ApplicationInstanceData applicationInstanceData, bool IsFromMenu)
        {
            isFromMainMenu = IsFromMenu;
            Init(applicationInstanceData);
        }

        private void Init(ApplicationInstanceData applicationInstanceData)
        {
            NavigateToLandingPageCommand = new Command(() => ExecuteNavigateToLandingPageCommand());
            NavigateToLandingPageCheckSelectionCommand = new Command(() => ExecuteNavigateToLandingPageCheckSelectionCommand());
            ApplicationInstanceData = applicationInstanceData;
            Localizations = new ObservableCollection<Localization>();
            foreach (var availableLocalization in ApplicationInstanceData.AvailableLocalizations)
            {
                Localizations.Add(availableLocalization);
            }
            SelectedLocalization = ApplicationInstanceData.SelectedLocalization;
        }

        private void ExecuteNavigateToLandingPageCommand()
        {
            // cancel selection
            if (ApplicationInstanceData.SelectedLocalization != null)
            {
                var query = Localizations.Where(a => a.Abbreviation == ApplicationInstanceData.SelectedLocalization.Abbreviation);
                if (query.Any()) SelectedLocalization = query.First();
                OnPropertyChanged(nameof(SelectedLocalization));
            }

            if (isFromMainMenu) return;

            Application.Current.MainPage = new LandingContentPage
            {
                BindingContext = new LandingContentPageModel(ApplicationInstanceData)
            };
        }

        private void ExecuteNavigateToLandingPageCheckSelectionCommand()
        {
            if (!ApplicationInstanceData.SelectedLocalization.Abbreviation.Equals(SelectedLocalization.Abbreviation))
            {
                ApplicationInstanceData.SetLocalization(SelectedLocalization.Abbreviation);
                SetLocalizationSelectionChanged();

                // save pref
                ApplicationInstanceData.SerializedApplicationInstanceData.Localization = SelectedLocalization.Abbreviation;
                ApplicationInstanceData.SaveSerializedApplicationInstanceData();
            }

            if (isFromMainMenu)
            {
                // this view localization refresh
                NotifyPropertyChange(nameof(ApplicationInstanceData));

                // menu model localization refresh
                var menuContentPageModel = (MenuContentPageModel)ApplicationInstanceData.RootPage.Master.BindingContext;
                menuContentPageModel.NotifyPropertyChange(nameof(menuContentPageModel.ApplicationInstanceData));

                return;
            }

            Application.Current.MainPage = new LandingContentPage
            {
                BindingContext = new LandingContentPageModel(ApplicationInstanceData)
            };
        }
    }
}
