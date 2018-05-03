using MDPMS.Shared.Models;

namespace MDPMS.Shared.ViewModels.Helpers
{
    public static class ViewModelDisplayValueConverter
    {
        public static string GetBooleanTranslated(Localization localization, bool? value)
        {
            if (value == null) return @"";
            return localization.Translations[(bool)value ? @"Yes" : @"No"];
        }
    }
}
