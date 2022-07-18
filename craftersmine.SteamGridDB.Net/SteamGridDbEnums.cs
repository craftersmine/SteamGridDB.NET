using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using craftersmine.SteamGridDBNet.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace craftersmine.SteamGridDBNet
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SteamGridDbGamePlatform
    {
        Steam = 1,
        Gog = 2,
        Origin = 4,
        Egs = 8,
        Bnet = 16,
        Uplay = 32,
        Flashpoint = 64,
        Eshop = 128
    }

    [JsonConverter(typeof(StringEnumConverter), converterParameters:typeof(SnakeCaseNamingStrategy)), Flags]
    public enum SteamGridDbStyles
    {
        None = 0,
        Alternate = 1,
        Blurred = 2,
        WhiteLogo = 4,
        Material = 8,
        NoLogo = 16,
        Official = 32,
        Black = 64,
        White = 128,
        Custom = 256,
        AllGrids = Alternate | Blurred | WhiteLogo | Material | NoLogo,
        AllHeroes = Alternate | Blurred | Material,
        AllLogos = Official | White | Black | Custom,
        AllIcons = Official | Custom
    }

    [Flags]
    public enum SteamGridDbDimensions
    {
        W600H900 = 1,
        W460H215 = 2,
        W920H430 = 4,
        W342H482 = 8,
        W660H930 = 16,
        W512H512 = 32,
        W1024H1024 = 64,
        W1920H620 = 128,
        W3840H1240 = 256,
        W1600H650 = 512,
        AllGrids = W600H900 | W460H215 | W920H430 | W342H482 | W660H930 | W512H512 | W1024H1024,
        AllHeroes = W1920H620 | W3840H1240 | W1600H650
    }

    [JsonConverter(typeof(MimeStringToFormatEnumConverter)), Flags]
    public enum SteamGridDbFormats
    {
        Unknown = 0,
        Png = 1,
        Jpeg = 2,
        Webp = 4,
        All = Png | Jpeg | Webp
    }

    [Flags]
    public enum SteamGridDbGridTypes
    {
        Static = 1,
        Animated = 2,
        All = Static | Animated
    }
}
