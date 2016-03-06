using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using ObcyInDesktop.Archive;
using ObcyInDesktop.Archive.Items;
using ObcyInDesktop.Filesystem;
using ObcyInDesktop.Localization;
using ObcyInDesktop.UI;
using ObcyProtoRev.Protocol.Client;
using Message = ObcyInDesktop.Archive.Items.Message;

namespace ObcyInDesktop.Windows
{
    public partial class ArchiveWindow
    {
        private readonly ColorAnimation _glowFadeInAnimation;
        private readonly ColorAnimation _glowFadeOutAnimation;

        private readonly Storyboard _borderActivateStoryboard;
        private readonly Storyboard _borderDeactivateStoryboard;

        private Thread _fillThread;

        public ArchiveWindow()
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

            Title = LocaleSelector.GetLocaleString("ArchiveWindow_Title");

            LoadAllArchives();
        }

        private void LoadAllArchives()
        {
            var di = new DirectoryInfo(DirectoryGuard.ArchiveDirectory);
            var dirinfos = di.GetDirectories();

            foreach (var info in dirinfos)
            {
                ConversationFolders.Items.Add(
                    new ListBoxItem
                    {
                        Content = info.Name
                    }
               );
            }
        }

        private void ArchiveWindow_OnActivated(object sender, EventArgs e)
        {
            var glow = (DropShadowEffect)FindName("BorderGlow");
            glow?.BeginAnimation(DropShadowEffect.ColorProperty, _glowFadeInAnimation);

            _borderActivateStoryboard.Begin();
        }

        private void ArchiveWindow_OnDeactivated(object sender, EventArgs e)
        {
            var glow = (DropShadowEffect)FindName("BorderGlow");
            glow?.BeginAnimation(DropShadowEffect.ColorProperty, _glowFadeOutAnimation);

            _borderDeactivateStoryboard.Begin();
        }

        private void ConversationFolders_OnSelected(object sender, SelectionChangedEventArgs e)
        {
            ConversationFiles.Items.Clear();

            var di = Directory.GetFiles(
                Path.Combine(
                    DirectoryGuard.ArchiveDirectory,
                    ((ListBoxItem)ConversationFolders.SelectedItem).Content.ToString()
                )
            );

            foreach (var path in di)
            {
                var name = Path.GetFileName(path);

                if (name != null)
                {
                    ConversationFiles.Items.Add(
                        new ListBoxItem
                        {
                            Content = name.Substring(0, name.Length - 4).Replace('.', ':')
                        }
                    );
                }
            }
        }

        private void ConversationFiles_OnSelected(object sender, SelectionChangedEventArgs e)
        {
            MessageLog.MessagePanel.Children.Clear();

            if (ConversationFiles.SelectedItem == null)
            {
                return;
            }

            var path = Path.Combine(
                DirectoryGuard.ArchiveDirectory,
                ((ListBoxItem)ConversationFolders.SelectedItem).Content.ToString(),
                ((ListBoxItem)ConversationFiles.SelectedItem).Content.ToString().Replace(':', '.') + ".log"
            );

            _fillThread = new Thread(() => FillMessageLog(path));
            _fillThread.SetApartmentState(ApartmentState.STA);
            _fillThread.Start();
        }

        private void FillMessageLog(string path)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                using (var sr = new StreamReader(path))
                {
                    var array = sr.ReadToEnd().Split('\n');

                    foreach (var s in array)
                    {
                        object item;
                        try
                        {
                            item = Presence.Parse(s);
                        }
                        catch
                        {
                            try
                            {
                                item = Message.Parse(s);
                            }
                            catch
                            {
                                try
                                {
                                    item = Topic.Parse(s);
                                }
                                catch
                                {
                                    return;
                                }
                            }
                        }

                        if (item is Presence)
                        {
                            var p = item as Presence;
                            MessageLog.AddPresence(p.Connect);
                        }

                        if (item is Message)
                        {
                            var m = item as Message;
                            MessageLog.AddMessage(
                                new ObcyProtoRev.Protocol.Client.Message(
                                    m.Body,
                                    -1,
                                    -1,
                                    MessageType.Chat
                                    ),
                                m.Incoming,
                                m.Timestamp.FromUnixTimestamp()
                                );
                        }

                        if (item is Topic)
                        {
                            var t = item as Topic;
                            MessageLog.AddTopic(
                                new ObcyProtoRev.Protocol.Client.Message(
                                    t.Body,
                                    0,
                                    0,
                                    MessageType.Topic
                                    )
                                );
                        }
                    }
                }
            });
        }
    }
}
