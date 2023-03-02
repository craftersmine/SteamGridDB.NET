using System;
using System.Runtime.Serialization;

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

        /// <summary>
        /// Instantiates new instance of <see cref="SteamGridDbException"/>
        /// </summary>
        public SteamGridDbException()
        {
        }

        /// <summary>
        /// Instantiates new instance of <see cref="SteamGridDbException"/>
        /// </summary>
        /// <param name="message"></param>
        public SteamGridDbException(string message) : base(message)
        {
        }

        /// <summary>
        /// Instantiates new instance of <see cref="SteamGridDbException"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public SteamGridDbException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Instantiates new instance of <see cref="SteamGridDbException"/>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
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
        /// <summary>
        /// When unauthorized access to API made
        /// </summary>
        Unauthorized = 401,
        /// <summary>
        /// When resource not found on server
        /// </summary>
        NotFound = 404,
        /// <summary>
        /// When request to server constructed incorrectly
        /// </summary>
        BadRequest = 400,
        /// <summary>
        /// When made request is forbidden on server
        /// </summary>
        Forbidden = 403,
        /// <summary>
        /// When you are rate limited on the server
        /// </summary>
        RateLimited = 429,
        /// <summary>
        /// When unknown error occurred
        /// </summary>
        Unknown = 0
    }
}
