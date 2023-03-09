using System.Collections.Generic;

namespace craftersmine.SteamGridDBNet
{
    internal static class SteamGridDbConstants
    {
        public static class Platforms
        {
            public const string Steam = "steam";
            public const string Gog = "gog";
            public const string Origin = "origin";
            public const string EpicGamesStore = "egs";
            public const string BattleNet = "bnet";
            public const string UbisoftConnect = "uplay";
            public const string Flashpoint = "flashpoint";
            public const string NintendoEshop = "eshop";

            public static string GetFromFlags(SteamGridDbGamePlatform platforms)
            {
                List<string> lst = new List<string>();
                if (platforms.HasFlag(SteamGridDbGamePlatform.Steam))
                    lst.Add(Steam);
                if (platforms.HasFlag(SteamGridDbGamePlatform.Gog))
                    lst.Add(Gog);
                if (platforms.HasFlag(SteamGridDbGamePlatform.Origin))
                    lst.Add(Origin);
                if (platforms.HasFlag(SteamGridDbGamePlatform.Egs))
                    lst.Add(EpicGamesStore);
                if (platforms.HasFlag(SteamGridDbGamePlatform.Bnet))
                    lst.Add(BattleNet);
                if (platforms.HasFlag(SteamGridDbGamePlatform.Uplay))
                    lst.Add(UbisoftConnect);
                if (platforms.HasFlag(SteamGridDbGamePlatform.Flashpoint))
                    lst.Add(Flashpoint);
                if (platforms.HasFlag(SteamGridDbGamePlatform.Eshop))
                    lst.Add(NintendoEshop);

                return string.Join(",", lst);
            }
        }

        public static class Styles
        {
            public const string Alternate = "alternate";
            public const string Blurred = "blurred";
            public const string WhiteLogo = "white_logo";
            public const string Material = "material";
            public const string NoLogo = "no_logo";
            public const string Official = "official";
            public const string White = "white";
            public const string Black = "black";
            public const string Custom = "custom";

            public static string GetFromFlags(SteamGridDbStyles styles)
            {
                List<string> lst = new List<string>();
                if (styles.HasFlag(SteamGridDbStyles.Alternate))
                    lst.Add(Alternate);
                if (styles.HasFlag(SteamGridDbStyles.Blurred))
                    lst.Add(Blurred);
                if (styles.HasFlag(SteamGridDbStyles.WhiteLogo))
                    lst.Add(WhiteLogo);
                if (styles.HasFlag(SteamGridDbStyles.Material))
                    lst.Add(Material);
                if (styles.HasFlag(SteamGridDbStyles.NoLogo)) 
                    lst.Add(NoLogo);
                if (styles.HasFlag(SteamGridDbStyles.Official))
                    lst.Add(Official);
                if (styles.HasFlag(SteamGridDbStyles.White))
                    lst.Add(White);
                if (styles.HasFlag(SteamGridDbStyles.Black))
                    lst.Add(Black);
                if (styles.HasFlag(SteamGridDbStyles.Custom))
                    lst.Add(Custom);

                return string.Join(",", lst);
            }
        }

        public static class Dimensions
        {
            public const string W600H900 = "600x900";
            public const string W460H215 = "460x215";
            public const string W920H430 = "920x430";
            public const string W342H482 = "342x482";
            public const string W660H930 = "660x930";
            public const string W512H512 = "512x512";
            public const string W1024H1024 = "1024x1024";
            public const string W1920H620 = "1920x620";
            public const string W3840H1240 = "3840x1240";
            public const string W1600H650 = "1600x650";

            public static string GetFromFlags(SteamGridDbDimensions dimensions)
            {
                List<string> lst = new List<string>();
                if (dimensions.HasFlag(SteamGridDbDimensions.W600H900))
                    lst.Add(W600H900);
                if (dimensions.HasFlag(SteamGridDbDimensions.W460H215))
                    lst.Add(W460H215);
                if (dimensions.HasFlag(SteamGridDbDimensions.W920H430))
                    lst.Add(W920H430);
                if (dimensions.HasFlag(SteamGridDbDimensions.W342H482))
                    lst.Add(W342H482);
                if (dimensions.HasFlag(SteamGridDbDimensions.W660H930))
                    lst.Add(W660H930);
                if (dimensions.HasFlag(SteamGridDbDimensions.W512H512))
                    lst.Add(W512H512);
                if (dimensions.HasFlag(SteamGridDbDimensions.W1024H1024))
                    lst.Add(W1024H1024);
                if (dimensions.HasFlag(SteamGridDbDimensions.W1920H620))
                    lst.Add(W1920H620);
                if (dimensions.HasFlag(SteamGridDbDimensions.W3840H1240))
                    lst.Add(W3840H1240);
                if (dimensions.HasFlag(SteamGridDbDimensions.W1600H650))
                    lst.Add(W1600H650);

                return string.Join(",", lst);
            }
        }

        public static class Mimes
        {
            public const string Png = "image/png";
            public const string Jpeg = "image/jpeg";
            public const string Webp = "image/webp";
            public const string Ico = "image/vnd.microsoft.icon";

            public static string GetFromFlags(SteamGridDbFormats formats)
            {
                List<string> lst = new List<string>();
                if (formats.HasFlag(SteamGridDbFormats.Png))
                    lst.Add(Png);
                if (formats.HasFlag(SteamGridDbFormats.Jpeg))
                    lst.Add(Jpeg);
                if (formats.HasFlag(SteamGridDbFormats.Webp))
                    lst.Add(Webp);
                if (formats.HasFlag(SteamGridDbFormats.Ico))
                    lst.Add(Ico);

                return string.Join(",", lst);
            }
        }

        public static class Types
        {
            public const string Static = "static";
            public const string Animated = "animated";

            public static string GetFromFlags(SteamGridDbTypes gridTypes)
            {
                List<string> lst = new List<string>();
                if (gridTypes.HasFlag(SteamGridDbTypes.Static))
                    lst.Add(Static);
                if (gridTypes.HasFlag(SteamGridDbTypes.Animated))
                    lst.Add(Animated);

                return string.Join(",", lst);
            }
        }

        public static class Tags
        {
            public const string Humor = "humor";
            public const string Nsfw = "nsfw";
            public const string Epilepsy = "epilepsy";

            public static string GetFromFlags(SteamGridDbTags tags)
            {
                List<string> tagsL = new List<string>();
                if (tags.HasFlag(SteamGridDbTags.Humor))
                    tagsL.Add("humor");
                if (tags.HasFlag(SteamGridDbTags.Nsfw))
                    tagsL.Add("nsfw");
                if (tags.HasFlag(SteamGridDbTags.Epilepsy))
                    tagsL.Add("epilepsy");
                return string.Join(",", tagsL);
            }
        }

        public static class IconDimensions
        {
            public const string D8 = "8x8";
            public const string D16 = "16x16";
            public const string D24 = "24x24";
            public const string D28 = "28x28";
            public const string D32 = "32x32";
            public const string D40 = "40x40";
            public const string D48 = "48x48";
            public const string D60 = "60x60";
            public const string D64 = "64x64";
            public const string D72 = "72x72";
            public const string D96 = "96x96";
            public const string D128 = "128x128";
            public const string D192 = "192x192";
            public const string D256 = "256x256";
            public const string D512 = "512x512";
            public const string D768 = "768x768";
            public const string D1024 = "1024x1024";

            public static string GetFromFlags(SteamGridDbIconDimensions dimensions)
            {
                List<string> lst = new List<string>();

                if (dimensions.HasFlag(SteamGridDbIconDimensions.D8))
                    lst.Add(D8);
                if (dimensions.HasFlag(SteamGridDbIconDimensions.D16))
                    lst.Add(D16);
                if (dimensions.HasFlag(SteamGridDbIconDimensions.D24))
                    lst.Add(D24);
                if (dimensions.HasFlag(SteamGridDbIconDimensions.D28))
                    lst.Add(D28);
                if (dimensions.HasFlag(SteamGridDbIconDimensions.D32))
                    lst.Add(D32);
                if (dimensions.HasFlag(SteamGridDbIconDimensions.D40))
                    lst.Add(D40);
                if (dimensions.HasFlag(SteamGridDbIconDimensions.D48))
                    lst.Add(D48);
                if (dimensions.HasFlag(SteamGridDbIconDimensions.D60))
                    lst.Add(D60);
                if (dimensions.HasFlag(SteamGridDbIconDimensions.D64))
                    lst.Add(D64);
                if (dimensions.HasFlag(SteamGridDbIconDimensions.D72))
                    lst.Add(D72);
                if (dimensions.HasFlag(SteamGridDbIconDimensions.D96))
                    lst.Add(D96);
                if (dimensions.HasFlag(SteamGridDbIconDimensions.D128))
                    lst.Add(D128);
                if (dimensions.HasFlag(SteamGridDbIconDimensions.D192))
                    lst.Add(D192);
                if (dimensions.HasFlag(SteamGridDbIconDimensions.D256))
                    lst.Add(D256);
                if (dimensions.HasFlag(SteamGridDbIconDimensions.D512))
                    lst.Add(D512);
                if (dimensions.HasFlag(SteamGridDbIconDimensions.D768))
                    lst.Add(D768);
                if (dimensions.HasFlag(SteamGridDbIconDimensions.D1024))
                    lst.Add(D1024);

                return string.Join(",", lst);
            }
        }
    }
}
