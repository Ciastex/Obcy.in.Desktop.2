using System;
using System.Linq;

namespace ObcyInDesktop.Archive.Items
{
    class Topic : Item
    {
        public string Body { get; }

        public Topic(long timestamp, string body) : base(timestamp)
        {
            Body = body;
        }

        public static Topic Parse(string input)
        {
            try
            {
                var array = input.Split('|');

                var type = (ItemType)int.Parse(array[0]);
                var timestamp = int.Parse(array[1]);

                var list = array.ToList();
                list.RemoveRange(0, 2);

                var body = string.Join("", list);

                if (type == ItemType.Topic)
                {
                    return new Topic(timestamp, body);
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
            return $"{2}|{Timestamp}|{Body}";
        }
    }
}
