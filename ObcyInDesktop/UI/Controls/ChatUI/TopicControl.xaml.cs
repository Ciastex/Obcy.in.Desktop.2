namespace ObcyInDesktop.UI.Controls.ChatUI
{
    public partial class TopicControl
    {
        public string Topic
        {
            get { return TopicBodyTextBlock.Text; }
            set { TopicBodyTextBlock.Text = value; }
        }

        public TopicControl()
        {
            InitializeComponent();
        }

        public TopicControl(string body) : this()
        {
            Topic = body;
        }
    }
}
