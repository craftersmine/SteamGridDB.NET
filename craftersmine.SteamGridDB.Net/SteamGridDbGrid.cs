using Newtonsoft.Json;

namespace craftersmine.SteamGridDBNet
{
    /// <summary>
    /// Represents a Grid item from SteamGridDB. This class cannot be inherited
    /// </summary>
    public sealed class SteamGridDbGrid : SteamGridDbObject
    {
        /// <summary>
        /// Gets a user-specified tags for grid
        /// </summary>
        [JsonProperty("tags")]
        public string[] Tags { get; private set; }
    }
}
