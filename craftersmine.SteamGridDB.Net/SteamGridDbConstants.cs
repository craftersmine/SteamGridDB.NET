using System.Net.Http.Headers;

namespace craftersmine.SteamGridDBNet
{
    internal static class SteamGridDbConstants
    {
        public static class Styles
        {
            public const string Alternate = "alternate";
            public const string Blurred = "blurred";
            public const string WhiteLogo = "white_logo";
            public const string Material = "material";
            public const string NoLogo = "no_logo";

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

                return string.Join(",", lst);
            }
        }

        public static class Mimes
        {
            public const string Png = "image/png";
            public const string Jpeg = "image/jpeg";
            public const string Webp = "image/webp";

            public static string GetFromFlags(SteamGridDbFormats formats)
            {
                List<string> lst = new List<string>();
                if (formats.HasFlag(SteamGridDbFormats.Png))
                    lst.Add(Png);
                if (formats.HasFlag(SteamGridDbFormats.Jpeg))
                    lst.Add(Jpeg);
                if (formats.HasFlag(SteamGridDbFormats.Webp))
                    lst.Add(Webp);

                return string.Join(",", lst);
            }
        }

        public static class Types
        {
            public const string Static = "static";
            public const string Animated = "animated";

            public static string GetFromFlags(SteamGridDbGridTypes gridTypes)
            {
                List<string> lst = new List<string>();
                if (gridTypes.HasFlag(SteamGridDbGridTypes.Static))
                    lst.Add(Static);
                if (gridTypes.HasFlag(SteamGridDbGridTypes.Animated))
                    lst.Add(Animated);

                return string.Join(",", lst);
            }
        }
    }
}
