using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace craftersmine.SteamGridDBNet
{
    public class SteamGridDbObject
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
