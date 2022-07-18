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
        [JsonProperty("width")]
        public int Width { get; private set; }
        [JsonProperty("height")]
        public int Height { get; private set; }
        [JsonProperty("nsfw")]
        public bool IsNsfw { get; private set; }
        [JsonProperty("humor")]
        public bool IsHumorous { get; private set; }
        [JsonProperty("notes")]
        public string? Notes { get; private set; }
        [JsonProperty("mime")]
        public SteamGridDbFormats Format { get; private set; }
        [JsonProperty("language")]
        public string? Language { get; private set; }
        [JsonProperty("url")]
        public string? FullImageUrl { get; private set; }
        [JsonProperty("thumb")]
        public string? ThumbnailImageUrl { get; private set; }
        [JsonProperty("lock")]
        public bool IsLocked { get; private set; }
        [JsonProperty("epilepsy")]
        public bool CanCauseEpilepsy { get; private set; }
        [JsonProperty("upvotes")]
        public int Upvotes { get; private set; }
        [JsonProperty("downvotes")]
        public int Downvotes { get; private set; }
        [JsonProperty("tags")]
        public string[]? Tags { get; private set; }
        [JsonProperty("author")]
        public SteamAuthor? Author { get; private set; }
    }
}
