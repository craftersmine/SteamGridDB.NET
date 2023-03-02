using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace craftersmine.SteamGridDBNet.Exceptions
{
    /// <summary>
    /// The exception that is thrown when you are being rate-limited on server
    /// </summary>
    [Serializable]
    public class SteamGridDbRateLimitedException : SteamGridDbException
    {
        public TimeSpan RetryAfter { get; private set; }

        /// <inheritdoc cref="SteamGridDbException"/>
        public SteamGridDbRateLimitedException(TimeSpan retryAfter)
        {
            RetryAfter = retryAfter;
        }
        
        /// <inheritdoc cref="SteamGridDbException"/>
        public SteamGridDbRateLimitedException(string message, TimeSpan retryAfter) : base(message)
        {
            RetryAfter = retryAfter;
        }
        
        /// <inheritdoc cref="SteamGridDbException"/>
        public SteamGridDbRateLimitedException(string message, TimeSpan retryAfter, Exception inner) : base(message, inner)
        {
            RetryAfter = retryAfter;
        }
        
        /// <inheritdoc cref="SteamGridDbException"/>
        protected SteamGridDbRateLimitedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
