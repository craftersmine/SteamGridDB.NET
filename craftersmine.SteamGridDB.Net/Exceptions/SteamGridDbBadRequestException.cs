namespace craftersmine.SteamGridDBNet.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a bad request is returned by SteamGridDB API
    /// </summary>
    public class SteamGridDbBadRequestException : SteamGridDbException
    {
        /// <summary>
        /// Instantiates new instance of <see cref="SteamGridDbBadRequestException"/>
        /// </summary>
        /// <param name="message"></param>
        public SteamGridDbBadRequestException(string message) : base(message)
        {

        }
    }
}
