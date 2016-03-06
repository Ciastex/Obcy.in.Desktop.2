using System;
using System.IO;
using ObcyInDesktop.Archive.Items;
using ObcyInDesktop.Filesystem;

namespace ObcyInDesktop.Archive
{
    public class ArchiveManager
    {
        private ConversationEventListener Listener { get; set; }
        public ItemContainer CurrentArchiveItems { get; private set; }

        private int MessageCount { get; set; }

        private string CurrentDirectory { get; }
        private string CurrentArchiveFileName { get; set; }

        private string CurrentArchiveFilePath => Path.Combine(CurrentDirectory, CurrentArchiveFileName);

        public ArchiveManager()
        {
            var archiveDirectory = $"{DateTime.Now.Day.ToString("00")}-{DateTime.Now.Month.ToString("00")}-{DateTime.Now.Year.ToString("0000")}";
            CurrentDirectory = Path.Combine(DirectoryGuard.ArchiveDirectory, archiveDirectory);

            if (!Directory.Exists(CurrentDirectory))
            {
                Directory.CreateDirectory(CurrentDirectory);
            }
            Listener = new ConversationEventListener(this);
        }

        public void AddMessage(bool incoming, string body)
        {
            CurrentArchiveItems.Add(
                new Message(incoming, DateTime.Now.ToUnixTimestamp(), body)
            );
            MessageCount += 1;
            SaveCurrentArchiveFile();
        }

        public void AddTopic(string body)
        {
            CurrentArchiveItems.Add(
                new Topic(DateTime.Now.ToUnixTimestamp(), body)
            );
            SaveCurrentArchiveFile();
        }

        public void ConversationStart()
        {
            MessageCount = 0;
            CurrentArchiveItems = new ItemContainer();

            CurrentArchiveFileName = string.Format(
                "{0}.{1}.{2}.{3}.log",
                DateTime.Now.Hour.ToString("00"),
                DateTime.Now.Minute.ToString("00"),
                DateTime.Now.Second.ToString("00"),
                DateTime.Now.Millisecond.ToString("0000")
            );
            
            File.Create(
                CurrentArchiveFilePath
            ).Dispose();

            AddPresence(true);
            SaveCurrentArchiveFile();
        }

        public void ConversationEndCleanup()
        {
            AddPresence(false);
            SaveCurrentArchiveFile();
        }

        public void EraseArchives()
        {
            var di = new DirectoryInfo(DirectoryGuard.ArchiveDirectory);

            foreach (var dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        private void AddPresence(bool connect)
        {
            CurrentArchiveItems.Add(
                new Presence(connect, DateTime.Now.ToUnixTimestamp())
            );
            SaveCurrentArchiveFile();
        }

        private void SaveCurrentArchiveFile()
        {
            if (App.CanWriteToCurrentFolder)
            {
                using (var sw = new StreamWriter(CurrentArchiveFilePath, false))
                {
                    sw.WriteLine(CurrentArchiveItems.ToString());
                }
            }
        }
    }
}
