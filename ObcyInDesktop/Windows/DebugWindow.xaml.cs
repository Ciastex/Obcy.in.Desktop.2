using System;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using ObcyInDesktop.Localization;
using ObcyInDesktop.UI;
using ObcyProtoRev.Protocol;
using ObcyProtoRev.Protocol.Events;

namespace ObcyInDesktop.Windows
{
    public partial class DebugWindow
    {
        private readonly Connection _connection;

        private readonly ColorAnimation _glowFadeOutAnimation;
        private readonly ColorAnimation _glowFadeInAnimation;

        private readonly Storyboard _borderActivateStoryboard;
        private readonly Storyboard _borderDeactivateStoryboard;

        public DebugWindow()
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

            Title = LocaleSelector.GetLocaleString("DebugWindow_Title");
        }

        public DebugWindow(Connection connection) : this()
        {
            _connection = connection;
            connection.JsonRead += connection_JsonRead;
            connection.JsonWrite += connection_JsonWrite;

            var updateTimer = new Timer(200);
            updateTimer.Elapsed += UpdateTimer_OnElapsed;
            updateTimer.Start();
        }

        private void DebugWindow_OnActivated(object sender, EventArgs e)
        {
            var glow = (DropShadowEffect)FindName("BorderGlow");
            glow?.BeginAnimation(DropShadowEffect.ColorProperty, _glowFadeInAnimation);

            _borderActivateStoryboard.Begin();
        }

        private void DebugWindow_OnDeactivated(object sender, EventArgs e)
        {
            var glow = (DropShadowEffect)FindName("BorderGlow");
            glow?.BeginAnimation(DropShadowEffect.ColorProperty, _glowFadeOutAnimation);

            _borderDeactivateStoryboard.Begin();
        }

        private void connection_JsonWrite(object sender, JsonEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                JsonConsoleTextBox.AppendText($"OUT\n{e.JsonData}\n\n");
                JsonConsoleTextBox.ScrollToEnd();
            });
        }

        void connection_JsonRead(object sender, JsonEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                JsonConsoleTextBox.AppendText($"IN\n{e.JsonData}\n\n");
                JsonConsoleTextBox.ScrollToEnd();
            });
        }

        private void UpdateTimer_OnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                IsOpenTextBlock.Text = $"IsOpen: {_connection.IsOpen}";
                IsReadyTextBlock.Text = $"IsReady: {_connection.IsReady}";
                IsStrangerConnectedTextBlock.Text = $"IsStrangerConnected: {_connection.IsStrangerConnected}";
                IsSearchingForStrangerTextBlock.Text = $"IsSearchingForStranger: {_connection.IsSearchingForStranger}";
                StrangerIdTextBlock.Text = $"Stranger ID: {_connection.CurrentContactUID}";
            });
        }

        private void JsonManualInputTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && JsonManualInputTextBox.Text.Length > 0)
            {
                _connection.SendJson(JsonManualInputTextBox.Text);
                JsonManualInputTextBox.Clear();
            }
        }

        private void InsertTemplateHyperlink_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                JsonManualInputTextBox.Text = "[\"{\\\"ev_name\\\":\\\"EVENT_NAME\\\",\\\"ev_data\\\":{EVENT_DATA}}\"]";
            });
        }
    }
}
