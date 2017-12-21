﻿using System;
using System.Collections.Generic;
using System.Linq;
using MDPMS.Database.Localization.Models;

namespace MDPMS.Database.Localization.Database
{
    public static class DatabaseSeed
    {
        public static void SeedDatabase(LocalizationDatabaseContext localizationDatabaseContext)
        {
            // Localizations
            const int enLocalizationId = 1;
            const int esLocalizationId = 2;
            // Dictionary<int, Tuple<string, string>> = <Id, <Code, Name>>
            foreach (var localization in new Dictionary<int, Tuple<string, string>>
            {
                { enLocalizationId, new Tuple<string, string>(@"en", @"English") },
                { esLocalizationId, new Tuple<string, string>(@"es", @"Español") }
            })
            {                
                if (localizationDatabaseContext.Localizations.Any(a => a.Code.Equals(localization.Value.Item1))) continue;
                if (localizationDatabaseContext.Localizations.Any(a => a.Id.Equals(localization.Key))) { throw new Exception(@"Duplicate Id in seed data"); }
                localizationDatabaseContext.Localizations.Add(new Models.Localization { Id = localization.Key, Code = localization.Value.Item1, Name = localization.Value.Item2 });
            }
            
            // Keys
            const int projectNameKeyId = 1;
            const int mobileKeyId = 2;
            const int projectPhraseKeyId = 3;
            const int continueKeyId = 4;
            const int selectLocalizationKeyId = 5;
            const int homeKeyId = 6;
            const int householdsKeyId = 7;
            const int settingsKeyId = 8;
            const int aboutKeyId = 9;
            const int dpmsUrlKeyId = 10;
            const int apiKeyKeyId = 11;
            const int syncKeyId = 12;
            const int syncingKeyId = 13;            
            const int obtainedKeyId = 14;
            const int usernameKeyId = 15;
            const int passwordKeyId = 16;
            const int trueKeyId = 17;
            const int falseKeyId = 18;
            const int yesKeyId = 19;
            const int noKeyId = 20;
            const int errorKeyId = 21;
            const int getNewApiKeyKeyId = 22;
            const int showPasswordKeyId = 23;
            const int authenticateKeyId = 24;
            const int okKeyId = 25;
            const int cancelKeyId = 26;
            const int alertKeyId = 27;
            const int connectivityProblemKeyId = 28;
            foreach (var key in new Dictionary<int, string>
            {
                { projectNameKeyId, @"ProjectName" },
                { mobileKeyId, @"Mobile" },
                { projectPhraseKeyId, @"ProjectPhrase" },
                { continueKeyId, @"Continue" },
                { selectLocalizationKeyId, @"SelectLocalization" },
                { homeKeyId, @"Home" },
                { householdsKeyId, @"Households" },
                { settingsKeyId, @"Settings" },
                { aboutKeyId, @"About" },
                { dpmsUrlKeyId, @"DPMSURL" },
                { apiKeyKeyId, @"ApiKey" },
                { syncKeyId, @"Sync" },
                { syncingKeyId, @"Syncing" },
                { obtainedKeyId, @"Obtained" },
                { usernameKeyId, @"Username" },
                { passwordKeyId, @"Password" },
                { trueKeyId, @"True" },
                { falseKeyId, @"False" },
                { yesKeyId, @"Yes" },
                { noKeyId, @"No" },
                { errorKeyId, @"Error" },
                { getNewApiKeyKeyId, @"GetNewApiKey" },
                { showPasswordKeyId, @"ShowPassword" },
                { authenticateKeyId, @"Authenticate" },
                { okKeyId, @"OK" },
                { cancelKeyId, @"Cancel" },
                { alertKeyId, @"Alert" },
                { connectivityProblemKeyId, @"ConnectivityProblem" }
            })
            {
                if (localizationDatabaseContext.Keys.Any(a => a.KeyName.Equals(key.Value))) continue;
                if (localizationDatabaseContext.Keys.Any(a => a.Id.Equals(key.Key))) { throw new Exception(@"Duplicate Id in seed data"); }                
                localizationDatabaseContext.Keys.Add(new Key { Id = key.Key, KeyName = key.Value });
            }
            localizationDatabaseContext.SaveChanges();

            // Values (translations)
            // Dictionary<int, List<Tuple<int, string>>> = <LocalizationId, <KeyId, Translation>>            
            var translations = new Dictionary<int, List<Tuple<int, string>>>
            {
                {
                    enLocalizationId, new List<Tuple<int, string>>
                    {
                        new Tuple<int, string>(projectNameKeyId, @"Direct Participant Monitoring System"),
                        new Tuple<int, string>(mobileKeyId, @"Mobile"),
                        new Tuple<int, string>(projectPhraseKeyId, @"The more we connect, the better it gets."),
                        new Tuple<int, string>(continueKeyId, @"Continue"),
                        new Tuple<int, string>(selectLocalizationKeyId, @"Select Localization"),
                        new Tuple<int, string>(homeKeyId, @"Home"),
                        new Tuple<int, string>(householdsKeyId, @"Households"),
                        new Tuple<int, string>(settingsKeyId, @"Settings"),
                        new Tuple<int, string>(aboutKeyId, @"About"),
                        new Tuple<int, string>(dpmsUrlKeyId, @"DPMS URL"),
                        new Tuple<int, string>(apiKeyKeyId, @"API Key"),
                        new Tuple<int, string>(syncKeyId, @"Sync"),
                        new Tuple<int, string>(syncingKeyId, @"Syncing"),
                        new Tuple<int, string>(obtainedKeyId, @"Obtained"),
                        new Tuple<int, string>(usernameKeyId, @"Username"),
                        new Tuple<int, string>(passwordKeyId, @"Password"),
                        new Tuple<int, string>(trueKeyId, @"True"),
                        new Tuple<int, string>(falseKeyId, @"False"),
                        new Tuple<int, string>(yesKeyId, @"Yes"),
                        new Tuple<int, string>(noKeyId, @"No"),
                        new Tuple<int, string>(errorKeyId, @"Error"),
                        new Tuple<int, string>(getNewApiKeyKeyId, @"Get New API Key"),
                        new Tuple<int, string>(showPasswordKeyId, @"Show Password"),
                        new Tuple<int, string>(authenticateKeyId, @"Authenticate"),
                        new Tuple<int, string>(okKeyId, @"OK"),
                        new Tuple<int, string>(cancelKeyId, @"Cancel"),
                        new Tuple<int, string>(alertKeyId, @"Alert"),
                        new Tuple<int, string>(connectivityProblemKeyId, @"Connectivity Problem")
                    }
                },
                {
                    esLocalizationId, new List<Tuple<int, string>>
                    {
                        new Tuple<int, string>(projectNameKeyId, @"Sistema de monitoreo directo de participantes"),
                        new Tuple<int, string>(mobileKeyId, @"Móvil"),
                        new Tuple<int, string>(projectPhraseKeyId, @"Cuanto más nos conectamos, mejor se pone."),
                        new Tuple<int, string>(continueKeyId, @"Continuar"),
                        new Tuple<int, string>(selectLocalizationKeyId, @"Seleccionar localización"),
                        new Tuple<int, string>(homeKeyId, @"Casa"),
                        new Tuple<int, string>(householdsKeyId, @"Hogares"),
                        new Tuple<int, string>(settingsKeyId, @"Configuraciones"),
                        new Tuple<int, string>(aboutKeyId, @"Acerca de"),
                        new Tuple<int, string>(dpmsUrlKeyId, @"DPMS URL"),
                        new Tuple<int, string>(apiKeyKeyId, @"Clave API"),
                        new Tuple<int, string>(syncKeyId, @"Sincronización"),
                        new Tuple<int, string>(syncingKeyId, @"Sincronizando"),
                        new Tuple<int, string>(obtainedKeyId, @"Adquirido"),
                        new Tuple<int, string>(usernameKeyId, @"Nombre de usuario"),
                        new Tuple<int, string>(passwordKeyId, @"Contraseña"),
                        new Tuple<int, string>(trueKeyId, @"Cierto"),
                        new Tuple<int, string>(falseKeyId, @"Falso"),
                        new Tuple<int, string>(yesKeyId, @"Sí"),
                        new Tuple<int, string>(noKeyId, @"No"),
                        new Tuple<int, string>(errorKeyId, @"Error"),
                        new Tuple<int, string>(getNewApiKeyKeyId, @"Obtener una nueva clave de API"),
                        new Tuple<int, string>(showPasswordKeyId, @"Mostrar contraseña"),
                        new Tuple<int, string>(authenticateKeyId, @"Autenticar"),
                        new Tuple<int, string>(okKeyId, @"De acuerdo"),
                        new Tuple<int, string>(cancelKeyId, @"Cancelar"),
                        new Tuple<int, string>(alertKeyId, @"Alerta"),
                        new Tuple<int, string>(connectivityProblemKeyId, @"Problema de conectividad")
                    }
                }
            };            
            foreach (var translationLocalization in translations)
            {
                foreach (var translation in translationLocalization.Value)
                {                    
                    if (localizationDatabaseContext.Values.Any(a => a.Localization.Id.Equals(translationLocalization.Key) & a.Key.Id.Equals(translation.Item1))) continue;
                    localizationDatabaseContext.Values.Add(new Value
                    {
                        Key = localizationDatabaseContext.Keys.First(a => a.Id.Equals(translation.Item1)),
                        Localization = localizationDatabaseContext.Localizations.First(a => a.Id.Equals(translationLocalization.Key)),
                        KeyLocalizationValue = translation.Item2
                    });
                }                
            }
            
            // Save
            localizationDatabaseContext.SaveChanges();
        }
    }
}
