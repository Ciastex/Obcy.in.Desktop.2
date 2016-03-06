using System.IO;
using System.Linq;

namespace ObcyInDesktop.Settings
{
    class SettingsFileParser
    {
        private readonly string _filePath;
        private readonly SettingsManager _settingsManager;
        
        public SettingsFileParser(string filePath, SettingsManager settingsManager)
        {
            _filePath = filePath;
            _settingsManager = settingsManager;
        }

        public void LoadSettings()
        {
            using (var sr = new StreamReader(_filePath))
            {
                var fileContent = sr.ReadToEnd();
                var splitSettingLines = fileContent.Split('\n');

                foreach (var settingString in splitSettingLines)
                {
                    if (settingString.StartsWith("#") || string.IsNullOrEmpty(settingString))
                    {
                        continue;
                    }

                    var settingStringSplitByEquality = settingString.Split('=');
                    var settingStringList = settingStringSplitByEquality.ToList();
                    settingStringList.RemoveAt(0);

                    var settingStringKey = settingStringSplitByEquality[0];
                    var settingStringValue = string.Join("", settingStringList.ToArray()).Trim();

                    _settingsManager[settingStringKey] = settingStringValue;
                }
            }
        }
    }
}
