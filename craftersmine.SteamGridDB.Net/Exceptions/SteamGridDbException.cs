
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.SteamGridDBNet.Exceptions
{
    [Serializable]
    public class SteamGridDbException : Exception
    {
        public ExceptionType ExceptionType { get; set; }
        public string[]? SteamGridDbErrorMessages { get; set; }

        public SteamGridDbException()
        {
        }

        public SteamGridDbException(string message) : base(message)
        {
        }

        public SteamGridDbException(string message, Exception inner) : base(message, inner)
        {
        }

        protected SteamGridDbException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }

    public enum ExceptionType
    {
        Unauthorized = 401,
        NotFound = 404,
        BadRequest = 400,
        Forbidden = 403,
        Unknown = 0
    }
}
