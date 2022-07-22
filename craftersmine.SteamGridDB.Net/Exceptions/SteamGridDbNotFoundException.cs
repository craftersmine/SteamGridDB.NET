namespace craftersmine.SteamGridDBNet.Exceptions
{
    /// <summary>
    /// The exception that is thrown when item is not found in SteamGridDB
    /// </summary>
    public class SteamGridDbNotFoundException : SteamGridDbException
    {
        public SteamGridDbNotFoundException(string message) : base(message)
        {

        }
    }
}
