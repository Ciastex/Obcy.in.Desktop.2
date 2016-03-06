using System;
using System.Linq;

namespace ObcyInDesktop.Archive.Items
{
    class Message : Item
    {
        public bool Incoming { get; }
        public string Body { get; }

        public Message(bool incoming, long timestamp, string body) : base(timestamp)
        {
            Incoming = incoming;
            Body = body;
        }

        public static Message Parse(string input)
        {
            try
            {
                var array = input.Split('|');

                var type = (ItemType) int.Parse(array[0]);
                var timestamp = int.Parse(array[1]);

                var list = array.ToList();
                list.RemoveRange(0, 2);

                var body = string.Join("", list);

                if (type == ItemType.MyMessage)
                {
                    return new Message(false, timestamp, body);
                }
                if (type == ItemType.StrangerMessage)
                {
                    return new Message(true, timestamp, body);
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
            if (Incoming)
            {
                return $"{1}|{Timestamp}|{Body}";
            }
            return $"{0}|{Timestamp}|{Body}";
        }
    }
}
