using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace craftersmine.SteamGridDBNet
{
    /// <summary>
    /// Represents a SteamGridDB API response object. This class cannot be inherited
    /// </summary>
    public sealed class SteamGridDbResponse
    {
        /// <summary>
        /// Gets <see langword="true"/> if request succeeded, otherwise <see langword="false"/>
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; private set; }
        /// <summary>
        /// Gets a <see cref="JToken"/> object of resulting data. Can be null
        /// </summary>
        [JsonProperty("data")]
        public JToken Data { get; private set; }
        /// <summary>
        /// Gets an array of strings that represent error messages of SteamGridDB
        /// </summary>
        [JsonProperty("errors")]
        public string[] Errors { get; private set; }
        /// <summary>
        /// Gets a total amount of objects that SteamGridDB has
        /// </summary>
        [JsonProperty("total")]
        public int Total { get; private set; }
        /// <summary>
        /// Gets a limited amount set to request objects from SteamGridDB
        /// </summary>
        [JsonProperty("limit")]
        public int Limit { get; private set; }
        /// <summary>
        /// Gets current page of objects requested
        /// </summary>
        [JsonProperty("page")]
        public int Page { get; private set; }
    }
}
