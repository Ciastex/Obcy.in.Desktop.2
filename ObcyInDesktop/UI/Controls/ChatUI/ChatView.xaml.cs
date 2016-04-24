using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using ObcyInDesktop.Localization;
using ObcyInDesktop.Settings;
using ObcyProtoRev.Protocol;
using ObcyProtoRev.Protocol.Client;
using ObcyProtoRev.Protocol.Client.Identity;
using ObcyProtoRev.Protocol.Events;

namespace ObcyInDesktop.UI.Controls.ChatUI
{
    public partial class ChatView
    {
        private readonly Connection _connection;

        private DoubleAnimation _gridFadeOutAnimation;
        private DoubleAnimation _gridFadeInAnimation;
        private DoubleAnimation _chatBoxHeightUpAnimation;
        private DoubleAnimation _chatBoxHeightDownAnimation;
        
        private bool _typing;

        private readonly List<int> _encounteredStrangerList;

        public ChatView()
        {
            InitializeComponent();
            _encounteredStrangerList = new List<int>();
        }

        public ChatView(Connection connection)
            : this()
        {
            _connection = connection;

            InitializeAnimations();
            InitializeConnection();

            Connect();
        }

        public event EventHandler MessageSent;

        private void InitializeAnimations()
        {
            _gridFadeOutAnimation = new DoubleAnimation(1, 0, new Duration(new TimeSpan(0, 0, 0, 0, 200)));
            _gridFadeInAnimation = new DoubleAnimation(0, 1, new Duration(new TimeSpan(0, 0, 0, 0, 200)));
            _chatBoxHeightUpAnimation = new DoubleAnimation(ChatBox.Height, ChatBox.Height + 25, new Duration(new TimeSpan(0, 0, 0, 0, 200)));
            _chatBoxHeightDownAnimation = new DoubleAnimation(ChatBox.Height + 25, ChatBox.Height, new Duration(new TimeSpan(0, 0, 0, 0, 200)));

            _gridFadeOutAnimation.Completed += GridFadeOutAnimation_Completed;
            _chatBoxHeightDownAnimation.Completed += ChatBoxHeightDownAnimation_Completed;
        }

        private void InitializeConnection()
        {
            _connection.StrangerFound += Connection_StrangerFound;
            _connection.ConversationEnded += Connection_ConversationEnded;
            _connection.MessageReceived += Connection_MessageReceived;
            _connection.StrangerChatstateChanged += Connection_StrangerChatstateChanged;
            _connection.ConnectionAccepted += Connection_ConnectionAccepted;
            _connection.ServerClosedConnection += connection_ServerClosedConnection;
            _connection.SocketClosed += connection_SocketClosed;
            _connection.SocketError += Connection_SocketError;
        }

        private void connection_SocketClosed(object sender, SocketClosedEventArgs e)
        {
            ConnectionFailureCleanup();
        }

        private void connection_ServerClosedConnection(object sender, EventArgs e)
        {
            ServerDisconnectionCleanup();
        }

        public void ClearLog()
        {
            MessageLog.MessagePanel.Children.Clear();
            MessageLog.CopyView.Clear();
        }

        public void Connect()
        {
            _connection.Open();
        }

        public void DisconnectFromStranger()
        {
            if (_connection.IsStrangerConnected)
            {
                _connection.DisconnectStranger();
            }
            Application.Current.Dispatcher.Invoke(() => StatusTextBlock.Text = LocaleSelector.GetLocaleString("StatusIndicator_ConnectedToServer"));
        }

        public void FlagStranger()
        {
            if (_connection.IsStrangerConnected)
            {
                _connection.FlagStranger();
            }
        }

        public void RequestRandomTopic()
        {
            if (_connection.IsStrangerConnected)
            {
                _connection.RequestRandomTopic();
            }
        }

        public void SearchForStranger()
        {
            if (!_connection.IsStrangerConnected)
            {
                _connection.SearchForStranger((Location)SettingsSelector.GetConfigurationValue<int>("Voivodeship"));
            }
        }

        public void ToggleCopyView()
        {
            MessageLog.ToggleCopyView();
        }

        public void ToggleLogScrolling()
        {
            MessageLog.ScrollOnMessage = !MessageLog.ScrollOnMessage;
        }

        public void ToggleToolbar()
        {
            switch (ToolbarGrid.Visibility)
            {
                case Visibility.Visible:
                    ToolbarGrid.BeginAnimation(OpacityProperty, _gridFadeOutAnimation);
                    break;
                default:
                    ToolbarGrid.Visibility = Visibility.Visible;
                    ChatBox.BeginAnimation(HeightProperty, _chatBoxHeightDownAnimation);
                    break;
            }
        }

        public void SendMessage(string message)
        {
            _connection.SendMessage(message);
        }

        private void ConnectionFailureCleanup()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                DisableChatRelatedControls();
                
                StatusTextBlock.Text = LocaleSelector.GetLocaleString("StatusIndicator_ConnectionFailure");
            });
        }

        private void DisableChatRelatedControls()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ChatBox.IsEnabled = false;

                Toolbar.SearchForStrangerButton.IsEnabled = false;
                Toolbar.DisconnectFromStrangerButton.IsEnabled = false;
                Toolbar.FlagStrangerButton.IsEnabled = false;
                Toolbar.RequestRandomTopicButton.IsEnabled = false;
            });
        }

        private void ServerDisconnectionCleanup()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                DisableChatRelatedControls();
                StatusTextBlock.Text = LocaleSelector.GetLocaleString("StatusIndicator_ServerClosedConnection");
            });
        }

        private void ChatBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrWhiteSpace(ChatBox.Text) && _connection.IsStrangerConnected)
            {
                _connection.SendMessage(ChatBox.Text);

                if (SettingsSelector.GetConfigurationValue<bool>("Behavior_SendChatstate"))
                {
                    _connection.ReportChatstate(false);
                    _typing = false;
                }

                MessageLog.AddMessage(new Message(ChatBox.Text, -1, -1, MessageType.Chat), false);
                ChatBox.Clear();

                OnMessageSent();
            }
        }

        private void ChatBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            if(SettingsSelector.GetConfigurationValue<bool>("Behavior_SendChatstate"))
            {
                if (e.Key != Key.Enter && _connection.IsStrangerConnected)
                {
                    if (e.Key == Key.Back && _typing)
                    {
                        _connection.ReportChatstate(false);
                        _typing = false;
                    }

                    if ((char.IsLetterOrDigit((char) e.Key) || char.IsPunctuation((char) e.Key) ||
                         char.IsSeparator((char) e.Key)) && !_typing)
                    {
                        _connection.ReportChatstate(true);
                        _typing = true;
                    }
                }
            }
        }

        private void Connection_ConnectionAccepted(object sender, ConnectionAcceptedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => StatusTextBlock.Text = LocaleSelector.GetLocaleString("StatusIndicator_ConnectedToServer"));
        }

        private void Connection_ConversationEnded(object sender, ConversationEndedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (!e.DisconnectInfo.IsReminder)
                {
                    MessageLog.AddPresence(false);
                }

                ChatBox.Clear();
                ChatBox.IsEnabled = false;

                StatusTextBlock.Text = LocaleSelector.GetLocaleString("StatusIndicator_ConnectedToServer");

                Toolbar.DisconnectFromStrangerButton.IsEnabled = false;
                Toolbar.SearchForStrangerButton.IsEnabled = true;
                Toolbar.FlagStrangerButton.IsEnabled = false;
                Toolbar.RequestRandomTopicButton.IsEnabled = false;
            });
        }

        private void Connection_MessageReceived(object sender, MessageEventArgs e)
        {
            if (e.Message.Type == MessageType.Chat)
            {
                Application.Current.Dispatcher.Invoke(() => MessageLog.AddMessage(e.Message, true));
                Application.Current.Dispatcher.Invoke(() => StatusTextBlock.Text = LocaleSelector.GetLocaleString("StatusIndicator_StrangerIdle"));
            }
            else if (e.Message.Type == MessageType.Topic)
            {
                Application.Current.Dispatcher.Invoke(() => MessageLog.AddTopic(e.Message));
            }
        }

        private void Connection_SocketError(object sender, ErrorEventArgs e)
        {
            _connection.Close();
        }

        private void Connection_StrangerChatstateChanged(object sender, ChatstateEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                StatusTextBlock.Text = e.ChatState == ChatState.Writing
                    ? LocaleSelector.GetLocaleString("StatusIndicator_StrangerTyping")
                    : LocaleSelector.GetLocaleString("StatusIndicator_StrangerIdle");
            });
        }

        private void Connection_StrangerFound(object sender, StrangerFoundEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ChatBox.IsEnabled = true;
                ChatBox.Focus();

                MessageLog.AddPresence(true, e.StrangerInfo.FlaggedAsUnpleasant);
                StatusTextBlock.Text = LocaleSelector.GetLocaleString("StatusIndicator_StrangerIdle");

                Toolbar.FlagStrangerButton.IsEnabled = true;
                Toolbar.DisconnectFromStrangerButton.IsEnabled = true;
                Toolbar.RequestRandomTopicButton.IsEnabled = true;
            });
        }

        private void GridFadeOutAnimation_Completed(object sender, EventArgs e)
        {
            ToolbarGrid.Visibility = Visibility.Hidden;
            ChatBox.BeginAnimation(HeightProperty, _chatBoxHeightUpAnimation);
        }

        private void ChatBoxHeightDownAnimation_Completed(object sender, EventArgs e)
        {
            ToolbarGrid.BeginAnimation(OpacityProperty, _gridFadeInAnimation);
        }

        protected virtual void OnMessageSent()
        {
            var handler = MessageSent;
            handler?.Invoke(this, EventArgs.Empty);
        }

        private void MessageLog_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left &&
                SettingsSelector.GetConfigurationValue<bool>("Behavior_CopyViewOnLogDoubleClick"))
            {
                MessageLog.ToggleCopyView();
                Toolbar.CopyViewToggleButton.IsChecked = !Toolbar.CopyViewToggleButton.IsChecked;
            }
        }
    }
}
