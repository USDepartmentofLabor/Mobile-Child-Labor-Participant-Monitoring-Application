using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MDPMS.Database.Data.Database;
using MDPMS.Database.Localization.Database;

namespace MDPMS.Shared.Models
{
    public class ApplicationInstanceData
    {
        // PURPOSE: package instance data (settings, etc.)     

        // Non-serialized data
        public string PlatformDataPath { get; set; }
        public string ApplicationInstanceDataFileName { get; set; }

        public string DatabasePath { get; set; }
        public MDPMSDatabaseContext Data { get; set; }

        public string LocalizationDatabasePath { get; set; }       
        public LocalizationDatabaseContext Localization { get; set; }
        
        // Serialized data
        public SerializedApplicationInstanceData SerializedApplicationInstanceData { get; set; }   
        
        // Localization data
        public ObservableCollection<Localization> AvailableLocalizations { get; set; }
        public Localization SelectedLocalization { get; set; }

        public void GetAvailableLocalizations()
        {
            AvailableLocalizations = new ObservableCollection<Localization>();
            var availableLocalizationsQuery = Localization.Localizations.ToList();
            foreach (var availableLocalization in availableLocalizationsQuery)
            {
                AvailableLocalizations.Add(new Localization
                {
                    Abbreviation = availableLocalization.Code,
                    Name = availableLocalization.Name,
                    Translations = new Dictionary<string, string>()
                });
            }
        }

        public void SetLocalization(string code)
        {
            SelectedLocalization = AvailableLocalizations.First(a => a.Abbreviation.Equals(code));
            var translationsQuery =
                from keyvalues in Localization.Values.Where(a => a.Localization.Code.Equals(SelectedLocalization.Abbreviation))
                join keys in Localization.Keys
                    on keyvalues.Key.Id equals keys.Id
                select new { keyvalues.Key.KeyName, keyvalues.KeyLocalizationValue };
            SelectedLocalization.Translations = new Dictionary<string, string>();
            foreach (var translation in translationsQuery)
            {
                SelectedLocalization.Translations.Add(translation.KeyName, translation.KeyLocalizationValue);
            }            
        }
    }
}
