using System.Windows.Input;
using ObcyInDesktop.UI.Controls.ChatUI;

namespace ObcyInDesktop.UI.Commands
{
    public static class ChatViewCommands
    {
        public static readonly ICommand ClearLogCommand = new RelayCommand(o =>
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

        public static readonly ICommand DisconnectFromStrangerCommand = new RelayCommand(o =>
        {
            var chatView = o as ChatView;

            if (chatView == null)
                return;

            chatView.DisconnectFromStranger();
        });

        public static readonly ICommand FlagStranger = new RelayCommand(o =>
        {
            var chatView = o as ChatView;

            if (chatView == null)
                return;

            chatView.FlagStranger();
        });

        public static readonly ICommand RequestRandomTopicCommand = new RelayCommand(o =>
        {
            var chatView = o as ChatView;

            if (chatView == null)
                return;

            chatView.RequestRandomTopic();
        });

        public static readonly ICommand SearchForStrangerCommand = new RelayCommand(o =>
        {
            var chatView = o as ChatView;

            if (chatView == null)
                return;

            chatView.DisconnectFromStranger();
            chatView.SearchForStranger();
        });

        public static readonly ICommand ToggleCopyViewCommand = new RelayCommand(o =>
        {
            var chatView = o as ChatView;

            if (chatView == null)
                return;

            chatView.ToggleCopyView();
        });

        public static readonly ICommand ToggleLogScrollingCommand = new RelayCommand(o =>
        {
            var chatView = o as ChatView;

            if (chatView == null)
                return;

            chatView.ToggleLogScrolling();
        });
    }
}
