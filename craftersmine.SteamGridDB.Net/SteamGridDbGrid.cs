using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace craftersmine.SteamGridDBNet
{
    public class SteamGridDbGrid : SteamGridDbObject
    {
        [JsonProperty("score")]
        public int Score { get; private set; }
        [JsonProperty("style")]
        public SteamGridDbStyles Style { get; private set; }
        [JsonProperty("url")]
        public string? FullImageUrl { get; private set; }
        [JsonProperty("thumb")]
        public string? ThumbnailImageUrl { get; private set; }
        [JsonProperty("tags")]
        public string[]? Tags { get; private set; }
        [JsonProperty("author")]
        public SteamAuthor? Author { get; private set; }
    }
}
