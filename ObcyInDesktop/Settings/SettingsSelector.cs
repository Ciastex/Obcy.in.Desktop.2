using System;
using System.IO;
using ObcyInDesktop.Filesystem;

namespace ObcyInDesktop.Settings
{
    static class SettingsSelector
    {
        public static event EventHandler SettingsChanged;
        private static readonly SettingsManager SettingsManager;

        static SettingsSelector()
        {
            SettingsManager = new SettingsManager();

            var settingsFileParser = new SettingsFileParser(
                Path.Combine(DirectoryGuard.DataDirectory, DirectoryGuard.SettingsFileName),
                SettingsManager
            );

            settingsFileParser.LoadSettings();
            SettingsChanged?.Invoke(null, EventArgs.Empty);
        }

        public static void SetConfigurationValue(string key, string value)
        {
            SettingsManager[key] = value;

            SettingsManager.Save(
                Path.Combine(DirectoryGuard.DataDirectory, DirectoryGuard.SettingsFileName)
            );
            SettingsChanged?.Invoke(null, EventArgs.Empty);
        }

        public static T GetConfigurationValue<T>(string key)
        {
            return SettingsManager.GetConfigurationValue<T>(key);
        }
    }
}
