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
    /// <summary>
    /// Contains all supported platforms by SteamGridDB. Can be used as bitmask in some cases
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter), converterParameters:typeof(SnakeCaseNamingStrategy)), Flags]
    public enum SteamGridDbGamePlatform
    {
        /// <summary>
        /// Represents Valve Steam platform
        /// </summary>
        Steam = 1,
        /// <summary>
        /// Represents GOG platform
        /// </summary>
        Gog = 2,
        /// <summary>
        /// Represents EA Origin platform
        /// </summary>
        Origin = 4,
        /// <summary>
        /// Represents Epic Games Store platform
        /// </summary>
        Egs = 8,
        /// <summary>
        /// Represents Blizzard Battle.Net platform
        /// </summary>
        Bnet = 16,
        /// <summary>
        /// Represents Ubisoft Connect platform (Uplay before)
        /// </summary>
        Uplay = 32,
        /// <summary>
        /// Represents BlueMaxima's Flashpoint platform
        /// </summary>
        Flashpoint = 64,
        /// <summary>
        /// Represents Nintendo Eshop platform
        /// </summary>
        Eshop = 128
    }

    /// <summary>
    /// Contains all supported Style tags on SteamGridDB
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter), converterParameters:typeof(SnakeCaseNamingStrategy)), Flags]
    public enum SteamGridDbStyles
    {
        /// <summary>
        /// No style selected. Only for internal library use
        /// </summary>
        None = 0,
        /// <summary>
        /// Alternate style
        /// </summary>
        Alternate = 1,
        /// <summary>
        /// Blurred image background style
        /// </summary>
        Blurred = 2,
        /// <summary>
        /// Contains game logo drawn completely in white
        /// </summary>
        WhiteLogo = 4,
        /// <summary>
        /// Material Design or Windows 8-10 tile styled
        /// </summary>
        Material = 8,
        /// <summary>
        /// Image doesn't contain any logos
        /// </summary>
        NoLogo = 16,
        /// <summary>
        /// Official or alike style
        /// </summary>
        Official = 32,
        /// <summary>
        /// Completely black logo
        /// </summary>
        Black = 64,
        /// <summary>
        /// Completely white logo
        /// </summary>
        White = 128,
        /// <summary>
        /// Custom styled
        /// </summary>
        Custom = 256,
        /// <summary>
        /// All styles supported by <see cref="SteamGridDbGrid"/> objects:
        /// <para><see cref="Alternate"/></para>
        /// <para><see cref="Blurred"/></para>
        /// <para><see cref="WhiteLogo"/></para>
        /// <para><see cref="Material"/></para>
        /// <para><see cref="NoLogo"/></para>
        /// </summary>
        AllGrids = Alternate | Blurred | WhiteLogo | Material | NoLogo,
        /// <summary>
        /// All styles supported by <see cref="SteamGridDbHero"/> objects:
        /// <para><see cref="Alternate"/></para>
        /// <para><see cref="Blurred"/></para>
        /// <para><see cref="Material"/></para>
        /// </summary>
        AllHeroes = Alternate | Blurred | Material,
        /// <summary>
        /// All styles supported by <see cref="SteamGridDbLogo"/> objects:
        /// <para><see cref="Official"/></para>
        /// <para><see cref="White"/></para>
        /// <para><see cref="Black"/></para>
        /// <para><see cref="Custom"/></para>
        /// </summary>
        AllLogos = Official | White | Black | Custom,
        /// <summary>
        /// All styles supported by <see cref="SteamGridDbIcon"/> objects:
        /// <para><see cref="Official"/></para>
        /// <para><see cref="Custom"/></para>
        /// </summary>
        AllIcons = Official | Custom
    }

    /// <summary>
    /// Contains all supported <see cref="SteamGridDbGrid"/> and <see cref="SteamGridDbHero"/> dimensions
    /// </summary>
    [Flags]
    public enum SteamGridDbDimensions
    {
        /// <summary>
        /// 600x900px
        /// </summary>
        W600H900 = 1,
        /// <summary>
        /// 460x215px
        /// </summary>
        W460H215 = 2,
        /// <summary>
        /// 920x430px
        /// </summary>
        W920H430 = 4,
        /// <summary>
        /// 342x482px
        /// </summary>
        W342H482 = 8,
        /// <summary>
        /// 660x930px
        /// </summary>
        W660H930 = 16,
        /// <summary>
        /// 512x512px
        /// </summary>
        W512H512 = 32,
        /// <summary>
        /// 1024x1024px
        /// </summary>
        W1024H1024 = 64,
        /// <summary>
        /// 1920x620px
        /// </summary>
        W1920H620 = 128,
        /// <summary>
        /// 3840x1240px
        /// </summary>
        W3840H1240 = 256,
        /// <summary>
        /// 1600x650px
        /// </summary>
        W1600H650 = 512,
        /// <summary>
        /// Contains all supported dimensions for <see cref="SteamGridDbGrid"/> object:
        /// <para><see cref="W600H900"/></para>
        /// <para><see cref="W460H215"/></para>
        /// <para><see cref="W920H430"/></para>
        /// <para><see cref="W342H482"/></para>
        /// <para><see cref="W660H930"/></para>
        /// <para><see cref="W512H512"/></para>
        /// <para><see cref="W1024H1024"/></para>
        /// </summary>
        AllGrids = W600H900 | W460H215 | W920H430 | W342H482 | W660H930 | W512H512 | W1024H1024,
        /// <summary>
        /// Contains all supported dimensions for <see cref="SteamGridDbHero"/> object:
        /// <para><see cref="W1920H620"/></para>
        /// <para><see cref="W3840H1240"/></para>
        /// <para><see cref="W1600H650"/></para>
        /// </summary>
        AllHeroes = W1920H620 | W3840H1240 | W1600H650
    }

    /// <summary>
    /// Contains all supported file types/MIME types by SteamGridDB
    /// </summary>
    [JsonConverter(typeof(MimeStringToFormatEnumConverter)), Flags]
    public enum SteamGridDbFormats
    {
        /// <summary>
        /// Unknown type. Only if SteamGridDB returns unknown type to library
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// PNG type. image/png MIME type
        /// </summary>
        Png = 1,
        /// <summary>
        /// JPEG type. image/jpeg MIME type
        /// </summary>
        Jpeg = 2,
        /// <summary>
        /// WEBP type. image/webp MIME type
        /// </summary>
        Webp = 4,
        /// <summary>
        /// Windows Icon type. image/vnd.microsoft.icon MIME type
        /// </summary>
        Ico = 8,
        /// <summary>
        /// All types supported by <see cref="SteamGridDbGrid"/> and <see cref="SteamGridDbHero"/>
        /// </summary>
        All = Png | Jpeg | Webp,
        /// <summary>
        /// All types supported by <see cref="SteamGridDbLogo"/>
        /// </summary>
        AllLogos = Png | Webp,
        /// <summary>
        /// All types supported by <see cref="SteamGridDbIcon"/>
        /// </summary>
        AllIcons = Png | Ico
    }

    /// <summary>
    /// Contains types of images supported by SteamGridDB
    /// </summary>
    [Flags]
    public enum SteamGridDbTypes
    {
        /// <summary>
        /// Static image. Simple PNG, JPEG or static WEBP
        /// </summary>
        Static = 1,
        /// <summary>
        /// Animated image. APNG or WEBP
        /// </summary>
        Animated = 2,
        /// <summary>
        /// Both static and animated images. Simple PNG, JPEG, static WEBP, APNG and animated WEBP
        /// </summary>
        All = Static | Animated
    }

    [Flags]
    internal enum SteamGridDbIconDimensions
    {
        D8 = 1,
        D16 = 4,
        D24 = 8,
        D28 = 16,
        D32 = 32,
        D40 = 64,
        D48 = 128,
        D60 = 256,
        D64 = 512,
        D72 = 1024,
        D96 = 2048,
        D128 = 4096,
        D192 = 8192,
        D256 = 16384,
        D512 = 32768,
        D768 = 65536,
        D1024 = 131072,
        All = D8 | D16 | D24 | D32 | D48 | D64 | D72 | D96 | D128 | D192 | D256 | D512 | D768 | D1024
    }
}
