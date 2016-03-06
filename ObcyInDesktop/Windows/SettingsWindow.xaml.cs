using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using Microsoft.Win32;
using ObcyInDesktop.Localization;
using ObcyInDesktop.Settings;
using ObcyInDesktop.UI;

namespace ObcyInDesktop.Windows
{
    public partial class SettingsWindow
    {
        private readonly ColorAnimation _glowFadeOutAnimation;
        private readonly ColorAnimation _glowFadeInAnimation;

        private readonly Storyboard _borderActivateStoryboard;
        private readonly Storyboard _borderDeactivateStoryboard;

        public SettingsWindow()
        {
            InitializeComponent();

            _glowFadeOutAnimation = new ColorAnimation(Colors.WindowActiveGlow, Colors.WindowInactiveGlow, new Duration(new TimeSpan(0, 0, 0, 0, 150)));
            _glowFadeInAnimation = new ColorAnimation(Colors.WindowInactiveGlow, Colors.WindowActiveGlow, new Duration(new TimeSpan(0, 0, 0, 0, 150)));

            var borderActivateAnimation = new ColorAnimation(Colors.WindowInactiveBorder, Colors.WindowActiveBorder, new Duration(new TimeSpan(0, 0, 0, 0, 150)));
            var borderDeactivateAnimation = new ColorAnimation(Colors.WindowActiveBorder, Colors.WindowInactiveBorder, new Duration(new TimeSpan(0, 0, 0, 0, 150)));

            _borderActivateStoryboard = new Storyboard();
            _borderDeactivateStoryboard = new Storyboard();

            Storyboard.SetTarget(borderActivateAnimation, WindowBorder);
            Storyboard.SetTargetProperty(borderActivateAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));
            _borderActivateStoryboard.Children.Add(borderActivateAnimation);

            Storyboard.SetTarget(borderDeactivateAnimation, WindowBorder);
            Storyboard.SetTargetProperty(borderDeactivateAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));
            _borderDeactivateStoryboard.Children.Add(borderDeactivateAnimation);

            Title = LocaleSelector.GetLocaleString("SettingsWindow_Title");
        }

        private void SettingsWindow_OnActivated(object sender, EventArgs e)
        {
            var glow = (DropShadowEffect)FindName("BorderGlow");
            glow?.BeginAnimation(DropShadowEffect.ColorProperty, _glowFadeInAnimation);

            _borderActivateStoryboard.Begin();
        }

        private void SettingsWindow_OnDeactivated(object sender, EventArgs e)
        {
            var glow = (DropShadowEffect)FindName("BorderGlow");
            glow?.BeginAnimation(DropShadowEffect.ColorProperty, _glowFadeOutAnimation);

            _borderDeactivateStoryboard.Begin();
        }

        private void SettingsWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            foreach (var s in LocaleSelector.GetAvailableLocaleIdentifiers())
            {
                LanguageSelectComboBox.Items.Add(s);
                LanguageSelectComboBox.SelectedItem = LocaleSelector.GetCurrentLocaleIdentifier();
            }

            foreach (var s in App.ColorSchemeLoader.ColorSchemes.Keys)
            {
                ColorSchemeComboBox.Items.Add(s);
                ColorSchemeComboBox.SelectedItem = SettingsSelector.GetConfigurationValue<string>("ColorScheme");
            }

            SendSexQueryCheckBox.IsChecked = SettingsSelector.GetConfigurationValue<bool>("Behavior_SendSexQueryOnStart");
            StartupWithSystemCheckBox.IsChecked = SettingsSelector.GetConfigurationValue<bool>("Behavior_StartupWithSystem");
            SendUserAgentCheckBox.IsChecked = SettingsSelector.GetConfigurationValue<bool>("Behavior_SendUserAgent");
            SendChatstateCheckBox.IsChecked = SettingsSelector.GetConfigurationValue<bool>("Behavior_SendChatstate");
            ToggleCopyViewOnDoubleClickCheckBox.IsChecked = SettingsSelector.GetConfigurationValue<bool>("Behavior_CopyViewOnLogDoubleClick");
            UpperCaseMenuHeaderCheckBox.IsChecked = SettingsSelector.GetConfigurationValue<bool>("Appearance_UppercaseMenuHeaders");
            MessageMarginsCheckBox.IsChecked = SettingsSelector.GetConfigurationValue<bool>("Appearance_MessageMargins");

            VoivodeshipSelectComboBox.SelectedIndex = SettingsSelector.GetConfigurationValue<int>("Voivodeship");
        }

        private void LanguageSelectComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems?[0] == null)
                return;

            SettingsSelector.SetConfigurationValue("Language", e.AddedItems[0] as string);
            LocaleSelector.SetLocaleIdentifier(e.AddedItems[0] as string);
        }

        private void VoivodeshipSelectComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingsSelector.SetConfigurationValue("Voivodeship", VoivodeshipSelectComboBox.SelectedIndex.ToString());
        }


        private void ColorSchemeComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems?[0] == null)
                return;

            SettingsSelector.SetConfigurationValue("ColorScheme", e.AddedItems[0] as string);
            App.ColorSchemeLoader.ApplyColors();
        }

        private void SendSexQueryCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            SettingsSelector.SetConfigurationValue("Behavior_SendSexQueryOnStart", "True");
        }

        private void SendSexQueryCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            SettingsSelector.SetConfigurationValue("Behavior_SendSexQueryOnStart", "False");
        }

        private void StartupWithSystemCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            SettingsSelector.SetConfigurationValue("Behavior_StartupWithSystem", "True");
            SetStartup(true);
        }

        private void StartupWithSystemCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            SettingsSelector.SetConfigurationValue("Behavior_StartupWithSystem", "False");
            SetStartup(false);
        }

        private void SendUserAgentCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            SettingsSelector.SetConfigurationValue("Behavior_SendUserAgent", "True");
        }

        private void SendUserAgentCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            SettingsSelector.SetConfigurationValue("Behavior_SendUserAgent", "False");
        }

        private void SendChatstateCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            SettingsSelector.SetConfigurationValue("Behavior_SendChatstate", "True");
        }

        private void SendChatstateCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            SettingsSelector.SetConfigurationValue("Behavior_SendChatstate", "False");
        }

        private void ToggleCopyViewOnDoubleClickCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            SettingsSelector.SetConfigurationValue("Behavior_CopyViewOnLogDoubleClick", "True");
        }

        private void ToggleCopyViewOnDoubleClickCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            SettingsSelector.SetConfigurationValue("Behavior_CopyViewOnLogDoubleClick", "False");
        }

        private void ClearStatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            var choice = ChoiceWindow.Show(LocaleSelector.GetLocaleString("ChoiceWindow_StatisticsEraseWarning"));

            if (choice)
            {
                App.StatsManager.ClearStats();
            }
        }

        private void ClearArchiveButton_Click(object sender, RoutedEventArgs e)
        {
            var choice = ChoiceWindow.Show(LocaleSelector.GetLocaleString("ChoiceWindow_ArchiveEraseWarning"));

            if (choice)
            {
                App.ArchiveManager.EraseArchives();
            }
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UpperCaseMenuHeaderCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            SettingsSelector.SetConfigurationValue("Appearance_UppercaseMenuHeaders", "True");
        }

        private void UpperCaseMenuHeaderCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            SettingsSelector.SetConfigurationValue("Appearance_UppercaseMenuHeaders", "False");
        }


        private void MessageMarginsCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            SettingsSelector.SetConfigurationValue("Appearance_MessageMargins", "True");
        }

        private void MessageMarginsCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            SettingsSelector.SetConfigurationValue("Appearance_MessageMargins", "False");
        }

        private void SetStartup(bool set)
        {
            var rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (set)
            {
                rk?.SetValue("Obcy.in.Desktop", Environment.GetCommandLineArgs()[0]);
            }
            else
            {
                rk?.DeleteValue("Obcy.in.Desktop", false);
            }
        }
    }
}
