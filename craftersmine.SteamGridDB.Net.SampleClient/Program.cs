using craftersmine.SteamGridDBNet;

namespace craftersmine.SteamGridDB.Net.SampleClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            SteamGridDb instance = new SteamGridDb("763883ca756f15cdf11a6712f9072e43");

            var resp = await instance.GetGameByIdAsync(35395);
        }
    }
}