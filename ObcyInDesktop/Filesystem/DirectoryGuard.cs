using System;
using System.IO;

namespace ObcyInDesktop.Filesystem
{
    static class DirectoryGuard
    {
        public static string AppDataDirectoryName => "ObcyInDesktop";

        public static string DataDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppDataDirectoryName, "_data");
        public static string ArchiveDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppDataDirectoryName, "_archives");
        public static string LocaleDirectory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_locale");
        public static string ColorSchemesDirectory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_schemes");

        public static string SettingsFileName => "settings.cfg";
        public static string StatisticsFileName => "statistics.dat";

        public static void DirectoriesSanityCheck()
        {
            if (!Directory.Exists(DataDirectory))
            {
                Directory.CreateDirectory(DataDirectory);
            }

            if (!Directory.Exists(ArchiveDirectory))
            {
                Directory.CreateDirectory(ArchiveDirectory);
            }

            if (!Directory.Exists(LocaleDirectory))
            {
                Directory.CreateDirectory(LocaleDirectory);
            }

            if (!Directory.Exists(ColorSchemesDirectory))
            {
                Directory.CreateDirectory(ColorSchemesDirectory);
            }

            if (!File.Exists(Path.Combine(DataDirectory, SettingsFileName)))
            {
                using (var sw = new StreamWriter(Path.Combine(DataDirectory, SettingsFileName)))
                {
                    sw.WriteLine(
@"Behavior_StartupWithSystem=False
Behavior_SendUserAgent=True
Behavior_SendChatstate=False
Behavior_CopyViewOnLogDoubleClick=True
Behavior_SendSexQueryOnStart=False
Appearance_UppercaseMenuHeaders=False
Appearance_MessageMargins=True
Language=Polski
Voivodeship=12
ColorScheme=Visual Studio 2012");
                }
            }
        }
    }
}
