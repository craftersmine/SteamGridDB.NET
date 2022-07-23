using Newtonsoft.Json;

namespace craftersmine.SteamGridDBNet
{
    /// <summary>
    /// Represents a Hero item from SteamGridDB. This class cannot be inherited
    /// </summary>
    public sealed class SteamGridDbHero : SteamGridDbObject
    {
        /// <summary>
        /// Gets a user-specified tags for Hero
        /// </summary>
        [JsonProperty("tags")]
        public string[] Tags { get; private set; }
    }
}
