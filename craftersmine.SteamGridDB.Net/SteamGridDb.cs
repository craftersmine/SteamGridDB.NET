using System.Net;
using System.Net.Http.Headers;

using craftersmine.SteamGridDBNet.Exceptions;

using Newtonsoft.Json;

namespace craftersmine.SteamGridDBNet
{
    public class SteamGridDb
    {
        private HttpClient _httpClient;

        public const string BaseAddress = "https://www.steamgriddb.com/api/v2/";

        public string ApiKey { get; }
        public TimeSpan Timeout
        {
            get => _httpClient.Timeout;
            set => _httpClient.Timeout = value;
        }

        public SteamGridDb(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentNullException(nameof(apiKey));

            ApiKey = apiKey;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseAddress);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
        }

        public void CancelPendingRequests()
        {
            _httpClient.CancelPendingRequests();
        }

        public async Task<SteamGridDbGame?> GetGameByIdAsync(int id)
        {
            var response = await Get($"games/id/{id}");
            return response.Data!.ToObject<SteamGridDbGame>();
        }

        public async Task<SteamGridDbGame?> GetGameBySteamIdAsync(int steamId)
        {
            var response = await Get($"games/steam/{steamId}");
            return response.Data!.ToObject<SteamGridDbGame>();
        }

        public async Task<SteamGridDbGrid?> GetGridByGameIdAsync(int gameId)
        {
            return null;
        }

        private async Task<SteamGridDbResponse> Get(string uri)
        {
            var response = await _httpClient.GetAsync(uri);
            var str = await response.Content.ReadAsStringAsync();
            var respObj = JsonConvert.DeserializeObject<SteamGridDbResponse>(str);

            if (respObj!.Success)
                return respObj;


            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new SteamGridDbException(Resources.Resources.Exception_BadRequest)
                        { ExceptionType = ExceptionType.BadRequest, SteamGridDbErrorMessages = respObj.Errors };
                case HttpStatusCode.Forbidden:
                    throw new SteamGridDbException(Resources.Resources.Exception_Forbidden)
                        { ExceptionType = ExceptionType.Forbidden, SteamGridDbErrorMessages = respObj.Errors };
                case HttpStatusCode.NotFound:
                    throw new SteamGridDbException(Resources.Resources.Exception_NotFound)
                        { ExceptionType = ExceptionType.NotFound, SteamGridDbErrorMessages = respObj.Errors };
                case HttpStatusCode.Unauthorized:
                    throw new SteamGridDbUnauthorizedException(Resources.Resources.Exception_Unauthorized)
                        { ExceptionType = ExceptionType.Unauthorized, SteamGridDbErrorMessages = respObj.Errors };
            }

            throw new SteamGridDbException(Resources.Resources.Exception_Unknown);
        }

        private T? DeserializeFromString<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}