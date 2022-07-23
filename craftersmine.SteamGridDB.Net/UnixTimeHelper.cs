using System;

namespace craftersmine.SteamGridDBNet
{
    internal static class UnixTimeHelper
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1);

        public static DateTime FromUnixTime(this long val)
        {
            return UnixEpoch.AddSeconds(val);
        }

        public static long ToUnixTime(this DateTime val)
        {
            if (val == DateTime.MinValue)
                return 0;

            var delta = val - UnixEpoch;

            if (delta.TotalSeconds < 0)
                throw new ArgumentOutOfRangeException(nameof(val), val, Resources.Resources.Exception_InvalidDateTime);

            return (long) delta.TotalSeconds;
        }
    }
}
