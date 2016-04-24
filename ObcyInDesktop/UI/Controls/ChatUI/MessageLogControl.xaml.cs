using System;
using System.Windows;
using ObcyProtoRev.Protocol.Client;

namespace ObcyInDesktop.UI.Controls.ChatUI
{
    public partial class MessageLogControl
    {
        public bool ScrollOnMessage { get; set; }

        public MessageLogControl()
        {
            InitializeComponent();
            ScrollOnMessage = true;
        }

        public void AddMessage(Message message, bool incoming)
        {
            var mc = new MessageControl(incoming, message.Body);
            MessagePanel.Children.Add(mc);

            if (ScrollOnMessage)
            {
                ScrollView.ScrollToEnd();
            }
        }

        public void AddMessage(Message message, bool incoming, DateTime creationTime)
        {
            var mc = new MessageControl(incoming, message.Body, creationTime);
            MessagePanel.Children.Add(mc);

            if (ScrollOnMessage)
            {
                ScrollView.ScrollToEnd();
            }
        }

        public void AddTopic(Message message)
        {
            var tc = new TopicControl(message.Body);
            MessagePanel.Children.Add(tc);

            if (ScrollOnMessage)
            {
                ScrollView.ScrollToEnd();
            }
        }

        public void AddPresence(bool started)
        {
            var pc = new PresenceControl(started);
            MessagePanel.Children.Add(pc);

            if (ScrollOnMessage)
            {
                ScrollView.ScrollToEnd();
            }
        }

        public void AddPresence(bool started, bool alreadyTalkedWith)
        {
            var pc = new PresenceControl(started, alreadyTalkedWith);
            MessagePanel.Children.Add(pc);

            if (ScrollOnMessage)
            {
                ScrollView.ScrollToEnd();
            }
        }

        public void ToggleCopyView()
        {
            switch (CopyView.Visibility)
            {
                case Visibility.Visible:
                    CopyView.Visibility = Visibility.Collapsed;
                    CopyViewScrollViewer.Visibility = Visibility.Collapsed;
                    break;
                default:
                    CopyView.ScrollToEnd();
                    CopyView.Visibility = Visibility.Visible;
                    CopyViewScrollViewer.Visibility = Visibility.Visible;
                    break;
            }
            FillCopyView();
        }

        private void FillCopyView()
        {
            CopyView.Clear();

            foreach (UIElement e in MessagePanel.Children)
            {
                if (e is TopicControl)
                {
                    var tc = e as TopicControl;
                    CopyView.AppendText($"{tc.TopicTitleTextBlock.Text}: {tc.Topic}\n");
                }

                if (e is PresenceControl)
                {
                    var pc = e as PresenceControl;
                    CopyView.AppendText($"[{pc.CreationTime}] {pc.PresenceTextBlock.Text}\n");
                }

                if (e is MessageControl)
                {
                    var mc = e as MessageControl;
                    CopyView.AppendText($"[{mc.TimeTextBlock.Text}] {mc.SenderTextBlock.Text}: {mc.MessageTextBlock.Text}\n");
                }
            }
        }
    }
}
