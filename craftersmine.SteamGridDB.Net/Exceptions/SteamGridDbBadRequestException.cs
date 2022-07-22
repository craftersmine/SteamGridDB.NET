namespace craftersmine.SteamGridDBNet.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a bad request is returned by SteamGridDB API
    /// </summary>
    public class SteamGridDbBadRequestException : SteamGridDbException
    {
        public SteamGridDbBadRequestException(string message) : base(message)
        {

        }
    }
}
