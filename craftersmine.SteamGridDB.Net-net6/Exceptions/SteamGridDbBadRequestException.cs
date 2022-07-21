using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.SteamGridDBNet.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a bad request is returned by SteamGridDB API
    /// </summary>
    public class SteamGridDbBadRequestException : SteamGridDbException
    {
        public SteamGridDbBadRequestException(string message) : base(message)
        {

        }
    }
}
