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
        /// <summary>
        /// Instantiates new instance of <see cref="InvalidMimeTypeException"/>
        /// </summary>
        public InvalidMimeTypeException()
        {
        }

        /// <summary>
        /// Instantiates new instance of <see cref="InvalidMimeTypeException"/>
        /// </summary>
        /// <param name="message"></param>
        public InvalidMimeTypeException(string message) : base(message)
        {
        }

        /// <summary>
        /// Instantiates new instance of <see cref="InvalidMimeTypeException"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidMimeTypeException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Instantiates new instance of <see cref="InvalidMimeTypeException"/>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected InvalidMimeTypeException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
