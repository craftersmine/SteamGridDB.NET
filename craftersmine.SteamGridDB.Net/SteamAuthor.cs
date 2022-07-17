using Newtonsoft.Json;

namespace craftersmine.SteamGridDBNet
{
    public sealed class SteamAuthor
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("steam64")]
        public string SteamId64 { get; set; }
        [JsonProperty("avatar")]
        public string AvatarUrl { get; set; }
    }
}
