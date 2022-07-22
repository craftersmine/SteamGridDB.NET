namespace craftersmine.SteamGridDBNet.Exceptions
{
    /// <summary>
    /// The exception that is thrown when invalid API key is provided
    /// </summary>
    public class SteamGridDbUnauthorizedException : SteamGridDbException
    {
        public SteamGridDbUnauthorizedException(string message) : base(message)
        {

        }
    }
}
