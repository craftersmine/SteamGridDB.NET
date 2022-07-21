using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace craftersmine.SteamGridDBNet
{
    /// <summary>
    /// Represents a base object for all SteamGridDB objects, <see cref="SteamGridDbGrid"/>, <see cref="SteamGridDbHero"/>, <see cref="SteamGridDbLogo"/> and <see cref="SteamGridDbIcon"/>
    /// </summary>
    public class SteamGridDbObject
    {
        /// <summary>
        /// Gets SteamGridDB item ID
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; private set; }
        /// <summary>
        /// Gets item Score. No official documentation
        /// </summary>
        [JsonProperty("score")]
        public int Score { get; private set; }
        /// <summary>
        /// Gets an item style
        /// </summary>
        [JsonProperty("style")]
        public SteamGridDbStyles Style { get; private set; }
        /// <summary>
        /// Gets an item image width
        /// </summary>
        [JsonProperty("width")]
        public int Width { get; private set; }
        /// <summary>
        /// Gets an item image height
        /// </summary>
        [JsonProperty("height")]
        public int Height { get; private set; }
        /// <summary>
        /// Gets <see langword="true"/> if contains Non-Suitable-For-Work content, otherwise <see langword="false"/>
        /// </summary>
        [JsonProperty("nsfw")]
        public bool IsNsfw { get; private set; }
        /// <summary>
        /// Gets <see langword="true"/> if contains humor content, otherwise <see langword="false"/>
        /// </summary>
        [JsonProperty("humor")]
        public bool IsHumorous { get; private set; }
        /// <summary>
        /// Gets user specified notes for object
        /// </summary>
        [JsonProperty("notes")]
        public string Notes { get; private set; }
        /// <summary>
        /// Gets item image format
        /// </summary>
        [JsonProperty("mime")]
        public SteamGridDbFormats Format { get; private set; }
        /// <summary>
        /// Gets item language
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; private set; }
        /// <summary>
        /// Gets item full image URL
        /// </summary>
        [JsonProperty("url")]
        public string FullImageUrl { get; private set; }
        /// <summary>
        /// Gets item thumbnail image URL
        /// </summary>
        [JsonProperty("thumb")]
        public string ThumbnailImageUrl { get; private set; }
        /// <summary>
        /// Gets <see langword="true"/> if item is locked by user, otherwise <see langword="false"/>. No official documentation
        /// </summary>
        [JsonProperty("lock")]
        public bool IsLocked { get; private set; }
        /// <summary>
        /// Gets <see langword="true"/> if contains content that can cause seizures or epilepsy, otherwise <see langword="false"/>
        /// </summary>
        [JsonProperty("epilepsy")]
        public bool CanCauseEpilepsy { get; private set; }
        /// <summary>
        /// Gets a number of upvotes for item. No official documentation
        /// </summary>
        [JsonProperty("upvotes")]
        public int Upvotes { get; private set; }
        /// <summary>
        /// Gets a number of downvotes for item. No official documentation
        /// </summary>
        [JsonProperty("downvotes")]
        public int Downvotes { get; private set; }
        /// <summary>
        /// Gets a <see cref="SteamAuthor"/> object that created that item
        /// </summary>
        [JsonProperty("author")]
        public SteamAuthor Author { get; private set; }
    }
}
