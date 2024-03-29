﻿namespace craftersmine.SteamGridDBNet.Exceptions
{
    /// <summary>
    /// The exception that is thrown when SteamGridDB returns a Forbidden response. Probably because user doesn't own an item
    /// </summary>
    public class SteamGridDbForbiddenException : SteamGridDbException
    {
        /// <summary>
        /// Instantiates new instance of <see cref="SteamGridDbForbiddenException"/>
        /// </summary>
        /// <param name="message"></param>
        public SteamGridDbForbiddenException(string message) : base(message)
        {

        }
    }
}
