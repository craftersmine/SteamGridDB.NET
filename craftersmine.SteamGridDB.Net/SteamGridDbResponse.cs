using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public JToken? Data { get; private set; }
        /// <summary>
        /// Gets an array of strings that represent error messages of SteamGridDB
        /// </summary>
        [JsonProperty("errors")]
        public string[]? Errors { get; private set; }
    }
}
