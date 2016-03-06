using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using ObcyInDesktop.Localization;
using ObcyInDesktop.UI;

namespace ObcyInDesktop.Windows
{
    public partial class AboutWindow
    {
        private readonly ColorAnimation _glowFadeInAnimation;
        private readonly ColorAnimation _glowFadeOutAnimation;

        private readonly Storyboard _borderActivateStoryboard;
        private readonly Storyboard _borderDeactivateStoryboard;

        private readonly DoubleAnimation _fadeOutAnimation;
        private readonly DoubleAnimation _fadeInAnimation;

        private readonly DoubleAnimation _heightUpAnimation;

        public AboutWindow()
        {
            InitializeComponent();

            Title = LocaleSelector.GetLocaleString("AboutWindow_Title");

            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            
            VersionNumberTextBlock.Text = string.Format(
                VersionNumberTextBlock.Text,
                assemblyName.Version.Major,
                assemblyName.Version.Minor,
                assemblyName.Version.Build,
                assemblyName.Version.Revision
            );
            
            _fadeOutAnimation = new DoubleAnimation(1, 0, new Duration(new TimeSpan(0, 0, 0, 0, 150)));
            _fadeInAnimation = new DoubleAnimation(0, 1, new Duration(new TimeSpan(0, 0, 0, 0, 150)));

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

            _heightUpAnimation = new DoubleAnimation(160, 370, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            _heightUpAnimation.Completed += heightUpAnimation_Completed;
        }

        private void heightUpAnimation_Completed(object sender, EventArgs e)
        {
            SpecialThanksGrid.Visibility = Visibility.Visible;
            SpecialThanksGrid.BeginAnimation(OpacityProperty, _fadeInAnimation);

            ShowSpecialThanksButton.BeginAnimation(OpacityProperty, _fadeOutAnimation);
            ShowSpecialThanksButton.Visibility = Visibility.Collapsed;

            MinHeight = 370;
        }

        private void AboutWindow_OnActivated(object sender, EventArgs e)
        {
            var glow = (DropShadowEffect)FindName("BorderGlow");
            glow?.BeginAnimation(DropShadowEffect.ColorProperty, _glowFadeInAnimation);

            _borderActivateStoryboard.Begin();
        }

        private void AboutWindow_OnDeactivated(object sender, EventArgs e)
        {
            var glow = (DropShadowEffect)FindName("BorderGlow");
            glow?.BeginAnimation(DropShadowEffect.ColorProperty, _glowFadeOutAnimation);

            _borderDeactivateStoryboard.Begin();
        }

        private void ShowSpecialThanksButton_OnClick(object sender, RoutedEventArgs e)
        {
            MaxHeight = 370;

            BeginAnimation(HeightProperty, _heightUpAnimation);
        }
    }
}
