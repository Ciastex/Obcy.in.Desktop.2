using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Timers;
using ObcyInDesktop.Filesystem;
using ObcyProtoRev.Protocol;
using ObcyProtoRev.Protocol.Client;
using ObcyProtoRev.Protocol.Events;

namespace ObcyInDesktop.Statistics
{
    public class StatsManager
    {
        private readonly Connection _connection;
        private Timer _conversationTimer;
        private bool _kilometerAddedInCurrentConversation;
        private bool _recordingStats;

        public StatsManager(Connection connection)
        {
            _connection = connection;
            Statistics = new Stats();

            CreateConversationTimer();
            InitializeConnectionEvents();

            _recordingStats = true;
        }

        public event EventHandler BestConversationTimeChanged;
        public event EventHandler ConversationCountChanged;
        public event EventHandler CurrentConversationTimeChanged;
        public event EventHandler KilometersCountChanged;
        public event EventHandler ReceivedMessagesCountChanged;
        public event EventHandler SentMessagesCountChanged;

        public Stats Statistics { get; private set; }
        public TimeSpan CurrentConversationTime { get; private set; }

        public void AddSentMessage()
        {
            Statistics.MessagesSent += 1;
            SentMessagesCountChanged?.Invoke(this, EventArgs.Empty);
        }

        public void LoadStats(string filePath)
        {
            using (var deflateStream = new DeflateStream(File.OpenRead(filePath), CompressionMode.Decompress))
            {
                var binaryFormatter = new BinaryFormatter();
                var stats = binaryFormatter.Deserialize(deflateStream) as Stats;

                if (stats != null)
                {
                    Statistics = stats;
                    Reload();
                }
            }
        }

        public void SaveStats(string filePath)
        {
            using (var deflateStream = new DeflateStream(File.OpenWrite(filePath), CompressionMode.Compress))
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(deflateStream, Statistics);
            }
        }

        public void ClearStats()
        {
            Statistics.BestConversationTime = TimeSpan.Zero;
            Statistics.ConversationCount = 0;
            Statistics.KilometersCount = 0;
            Statistics.MessagesReceived = 0;
            Statistics.MessagesSent = 0;

            SaveStats(
                Path.Combine(DirectoryGuard.DataDirectory, "statistics.dat")
            );

            Reload();
        }

        public void StopRecording()
        {
            _conversationTimer.Stop();
            _recordingStats = false;
        }

        private void CreateConversationTimer()
        {
            _conversationTimer = new Timer(1000);
            _conversationTimer.Elapsed += conversationTimer_Elapsed;
        }

        private void InitializeConnectionEvents()
        {
            _connection.StrangerFound += connection_StrangerFound;
            _connection.MessageReceived += connection_MessageReceived;
            _connection.ConversationEnded += connection_ConversationEnded;
        }

        private void connection_ConversationEnded(object sender, ConversationEndedEventArgs e)
        {
            if (!_recordingStats)
                return;

            if (!e.DisconnectInfo.IsReminder)
            {
                _conversationTimer.Stop();

                CurrentConversationTime = TimeSpan.Zero;
                CurrentConversationTimeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void connection_MessageReceived(object sender, MessageEventArgs e)
        {
            if (!_recordingStats)
                return;

            if (e.Message.Type == MessageType.Chat)
            {
                if (Regex.IsMatch(e.Message.Body, @"^[Kk]\/?[Mm]\b"))
                {
                    if (!_kilometerAddedInCurrentConversation)
                    {
                        Statistics.KilometersCount += 1;
                        KilometersCountChanged?.Invoke(this, EventArgs.Empty);

                        _kilometerAddedInCurrentConversation = true;
                    }
                }
                Statistics.MessagesReceived += 1;
                ReceivedMessagesCountChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void connection_StrangerFound(object sender, StrangerFoundEventArgs e)
        {
            if (!_recordingStats)
                return;

            _kilometerAddedInCurrentConversation = false;

            Statistics.ConversationCount += 1;
            ConversationCountChanged?.Invoke(this, EventArgs.Empty);

            _conversationTimer.Start();
        }

        private void conversationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!_recordingStats)
                return;

            CurrentConversationTime = CurrentConversationTime.Add(new TimeSpan(0, 0, 0, 1));
            CurrentConversationTimeChanged?.Invoke(this, EventArgs.Empty);

            if (CurrentConversationTime > Statistics.BestConversationTime)
            {
                Statistics.BestConversationTime = CurrentConversationTime;
                BestConversationTimeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void Reload()
        {
            BestConversationTimeChanged?.Invoke(this, EventArgs.Empty);
            ConversationCountChanged?.Invoke(this, EventArgs.Empty);
            KilometersCountChanged?.Invoke(this, EventArgs.Empty);
            ReceivedMessagesCountChanged?.Invoke(this, EventArgs.Empty);
            SentMessagesCountChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}