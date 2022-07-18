using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.SteamGridDBNet.Exceptions
{
    public class SteamGridDbForbiddenException : SteamGridDbException
    {
        public SteamGridDbForbiddenException(string message) : base(message)
        {

        }
    }
}
