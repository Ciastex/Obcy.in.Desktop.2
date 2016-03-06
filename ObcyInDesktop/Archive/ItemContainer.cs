using System.Collections.Generic;
using System.Text;
using ObcyInDesktop.Archive.Items;

namespace ObcyInDesktop.Archive
{
    public class ItemContainer : List<Item>
    {
        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var item in this)
            {
                if (item is Presence)
                {
                    var presence = item as Presence;
                    builder.AppendFormat("{0}\n", presence);
                }

                if (item is Message)
                {
                    var message = item as Message;
                    builder.AppendFormat("{0}\n", message);
                }

                if (item is Topic)
                {
                    var topic = item as Topic;
                    builder.AppendFormat("{0}\n", topic);
                }
            }
            return builder.ToString();
        }
    }
}
