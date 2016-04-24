using System;
using System.Windows;
using ObcyInDesktop.Localization;

namespace ObcyInDesktop.UI.Controls.ChatUI
{
    public partial class PresenceControl
    {
        public DateTime CreationTime { get; private set; }
        public PresenceControl()
        {
            InitializeComponent();
            CreationTime = DateTime.Now;
        }

        public PresenceControl(bool started) : this()
        {
            if (started)
            {
                PresenceTextBlock.Text = LocaleSelector.GetLocaleString("PresenceControl_ConversationStarted");
                PresenceTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
            }
            else
            {
                PresenceTextBlock.Text = LocaleSelector.GetLocaleString("PresenceControl_ConversationEnded");
                PresenceTextBlock.HorizontalAlignment = HorizontalAlignment.Right;
            }
        }

        public PresenceControl(bool started, bool flaggedAsUnpleasant) : this()
        {
            if (started)
            {
                if (flaggedAsUnpleasant)
                {
                    PresenceTextBlock.Text = LocaleSelector.GetLocaleString("PresenceControl_ConversationStartedButUnpleasant");
                    PresenceTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
                }
                else
                {
                    PresenceTextBlock.Text = LocaleSelector.GetLocaleString("PresenceControl_ConversationStarted");
                    PresenceTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
                }
            }
            else
            {
                PresenceTextBlock.Text = LocaleSelector.GetLocaleString("PresenceControl_ConversationEnded");
                PresenceTextBlock.HorizontalAlignment = HorizontalAlignment.Right;
            }
        }
    }
}
