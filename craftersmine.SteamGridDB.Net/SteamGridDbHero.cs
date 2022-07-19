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
    /// <summary>
    /// Represents a Hero item from SteamGridDB. This class cannot be inherited
    /// </summary>
    public sealed class SteamGridDbHero : SteamGridDbObject
    {
        /// <summary>
        /// Gets a user-specified tags for Hero
        /// </summary>
        [JsonProperty("tags")]
        public string[]? Tags { get; private set; }
    }
}
