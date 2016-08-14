using System;
using System.IO;
using System.Windows;
using ObcyInDesktop.Archive;
using ObcyInDesktop.Filesystem;
using ObcyInDesktop.Localization;
using ObcyInDesktop.Settings;
using ObcyInDesktop.Statistics;
using ObcyInDesktop.UI;
using ObcyInDesktop.Windows;
using ObcyInDesktop.Windows.Blinking;
using ObcyProtoRev.Protocol;

namespace ObcyInDesktop
{
    public partial class App
    {
        public static WindowFlashHelper WindowFlashHelper { get; private set; }
        public static Connection Connection { get; private set; }
        public static ColorSchemeLoader ColorSchemeLoader { get; private set; }
        public static StatsManager StatsManager { get; private set; }
        public static ArchiveManager ArchiveManager { get; private set; }

        public static bool CanWriteToCurrentFolder { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var systemVersion = Environment.OSVersion.Version;
            if (systemVersion.Major < 6)
            {
                MessageBox.Show("Incompatible system version, must be at least Windows Vista", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            DirectoryGuard.DirectoriesSanityCheck();
            if (!HasWriteAccessToFolder(AppDomain.CurrentDomain.BaseDirectory))
            {
                AlertWindow.Show(LocaleSelector.GetLocaleString("AlertWindow_NoWriteAccess"));
                CanWriteToCurrentFolder = false;
            }
            else
            {
                CanWriteToCurrentFolder = true;
            }
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            WindowFlashHelper = new WindowFlashHelper(this);

            Connection = new Connection
            {
                SendUserAgent = SettingsSelector.GetConfigurationValue<bool>("Behavior_SendUserAgent")
            };
            StatsManager = new StatsManager(Connection);
            ArchiveManager = new ArchiveManager();

            LocaleSelector.SetLocaleIdentifier(
                SettingsSelector.GetConfigurationValue<string>("Language")
            );

            ColorSchemeLoader = new ColorSchemeLoader();

            ColorSchemeLoader.LoadColorSchemes();
            ColorSchemeLoader.ApplyColors();

            base.OnStartup(e);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception
                = e.ExceptionObject as Exception;
            if (exception != null)
                MessageBox.Show(exception.ToString());
            else
                MessageBox.Show("Exception is null", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private static bool HasWriteAccessToFolder(string folderPath)
        {
            try
            {
                Directory.GetAccessControl(folderPath);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }
    }
}
