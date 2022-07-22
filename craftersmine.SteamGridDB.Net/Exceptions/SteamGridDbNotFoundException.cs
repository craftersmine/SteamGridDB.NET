namespace craftersmine.SteamGridDBNet.Exceptions
{
    /// <summary>
    /// The exception that is thrown when item is not found in SteamGridDB
    /// </summary>
    public class SteamGridDbNotFoundException : SteamGridDbException
    {
        /// <summary>
        /// Instantiates new instance of <see cref="SteamGridDbNotFoundException"/>
        /// </summary>
        /// <param name="message"></param>
        public SteamGridDbNotFoundException(string message) : base(message)
        {

        }
    }
}
