using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ObcyInDesktop.UI.Controls.ChatUI
{
    public partial class ToolbarControl
    {
        private readonly ImageSource _chatLogImage;
        private readonly ImageSource _scrollStopImage;
        private readonly ImageSource _eraseLogImage;
        private readonly ImageSource _flagStrangerImage;
        private readonly ImageSource _randomTopicImage;
        private readonly ImageSource _disconnectImage;
        private readonly ImageSource _searchImage;

        public ToolbarControl()
        {
            InitializeComponent();

            if (App.ColorSchemeLoader.CurrentSchemeUsesDarkIcons)
            {
                _chatLogImage =
                    new BitmapImage(new Uri("pack://application:,,,/Resources/Graphics/Toolbar/Dark/ChatLog.png")
                );

                _scrollStopImage =
                    new BitmapImage(new Uri("pack://application:,,,/Resources/Graphics/Toolbar/Dark/ScrollStop.png")
                );

                _eraseLogImage =
                    new BitmapImage(new Uri("pack://application:,,,/Resources/Graphics/Toolbar/Dark/EraseLog.png")
                );

                _flagStrangerImage =
                    new BitmapImage(new Uri("pack://application:,,,/Resources/Graphics/Toolbar/Dark/FlagStranger.png")
                );

                _randomTopicImage =
                    new BitmapImage(new Uri("pack://application:,,,/Resources/Graphics/Toolbar/Dark/RandomTopic.png")
                );

                _disconnectImage =
                    new BitmapImage(new Uri("pack://application:,,,/Resources/Graphics/Toolbar/Dark/Disconnect.png")
                );

                _searchImage =
                    new BitmapImage(new Uri("pack://application:,,,/Resources/Graphics/Toolbar/Dark/Search.png")
                );
            }

            if (App.ColorSchemeLoader.CurrentSchemeUsesLightIcons)
            {
                _chatLogImage =
                    new BitmapImage(
                        new Uri("pack://application:,,,/Resources/Graphics/Toolbar/Light/ChatLog.png")
                    );

                _scrollStopImage =
                    new BitmapImage(new Uri("pack://application:,,,/Resources/Graphics/Toolbar/Light/ScrollStop.png")
                );

                _eraseLogImage =
                    new BitmapImage(new Uri("pack://application:,,,/Resources/Graphics/Toolbar/Light/EraseLog.png")
                );

                _flagStrangerImage =
                    new BitmapImage(new Uri("pack://application:,,,/Resources/Graphics/Toolbar/Light/FlagStranger.png")
                );

                _randomTopicImage =
                    new BitmapImage(new Uri("pack://application:,,,/Resources/Graphics/Toolbar/Light/RandomTopic.png")
                );

                _disconnectImage =
                    new BitmapImage(new Uri("pack://application:,,,/Resources/Graphics/Toolbar/Light/Disconnect.png")
                );

                _searchImage =
                    new BitmapImage(new Uri("pack://application:,,,/Resources/Graphics/Toolbar/Light/Search.png")
                );
            }

            CopyViewToggleButton.Content = new Image
            {
                Source = _chatLogImage,
                Height = 16,
                Width = 16,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                SnapsToDevicePixels = true
            };

            LogScrollingToggleButton.Content = new Image
            {
                Source = _scrollStopImage,
                Height = 16,
                Width = 16,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                SnapsToDevicePixels = true
            };

            ClearLogButton.Content = new Image
            {
                Source = _eraseLogImage,
                Height = 16,
                Width = 16,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                SnapsToDevicePixels = true
            };

            FlagStrangerButton.Content = new Image
            {
                Source = _flagStrangerImage,
                Height = 16,
                Width = 16,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                SnapsToDevicePixels = true
            };

            RequestRandomTopicButton.Content = new Image
            {
                Source = _randomTopicImage,
                Height = 16,
                Width = 16,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                SnapsToDevicePixels = true
            };

            DisconnectFromStrangerButton.Content = new Image
            {
                Source = _disconnectImage,
                Height = 16,
                Width = 16,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                SnapsToDevicePixels = true
            };

            SearchForStrangerButton.Content = new Image
            {
                Source = _searchImage,
                Height = 16,
                Width = 16,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                SnapsToDevicePixels = true
            };
        }
    }
}
