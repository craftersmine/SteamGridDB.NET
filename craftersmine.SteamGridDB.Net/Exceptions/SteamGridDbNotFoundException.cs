using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.SteamGridDBNet.Exceptions
{
    public class SteamGridDbNotFoundException : SteamGridDbException
    {
        public SteamGridDbNotFoundException(string message) : base(message)
        {

        }
    }
}
