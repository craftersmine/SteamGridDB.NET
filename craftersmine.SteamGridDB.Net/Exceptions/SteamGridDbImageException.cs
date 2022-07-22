using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace craftersmine.SteamGridDBNet.Exceptions
{
    /// <summary>
    /// The exception that is thrown when image failed to load from specified URL
    /// </summary>
    [Serializable]
    public class SteamGridDbImageException : Exception
    {
        public ExceptionType ExceptionType { get; private set; }

        public SteamGridDbImageException(ExceptionType type)
        {
            ExceptionType = type;
        }

        public SteamGridDbImageException(ExceptionType type, string message) : base(message)
        {
            ExceptionType = type;
        }

        public SteamGridDbImageException(ExceptionType type, string message, Exception inner) : base(message, inner)
        {
            ExceptionType = type;
        }

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
