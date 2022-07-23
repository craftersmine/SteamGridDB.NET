namespace craftersmine.SteamGridDBNet.Exceptions
{
    /// <summary>
    /// The exception that is thrown when invalid API key is provided
    /// </summary>
    public class SteamGridDbUnauthorizedException : SteamGridDbException
    {
        /// <summary>
        /// Instantiates new instance of <see cref="SteamGridDbUnauthorizedException"/>
        /// </summary>
        /// <param name="message"></param>
        public SteamGridDbUnauthorizedException(string message) : base(message)
        {

        }
    }
}
