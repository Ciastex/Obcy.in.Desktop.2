using System;
using System.Windows;
using ObcyInDesktop.Localization;
using ObcyInDesktop.Settings;

namespace ObcyInDesktop.UI.Controls.ChatUI
{
    public partial class MessageControl
    {
        public string Message
        {
            get { return MessageTextBlock.Text; }
            set { MessageTextBlock.Text = value; }
        }

        public string MessageTime
        {
            get { return TimeTextBlock.Text; }
            set { TimeTextBlock.Text = value; }
        }

        public MessageControl()
        {
            InitializeComponent();
        }

        public MessageControl(bool incoming, string message)
            : this()
        {

            if (incoming)
            {
                PaintIncomingMessage();
            }
            else
            {
                PaintOutgoingMessage();
            }

            MessageTime = $"{DateTime.Now.Hour.ToString("00")}:{DateTime.Now.Minute.ToString("00")}:{DateTime.Now.Second.ToString("00")}";

            Message = message;
        }

        public MessageControl(bool incoming, string message, DateTime creationTime)
            : this(incoming, message)
        {
            MessageTime = $"{creationTime.Hour.ToString("00")}:{creationTime.Minute.ToString("00")}:{creationTime.Second.ToString("00")}";
        }


        private void PaintIncomingMessage()
        {
            SenderTextBlock.Text = LocaleSelector.GetLocaleString("MessageControl_Sender_Stranger");

            MainGrid.Background = Colors.IncomingMessageBackgroundBrush;
            MainGrid.BorderBrush = Colors.IncomingMessageBorderBrush;

            if (SettingsSelector.GetConfigurationValue<bool>("Appearance_MessageMargins"))
            {
                Margin = new Thickness(0, 0, 20, 0);
            }
        }

        private void PaintOutgoingMessage()
        {
            SenderTextBlock.Text = LocaleSelector.GetLocaleString("MessageControl_Sender_You");

            MainGrid.Background = Colors.OutgoingMessageBackgroundBrush;
            MainGrid.BorderBrush = Colors.OutgoingMessageBorderBrush;

            if (SettingsSelector.GetConfigurationValue<bool>("Appearance_MessageMargins"))
            {
                Margin = new Thickness(20, 0, 0, 0);
            }
        }
    }
}
