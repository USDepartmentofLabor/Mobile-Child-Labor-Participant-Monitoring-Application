using System;
using System.Linq;
using MDPMS.Database.Localization.Models;

namespace MDPMS.Database.Localization.Database
{
    public static class DatabaseSeed
    {
        public static void SeedDatabase(LocalizationDatabaseContext context)
        {
            // update value if exists throughout
            var localizations = Translations.TranslationFile.GetAllSupportedTranslations();
            foreach (var localization in localizations)
            {
                var localizationQuery = context.Localizations.Where(a => a.Code.Equals(localization.Abbreviation));
                var localizationQueryCount = localizationQuery.Count();
                if (localizationQueryCount > 1) throw new Exception(@"Localization Database Seed Error - Localization Query Count is > 1");
                Models.Localization dbLocalization = null;
                if (localizationQueryCount.Equals(0))
                {
                    dbLocalization = new Models.Localization
                    {
                        Code = localization.Abbreviation,
                        Name = localization.Name
                    };
                    context.Localizations.Add(dbLocalization);
                    context.SaveChanges();
                }
                else
                {
                    dbLocalization = localizationQuery.First();
                }

                // check keys and update values
                foreach (var keyValuePair in localization.Translations)
                {
                    Key key = null;
                    var keyQuery = context.Keys.Where(a => a.KeyName.Equals(keyValuePair.Key));
                    var keyQueryCount = keyQuery.Count();
                    if (keyQueryCount > 1) throw new Exception(@"Localization Database Seed Error - Key Query Count is > 1");                    
                    if (keyQueryCount.Equals(0))
                    {
                        key = new Key { KeyName = keyValuePair.Key };
                        context.Keys.Add(key);
                        context.SaveChanges();
                    }
                    else
                    {
                        key = keyQuery.First();
                    }

                    Value value = null;
                    var valueQuery = context.Values.Where(a => a.Localization.Equals(dbLocalization) && a.Key.Equals(key));
                    var valueQueryCount = valueQuery.Count();
                    if (valueQueryCount > 1) throw new Exception(@"Localization Database Seed Error - Value Query Count is > 1");
                    if (valueQueryCount.Equals(0))
                    {
                        value = new Value
                        {
                            Key = key,
                            Localization = dbLocalization,
                            KeyLocalizationValue = keyValuePair.Value
                        };
                        context.Values.Add(value);
                        context.SaveChanges();
                    }
                    else
                    {
                        value = valueQuery.First();
                        if (!value.KeyLocalizationValue.Equals(keyValuePair.Value))
                        {
                            value.KeyLocalizationValue = keyValuePair.Value;
                        }
                        context.SaveChanges();
                    }                  
                }                
            }            
        }        
    }
}
