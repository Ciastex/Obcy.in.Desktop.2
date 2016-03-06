using System.Windows;
using ObcyProtoRev.Protocol.Client;
using ObcyProtoRev.Protocol.Events;

namespace ObcyInDesktop.Archive
{
    class ConversationEventListener
    {
        private ArchiveManager ArchiveManager { get; }

        public ConversationEventListener(ArchiveManager archiveManager)
        {
            ArchiveManager = archiveManager;

            App.Connection.StrangerFound += Connection_StrangerFound;
            App.Connection.ConversationEnded += Connection_ConversationEnded;
            App.Connection.MessageReceived += Connection_MessageReceived;
            App.Connection.MessageSent += Connection_MessageSent;

            Application.Current.Exit += Current_Exit;
        }

        private void Connection_StrangerFound(object sender, StrangerFoundEventArgs e)
        {
            ArchiveManager.ConversationStart();
        }

        private void Connection_ConversationEnded(object sender, ConversationEndedEventArgs e)
        {
            if (!e.DisconnectInfo.IsReminder)
            {
                ArchiveManager.ConversationEndCleanup();
            }
        }

        private void Connection_MessageReceived(object sender, MessageEventArgs e)
        {
            switch (e.Message.Type)
            {
                case MessageType.Service:
                    return;
                case MessageType.Chat:
                    ArchiveManager.AddMessage(true, e.Message.Body);
                    break;
                case MessageType.Topic:
                    ArchiveManager.AddTopic(e.Message.Body);
                    break;
            }
        }

        private void Connection_MessageSent(object sender, MessageEventArgs e)
        {
            ArchiveManager.AddMessage(false, e.Message.Body);
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            ArchiveManager.ConversationEndCleanup();
        }
    }
}
