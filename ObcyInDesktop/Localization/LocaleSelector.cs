using System.Collections.Generic;
using System.Linq;
using ObcyInDesktop.Filesystem;

namespace ObcyInDesktop.Localization
{
    static class LocaleSelector
    {
        private static readonly LocaleManager LocaleManager;

        static LocaleSelector()
        {
            LocaleManager = new LocaleManager();

            var localeFileParser = new LocaleFileParser(DirectoryGuard.LocaleDirectory, LocaleManager);
            localeFileParser.LoadTranslations();
        }

        public static string GetLocaleString(string key)
        {
            return LocaleManager[key];
        }

        public static string GetCurrentLocaleIdentifier()
        {
            return LocaleManager.CurrentLocaleIdentifier;
        }

        public static void SetLocaleIdentifier(string identifier)
        {
            LocaleManager.CurrentLocaleIdentifier = identifier;
        }

        public static List<string> GetAvailableLocaleIdentifiers()
        {
            return LocaleManager.Keys.ToList();
        } 
    }
}
