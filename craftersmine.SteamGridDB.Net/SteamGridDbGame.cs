using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace craftersmine.SteamGridDBNet
{
    public class SteamGridDbGame : SteamGridDbObject
    {
        [JsonProperty("name")]
        public string? Name { get; private set; }
        [JsonProperty("types")]
        public SteamGridDbGamePlatform[]? Types { get; private set; }
        [JsonProperty("verified")]
        public bool Verified { get; private set; }
        [JsonProperty("release_date"), JsonConverter(typeof(craftersmine.SteamGridDBNet.Converters.UnixDateTimeConverter))]
        public DateTime ReleaseDate { get; private set; }
    }
}
