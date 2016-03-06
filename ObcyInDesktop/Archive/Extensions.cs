using System;

namespace ObcyInDesktop.Archive
{
    public static class Extensions
    {
        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            var timeSpan = (dateTime - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }

        public static DateTime FromUnixTimestamp(this long timestamp)
        {
            var unixDateStart = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            var currentTime = unixDateStart.AddSeconds(timestamp);
            return currentTime;
        }
    }
}
