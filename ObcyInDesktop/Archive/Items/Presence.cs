using System;

namespace ObcyInDesktop.Archive.Items
{
    class Presence : Item
    {
        public bool Connect { get; }

        public Presence(bool connect, long timestamp) : base(timestamp)
        {
            Connect = connect;
        }

        public static Presence Parse(string input)
        {
            try
            {
                var array = input.Split('|');

                var type = (ItemType)int.Parse(array[0]);
                var timestamp = int.Parse(array[1]);

                if (type == ItemType.ConnectPresence)
                {
                    return new Presence(true, timestamp);
                }
                if (type == ItemType.DisconnectPresence)
                {
                    return new Presence(false, timestamp);
                }
                throw new Exception("Incorrect item type.");
            }
            catch (Exception ex)
            {
                throw new FormatException("Input string was incorrectly formatted.", ex);
            }
        }

        public override string ToString()
        {
            if (Connect)
            {
                return $"{3}|{Timestamp}";
            }
            return $"{4}|{Timestamp}";
        }
    }
}
