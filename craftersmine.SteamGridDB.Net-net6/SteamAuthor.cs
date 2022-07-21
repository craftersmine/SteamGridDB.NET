using Newtonsoft.Json;

namespace craftersmine.SteamGridDBNet
{
    /// <summary>
    /// Represents a Steam Author information. This class cannot be inherited
    /// </summary>
    public sealed class SteamAuthor
    {
        /// <summary>
        /// Gets a Steam username of author
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }
        /// <summary>
        /// Gets a SteamID64 of author
        /// </summary>
        [JsonProperty("steam64")]
        public string SteamId64 { get; private set; }
        /// <summary>
        /// Gets a Steam avatar URL of author
        /// </summary>
        [JsonProperty("avatar")]
        public string AvatarUrl { get; private set; }
    }
}
