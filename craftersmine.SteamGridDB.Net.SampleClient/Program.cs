using craftersmine.SteamGridDBNet;

namespace craftersmine.SteamGridDB.Net.SampleClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Initialize new SteamGridDb instance with your API key
            // You can get your API key here
            // https://www.steamgriddb.com/profile/preferences/api
            SteamGridDb instance = new SteamGridDb("PASTE_YOUR_API_KEY_HERE");

            // Use try-catch to handle errors that may occur (like item not found, unauthorized, forbidden, etc.)
            try
            {
                // You can search for games by keywords, just like on website
                var games = await instance.SearchForGamesAsync("Terraria");

                // Then you can get game information from SteamGridDB by ID
                var game = await instance.GetGameByIdAsync(1226);
                // Or, use Steam game ID
                game = await instance.GetGameBySteamIdAsync(105600);


                // You can get Grids, Heroes, Logos and Icons by calling needed methods
                // Here, we will get all available grids for game Terraria, with all styles, dimensions, etc by using SteamGridDB game ID
                var grids = await instance.GetGridsByGameIdAsync(1226);
                grids = await instance.GetGridsByPlatformGameIdAsync(SteamGridDbGamePlatform.Steam, 105600);

                // And here we will get all available Terraria logos with "Custom" style
                var logos = await instance.GetLogosByGameIdAsync(1226, styles: SteamGridDbStyles.Custom);
                // Here we will get all available animated Terraria logos
                logos = await instance.GetLogosByPlatformGameIdAsync(SteamGridDbGamePlatform.Steam, 105600, types: SteamGridDbTypes.Animated);
            }
            catch (Exception e)
            {
                // Here we can catch all errors that might occur while doing all above.
                // For SteamGridDB specific errors like Unauthorized, Not Found, Forbidden and Bad Request there is specific exceptions with additional info from SteamGridDB
                // Here are they
                //   SteamGridDbUnauthorizedException
                //   SteamGridDbNotFoundException
                //   SteamGridDbForbiddenException
                //   SteamGridDbBadRequestException
                // They are all derived from SteamGridDbException, which represents a generic SteamGridDB exception
                // Also they have properties such as ExceptionType enum, with error type
                // And SteamGridDbErrorMessages string array with SteamGridDB error response messages for additional info
                //
                // Also there is InvalidMimeTypeException which might occur when you try to upload invalid file type
                Console.WriteLine(e);
                throw;
            }
        }
    }
}