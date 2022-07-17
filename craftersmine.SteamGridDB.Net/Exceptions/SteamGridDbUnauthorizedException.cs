using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.SteamGridDBNet.Exceptions
{
    public class SteamGridDbUnauthorizedException : SteamGridDbException
    {
        public SteamGridDbUnauthorizedException(string message) : base(message)
        {

        }
    }
}
