using System;
using System.Runtime.Serialization;

namespace craftersmine.SteamGridDBNet.Exceptions
{
    /// <summary>
    /// The exception that is thrown when data with invalid or unsupported MIME type is provided
    /// </summary>
    [Serializable]
    public class InvalidMimeTypeException : Exception
    {
        public InvalidMimeTypeException()
        {
        }

        public InvalidMimeTypeException(string message) : base(message)
        {
        }

        public InvalidMimeTypeException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InvalidMimeTypeException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
