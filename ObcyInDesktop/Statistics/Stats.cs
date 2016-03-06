using System;
using System.Runtime.Serialization;

namespace ObcyInDesktop.Statistics
{
    [Serializable]
    public class Stats
    {
        public Stats()
        {
        }

        public int ConversationCount { get; set; }
        public int KilometersCount { get; set; }
        public int MessagesSent { get; set; }
        public int MessagesReceived { get; set; }
        public TimeSpan BestConversationTime { get; set; }

        public Stats(SerializationInfo serializationInfo, StreamingContext streamingContext)
        {
        }
    }
}
