using System;
using Newtonsoft.Json;

namespace craftersmine.SteamGridDBNet
{
    /// <summary>
    /// Represents a Game object from SteamGridDB. This class cannot be inherited
    /// </summary>
    public sealed class SteamGridDbGame : SteamGridDbObject
    {
        /// <summary>
        /// Gets a name of game
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }
        /// <summary>
        /// Gets a bitwise mask of platforms on which game is exists
        /// </summary>
        [JsonProperty("types")]
        public SteamGridDbGamePlatform[] Platforms { get; private set; }
        /// <summary>
        /// Gets <see langword="true"/> if game is verified on SteamGridDB, otherwise <see langword="false"/>
        /// </summary>
        [JsonProperty("verified")]
        public bool Verified { get; private set; }
        /// <summary>
        /// Gets a <see cref="DateTime"/> of when the game was released
        /// </summary>
        [JsonProperty("release_date"), JsonConverter(typeof(craftersmine.SteamGridDBNet.Converters.UnixDateTimeConverter))]
        public DateTime ReleaseDate { get; private set; }
    }
}
