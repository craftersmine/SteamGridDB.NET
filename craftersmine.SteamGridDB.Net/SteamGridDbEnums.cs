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
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SteamGridDbGamePlatform
    {
        Steam = 1,
        GOG = 2,
        Origin = 4,
        Egs = 8,
        Bnet = 16,
        Uplay = 32,
        Flashpoint = 64,
        Eshop = 128,
    }

    [JsonConverter(typeof(StringEnumConverter), converterParameters:typeof(SnakeCaseNamingStrategy))]
    public enum SteamGridDbStyles
    {
        Alternate = 1,
        Blurred = 2,
        WhiteLogo = 4,
        Material = 8,
        NoLogo = 16
    }

    public enum SteamGridDbDimensions
    {
        W600H900 = 1,
        W460H215 = 2,
        W920H430 = 4,
        W342H482 = 8,
        W660H930 = 16,
        W512H512 = 32,
        W1024H1024 = 64
    }

    public enum SteamGridDbFormats
    {
        Png = 1,
        Jpeg = 2,
        Webp = 4
    }

    public enum SteamGridDbGridTypes
    {
        Static = 1,
        Animated = 2
    }
}
