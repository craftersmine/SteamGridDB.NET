using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.SteamGridDBNet
{
    internal static class EnumHelper
    {
        public static bool MoreThanOneFlag(this Enum flag) => (Convert.ToInt32(flag) & (Convert.ToInt32(flag) - 1)) != 0;
    }
}
