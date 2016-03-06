using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using ObcyInDesktop.Localization;
using ObcyInDesktop.UI;

namespace ObcyInDesktop.Windows
{
    public partial class AlertWindow
    {
        private readonly ColorAnimation _glowFadeInAnimation;
        private readonly ColorAnimation _glowFadeOutAnimation;

        private readonly Storyboard _borderActivateStoryboard;
        private readonly Storyboard _borderDeactivateStoryboard;

        public AlertWindow()
        {
            InitializeComponent();

            Title = LocaleSelector.GetLocaleString("AlertWindow_Title");


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
        }

        public static void Show(string message)
        {
            var wnd = new AlertWindow
            {
                MessageTextBlock = { Text = message }
            };
            wnd.ShowDialog();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AlertWindow_OnActivated(object sender, EventArgs e)
        {
            var glow = (DropShadowEffect)FindName("BorderGlow");
            glow?.BeginAnimation(DropShadowEffect.ColorProperty, _glowFadeInAnimation);

            _borderActivateStoryboard.Begin();
        }

        private void AlertWindow_OnDeactivated(object sender, EventArgs e)
        {
            var glow = (DropShadowEffect)FindName("BorderGlow");
            glow?.BeginAnimation(DropShadowEffect.ColorProperty, _glowFadeOutAnimation);

            _borderDeactivateStoryboard.Begin();
        }
    }
}
