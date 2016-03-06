namespace ObcyInDesktop.Archive.Items
{
    public class Item
    {
        public long Timestamp { get; private set; }

        public Item(long timestamp)
        {
            Timestamp = timestamp;
        }
    }
}
