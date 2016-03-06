using System.Windows.Input;
using ObcyInDesktop.UI.Controls.ChatUI;

namespace ObcyInDesktop.UI.Commands
{
    public static class ChatViewCommands
    {
        public static ICommand ClearLogCommand = new RelayCommand(o =>
        {
            var chatView = o as ChatView;

            if (chatView == null)
                return;

            chatView.ClearLog();
        });

        public static ICommand ConnectCommand = new RelayCommand(o =>
        {
            ((ChatView)o).Connect();
        });

        public static ICommand DisconnectFromStrangerCommand = new RelayCommand(o =>
        {
            var chatView = o as ChatView;

            if (chatView == null)
                return;

            chatView.DisconnectFromStranger();
        });

        public static ICommand FlagStranger = new RelayCommand(o =>
        {
            var chatView = o as ChatView;

            if (chatView == null)
                return;

            chatView.FlagStranger();
        });

        public static ICommand RequestRandomTopicCommand = new RelayCommand(o =>
        {
            var chatView = o as ChatView;

            if (chatView == null)
                return;

            chatView.RequestRandomTopic();
        });

        public static ICommand SearchForStrangerCommand = new RelayCommand(o =>
        {
            var chatView = o as ChatView;

            if (chatView == null)
                return;

            chatView.DisconnectFromStranger();
            chatView.SearchForStranger();
        });

        public static ICommand ToggleCopyViewCommand = new RelayCommand(o =>
        {
            var chatView = o as ChatView;

            if (chatView == null)
                return;

            chatView.ToggleCopyView();
        });

        public static ICommand ToggleLogScrollingCommand = new RelayCommand(o =>
        {
            var chatView = o as ChatView;

            if (chatView == null)
                return;

            chatView.ToggleLogScrolling();
        });
    }
}
