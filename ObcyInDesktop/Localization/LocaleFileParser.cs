using System.IO;
using System.Linq;

namespace ObcyInDesktop.Localization
{
    class LocaleFileParser
    {
        private readonly string _directory;
        private readonly LocaleManager _localeManager;

        public LocaleFileParser(string directory, LocaleManager localeManager)
        {
            this._directory = directory;
            this._localeManager = localeManager;
        }
        
        public void LoadTranslations()
        {
            var localeFiles = Directory.EnumerateFiles(_directory, "*.loc");

            foreach (var fileName in localeFiles)
            {
                var localeIdentifier = Path.GetFileNameWithoutExtension(fileName);
                _localeManager.CurrentLocaleIdentifier = localeIdentifier;

                using (var sr = new StreamReader(fileName))
                {
                    var fileContent = sr.ReadToEnd();
                    var splitLines = fileContent.Split('\n');

                    foreach(var localeEntryString in splitLines)
                    {
                        if (localeEntryString.StartsWith("#") || string.IsNullOrEmpty(localeEntryString))
                        {
                            continue;
                        }

                        var localeEntrySplitByEquality = localeEntryString.Split('=');
                        var localeEntryList = localeEntrySplitByEquality.ToList();
                        localeEntryList.RemoveAt(0);

                        var localeEntryKey = localeEntrySplitByEquality[0];
                        var localeEntryValue = string.Join("", localeEntryList.ToArray()).Trim();

                        _localeManager[localeEntryKey] = localeEntryValue;
                    }
                }
            }
        }
    }
}
