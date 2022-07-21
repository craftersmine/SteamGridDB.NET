
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.SteamGridDBNet.Exceptions
{
    /// <summary>
    /// The exception that is thrown when generic SteamGridDB or library error occurred
    /// </summary>
    [Serializable]
    public class SteamGridDbException : Exception
    {
        /// <summary>
        /// Gets a type of exception occurred
        /// </summary>
        public ExceptionType ExceptionType { get; set; }
        /// <summary>
        /// Gets an array of SteamGridDB API response error messages, when request is not succeeded
        /// </summary>
        public string[] SteamGridDbErrorMessages { get; set; }

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

    /// <summary>
    /// Contains types of exceptions
    /// </summary>
    public enum ExceptionType
    {
        Unauthorized = 401,
        NotFound = 404,
        BadRequest = 400,
        Forbidden = 403,
        Unknown = 0
    }
}
