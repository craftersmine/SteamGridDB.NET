using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace craftersmine.SteamGridDBNet.Exceptions
{
    /// <summary>
    /// The exception that is thrown when image failed to load from specified URL
    /// </summary>
    [Serializable]
    public class SteamGridDbImageException : Exception
    {
        /// <summary>
        /// Gets an SteamGridDB response exception type
        /// </summary>
        public ExceptionType ExceptionType { get; private set; }

        /// <summary>
        /// Instantiates new instance of <see cref="SteamGridDbImageException"/>
        /// </summary>
        /// <param name="type"></param>
        public SteamGridDbImageException(ExceptionType type)
        {
            ExceptionType = type;
        }

        /// <summary>
        /// Instantiates new instance of <see cref="SteamGridDbImageException"/>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public SteamGridDbImageException(ExceptionType type, string message) : base(message)
        {
            ExceptionType = type;
        }

        /// <summary>
        /// Instantiates new instance of <see cref="SteamGridDbImageException"/>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public SteamGridDbImageException(ExceptionType type, string message, Exception inner) : base(message, inner)
        {
            ExceptionType = type;
        }

        /// <summary>
        /// Instantiates new instance of <see cref="SteamGridDbImageException"/>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected SteamGridDbImageException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }

    internal sealed class SteamGridDbImageErrorResponse
    {
        [JsonProperty("code")]
        public string Code { get; private set; }
        [JsonProperty("message")]
        public string Message { get; private set; }
        [JsonProperty("code")]
        public int Status { get; private set; }
    }
}
