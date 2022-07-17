using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace craftersmine.SteamGridDBNet
{
    public class SteamGridDbResponse
    {
        [JsonProperty("success")]
        public bool Success { get; private set; }
        [JsonProperty("data")]

        public JToken? Data { get; private set; }
        [JsonProperty("errors")]
        public string[]? Errors { get; private set; }
    }
}
