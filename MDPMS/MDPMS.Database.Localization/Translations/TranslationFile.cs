using System.Collections.Generic;

namespace MDPMS.Database.Localization.Translations
{
    public class Translation
    {
        public string Abbreviation { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Translations { get; set; }
    }

    public static class TranslationFile
    {
        public static List<Translation> GetAllSupportedTranslations()
        {
            return new List<Translation>
            {
                GetTranslationValues_en_English(),
                GetTranslationValues_es_Español()
            };
        }
        
        public static Translation GetTranslationValues_en_English()
        {
            var translation = new Translation
            {
                Abbreviation = @"en",
                Name = @"English",
                Translations = new Dictionary<string, string>()
            };            
            translation.Translations.Add(@"ProjectName", @"Direct Participant Monitoring System");
            translation.Translations.Add(@"Mobile", @"Mobile");
            translation.Translations.Add(@"ProjectPhrase", @"The more we connect, the better it gets.");           
            translation.Translations.Add(@"Continue", @"Continue");
            translation.Translations.Add(@"SelectLocalization", @"Select Localization");
            translation.Translations.Add(@"Home", @"Home");
            translation.Translations.Add(@"Households", @"Households");
            translation.Translations.Add(@"Settings", @"Settings");
            translation.Translations.Add(@"About", @"About");
            translation.Translations.Add(@"ApiKey", @"API Key");
            translation.Translations.Add(@"Sync", @"Sync");
            translation.Translations.Add(@"Syncing", @"Syncing");
            translation.Translations.Add(@"Obtained", @"Obtained");
            translation.Translations.Add(@"Username", @"Username");
            translation.Translations.Add(@"Password", @"Password");
            translation.Translations.Add(@"True", @"True");
            translation.Translations.Add(@"False", @"False");
            translation.Translations.Add(@"Yes", @"Yes");
            translation.Translations.Add(@"No", @"No");
            translation.Translations.Add(@"Error", @"Error");
            translation.Translations.Add(@"GetNewApiKey", @"Get New API Key");
            translation.Translations.Add(@"ShowPassword", @"Show Password");
            translation.Translations.Add(@"Authenticate", @"Authenticate");
            translation.Translations.Add(@"OK", @"OK");
            translation.Translations.Add(@"Cancel", @"Cancel");
            translation.Translations.Add(@"Alert", @"Alert");
            translation.Translations.Add(@"ConnectivityProblem", @"Connectivity Problem");            
            return translation;
        }

        public static Translation GetTranslationValues_es_Español()
        {
            var translation = new Translation
            {
                Abbreviation = @"es",
                Name = @"Español",
                Translations = new Dictionary<string, string>()
            };
            translation.Translations.Add(@"ProjectName", @"Sistema de monitoreo directo de participantes");
            translation.Translations.Add(@"Mobile", @"Móvil");
            translation.Translations.Add(@"ProjectPhrase", @"Cuanto más nos conectamos, mejor se pone.");
            translation.Translations.Add(@"Continue", @"Continuar");
            translation.Translations.Add(@"SelectLocalization", @"Seleccionar localización");
            translation.Translations.Add(@"Home", @"Casa");
            translation.Translations.Add(@"Households", @"Hogares");
            translation.Translations.Add(@"Settings", @"Configuraciones");
            translation.Translations.Add(@"About", @"Acerca de");
            translation.Translations.Add(@"ApiKey", @"Clave API");
            translation.Translations.Add(@"Sync", @"Sincronización");
            translation.Translations.Add(@"Syncing", @"Sincronizando");
            translation.Translations.Add(@"Obtained", @"Adquirido");
            translation.Translations.Add(@"Username", @"Nombre de usuario");
            translation.Translations.Add(@"Password", @"Contraseña");
            translation.Translations.Add(@"True", @"Cierto");
            translation.Translations.Add(@"False", @"Falso");
            translation.Translations.Add(@"Yes", @"Sí");
            translation.Translations.Add(@"No", @"No");
            translation.Translations.Add(@"Error", @"Error");
            translation.Translations.Add(@"GetNewApiKey", @"Obtener una nueva clave de API");
            translation.Translations.Add(@"ShowPassword", @"Mostrar contraseña");
            translation.Translations.Add(@"Authenticate", @"Autenticar");
            translation.Translations.Add(@"OK", @"De acuerdo");
            translation.Translations.Add(@"Cancel", @"Cancelar");
            translation.Translations.Add(@"Alert", @"Alerta");
            translation.Translations.Add(@"ConnectivityProblem", @"Problema de conectividad");            
            return translation;
        }
    }     
}
