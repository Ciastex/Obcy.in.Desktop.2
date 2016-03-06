using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using ObcyInDesktop.Filesystem;
using ObcyInDesktop.Localization;
using ObcyInDesktop.Settings;
using ObcyInDesktop.UI;
using ObcyInDesktop.UI.Controls.ChatUI;
using ObcyProtoRev.Protocol.Client;
using ObcyProtoRev.Protocol.Events;
using Timer = System.Timers.Timer;

namespace ObcyInDesktop.Windows
{
    public partial class MainWindow
    {
        private bool _firstEscPressed;
        private readonly Timer _escTimer;

        private ChatView _chatView;

        private DebugWindow _debuggingWindow;
        private SettingsWindow _settingsWindow;
        private AboutWindow _aboutWindow;
        private ArchiveWindow _archiveWindow;

        private readonly ColorAnimation _glowFadeOutAnimation;
        private readonly ColorAnimation _glowFadeInAnimation;

        private readonly Storyboard _borderActivateStoryboard;
        private readonly Storyboard _borderDeactivateStoryboard;

        private Thread _blinkThread;

        public MainWindow()
        {
            InitializeComponent();

            CreateConnection();

            DirectoryGuard.DirectoriesSanityCheck();

            InitializeChatView();
            InitializeStatistics();

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

            SettingsSelector.SettingsChanged += SettingsSelector_SettingsChanged;

            _debuggingWindow = new DebugWindow(App.Connection);
            _settingsWindow = new SettingsWindow();
            _aboutWindow = new AboutWindow();
            _archiveWindow = new ArchiveWindow();

            _escTimer = new Timer(800);
            _escTimer.Elapsed += escTimer_Elapsed;

            SettingsSelector.SetConfigurationValue("Voivodeship", SettingsSelector.GetConfigurationValue<string>("Voivodeship"));
        }

        private void escTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _firstEscPressed = false;
            _escTimer.Stop();
        }

        private void SettingsSelector_SettingsChanged(object sender, EventArgs e)
        {
            if (SettingsSelector.GetConfigurationValue<bool>("Appearance_UppercaseMenuHeaders"))
            {
                MainMenuButton.Label = LocaleSelector.GetLocaleString("Menu_File").ToUpper();
                OptionsMenuButton.Label = LocaleSelector.GetLocaleString("Menu_Options").ToUpper();
                ViewMenuButton.Label = LocaleSelector.GetLocaleString("Menu_View").ToUpper();
            }
            else
            {
                MainMenuButton.Label = LocaleSelector.GetLocaleString("Menu_File");
                OptionsMenuButton.Label = LocaleSelector.GetLocaleString("Menu_Options");
                ViewMenuButton.Label = LocaleSelector.GetLocaleString("Menu_View");
            }
        }

        private void CreateConnection()
        {
            App.Connection.StrangerFound += connection_StrangerFound;
            App.Connection.MessageReceived += connection_MessageReceived;
            App.Connection.SocketClosed += connection_SocketClosed;
        }

        private void BlinkWindowGlow()
        {
            var isActive = false;
            var glow = Application.Current.Dispatcher.Invoke(() => (DropShadowEffect)FindName("BorderGlow"));

            for (var i = 0; i <= 3; i++)
            {
                if (isActive)
                    break;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (IsActive)
                        isActive = true;

                    glow?.BeginAnimation(DropShadowEffect.ColorProperty, _glowFadeInAnimation);

                    _borderActivateStoryboard.Begin();
                });

                Thread.Sleep(1000);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    glow?.BeginAnimation(DropShadowEffect.ColorProperty, _glowFadeOutAnimation);

                    _borderDeactivateStoryboard.Begin();
                });
            }
        }

        private void InitializeChatView()
        {
            _chatView = new ChatView(App.Connection)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Height = double.NaN,
                Width = double.NaN
            };
            _chatView.MessageSent += chatView_MessageSent;

            RightPanelArea.Child = _chatView;
        }

        private void InitializeStatistics()
        {
            if (File.Exists(Path.Combine(DirectoryGuard.DataDirectory, DirectoryGuard.StatisticsFileName)))
            {
                App.StatsManager.LoadStats(
                    Path.Combine(DirectoryGuard.DataDirectory, DirectoryGuard.StatisticsFileName)
                );
            }
        }

        private void chatView_MessageSent(object sender, EventArgs e)
        {
            App.StatsManager.AddSentMessage();
        }

        private void connection_MessageReceived(object sender, MessageEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (!IsActive)
                {
                    _blinkThread = new Thread(BlinkWindowGlow);
                    _blinkThread.Start();

                    if(e.Message.Type != MessageType.Service)
                        App.WindowFlashHelper.FlashApplicationWindow();
                }
            });
        }

        private void connection_SocketClosed(object sender, SocketClosedEventArgs e)
        {
            App.StatsManager.StopRecording();
        }

        private void connection_StrangerFound(object sender, StrangerFoundEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (SettingsSelector.GetConfigurationValue<bool>("Behavior_SendSexQueryOnStart"))
                {
                    _chatView.SendMessage("km, wiek?");
                }
            });
        }

        private void MainWindow_OnActivated(object sender, EventArgs e)
        {
            var glow = (DropShadowEffect)FindName("BorderGlow");

            _blinkThread?.Abort();
            glow?.BeginAnimation(DropShadowEffect.ColorProperty, _glowFadeInAnimation);

            _borderActivateStoryboard.Begin();
        }

        private void MainWindow_OnDeactivated(object sender, EventArgs e)
        {
            var glow = (DropShadowEffect)FindName("BorderGlow");

            glow?.BeginAnimation(DropShadowEffect.ColorProperty, _glowFadeOutAnimation);

            _borderDeactivateStoryboard.Begin();
        }

        private void ToggleCopyViewMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            _chatView.ToggleCopyView();
            _chatView.Toolbar.CopyViewToggleButton.IsChecked = !_chatView.Toolbar.CopyViewToggleButton.IsChecked;
        }

        private void ToggleToolbarMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            _chatView.ToggleToolbar();
        }

        private void ExitMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ArchiveManagerMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            // AlertWindow.Show(LocaleSelector.GetLocaleString("AlertWindow_IDontThinkSo"));
            if (_archiveWindow.IsVisible)
            {
                _archiveWindow.Activate();
            }
            else
            {
                _archiveWindow = new ArchiveWindow();
                _archiveWindow.Show();
            }
        }

        private void SettingsMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (_settingsWindow.IsVisible)
            {
                _settingsWindow.Activate();
            }
            else
            {
                _settingsWindow = new SettingsWindow();
                _settingsWindow.ShowDialog();
            }
        }

        private void AboutApplicationMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (_aboutWindow.IsVisible)
            {
                _aboutWindow.Activate();
            }
            else
            {
                _aboutWindow = new AboutWindow();
                _aboutWindow.ShowDialog();
            }
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) &&
                Keyboard.IsKeyDown(Key.LeftShift) &&
                Keyboard.IsKeyDown(Key.F12))
            {
                if (_debuggingWindow.IsVisible)
                {
                    _debuggingWindow.Activate();
                }
                else
                {
                    _debuggingWindow = new DebugWindow(App.Connection);
                    _debuggingWindow.Show();
                }
            }
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            _debuggingWindow.Close();
            _settingsWindow.Close();

            App.StatsManager.StopRecording();
            App.StatsManager.SaveStats(
                Path.Combine(DirectoryGuard.DataDirectory, DirectoryGuard.StatisticsFileName)
            );

            Environment.Exit(0);
        }

        private void ClearLogCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _chatView.ClearLog();
        }

        private void SwitchStrangerCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (App.Connection.IsStrangerConnected)
            {
                if (!_firstEscPressed)
                {
                    _escTimer.Start();
                    _firstEscPressed = true;
                }
                else
                {
                    _chatView.DisconnectFromStranger();
                    _escTimer.Stop();
                    _firstEscPressed = false;
                }
            }
            else
            {
                _chatView.SearchForStranger();
            }
        }


        private void FlagStrangerCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _chatView.FlagStranger();
        }

        private void ToggleCopyViewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _chatView.ToggleCopyView();
            _chatView.Toolbar.CopyViewToggleButton.IsChecked = !_chatView.Toolbar.CopyViewToggleButton.IsChecked;
        }

        private void ToggleScrollingCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _chatView.ToggleLogScrolling();
            _chatView.Toolbar.LogScrollingToggleButton.IsChecked = !_chatView.Toolbar.LogScrollingToggleButton.IsChecked;
        }
    }
}
