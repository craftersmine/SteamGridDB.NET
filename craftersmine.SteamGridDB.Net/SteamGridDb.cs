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

        public async Task<SteamGridDbGrid[]?> GetGridsByGameIdAsync(int gameId, bool nsfw = false, bool humorous = false, 
            SteamGridDbStyles styles = SteamGridDbStyles.AllGrids, SteamGridDbDimensions dimensions = SteamGridDbDimensions.AllGrids, 
            SteamGridDbFormats formats = SteamGridDbFormats.All, SteamGridDbGridTypes types = SteamGridDbGridTypes.All)
        {
            if (styles.HasFlag(SteamGridDbStyles.AllHeroes | SteamGridDbStyles.AllIcons | SteamGridDbStyles.AllLogos))
                styles &= ~(SteamGridDbStyles.AllHeroes | SteamGridDbStyles.AllIcons | SteamGridDbStyles.AllLogos);

            if (dimensions.HasFlag(SteamGridDbDimensions.AllHeroes)) dimensions &= ~(SteamGridDbDimensions.AllHeroes);
            var stylesFilter = SteamGridDbConstants.Styles.GetFromFlags(styles);
            var dimensionsFilter = SteamGridDbConstants.Dimensions.GetFromFlags(dimensions);
            var formatsFilter = SteamGridDbConstants.Mimes.GetFromFlags(formats);
            var typesFilter = SteamGridDbConstants.Types.GetFromFlags(types);
            var response = await Get($"grids/game/{gameId}?styles={stylesFilter}&dimensions={dimensionsFilter}&mimes={formatsFilter}&types={typesFilter}&nsfw={nsfw.ToString().ToLower()}&humor={humorous.ToString().ToLower()}");
            return response.Data!.ToObject<SteamGridDbGrid[]>();
        }

        public async Task<SteamGridDbGrid[]?> GetGridsByPlatformGameIdAsync(SteamGridDbGamePlatform platform, int platformGameId,
            bool nsfw = false, bool humorous = false, SteamGridDbStyles styles = SteamGridDbStyles.AllGrids,
            SteamGridDbDimensions dimensions = SteamGridDbDimensions.AllGrids,
            SteamGridDbFormats formats = SteamGridDbFormats.All, SteamGridDbGridTypes types = SteamGridDbGridTypes.All)
        {
            if (styles.HasFlag(SteamGridDbStyles.AllHeroes | SteamGridDbStyles.AllIcons | SteamGridDbStyles.AllLogos))
                styles &= ~(SteamGridDbStyles.AllHeroes | SteamGridDbStyles.AllIcons | SteamGridDbStyles.AllLogos);

            if (dimensions.HasFlag(SteamGridDbDimensions.AllHeroes)) dimensions &= ~(SteamGridDbDimensions.AllHeroes);
            var platforms = SteamGridDbConstants.Platforms.GetFromFlags(platform);
            var stylesFilter = SteamGridDbConstants.Styles.GetFromFlags(styles);
            var dimensionsFilter = SteamGridDbConstants.Dimensions.GetFromFlags(dimensions);
            var formatsFilter = SteamGridDbConstants.Mimes.GetFromFlags(formats);
            var typesFilter = SteamGridDbConstants.Types.GetFromFlags(types);
            var response = await Get($"grids/{platforms}/{platformGameId}?styles={stylesFilter}&dimensions={dimensionsFilter}&mimes={formatsFilter}&types={typesFilter}&nsfw={nsfw.ToString().ToLower()}&humor={humorous.ToString().ToLower()}");
            return response.Data!.ToObject<SteamGridDbGrid[]>();
        }

        public async Task<bool> UploadGridAsync(int gameId, Stream imageStream, SteamGridDbStyles style = SteamGridDbStyles.Alternate)
        {
            if (style.MoreThanOneFlag())
                throw new ArgumentException(Resources.Resources.Exception_MoreThanOneStyleSelected, nameof(style));

            var styleStr = SteamGridDbConstants.Styles.GetFromFlags(style);
            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new StringContent(gameId.ToString()), "game_id");
            if (style != SteamGridDbStyles.None)
                content.Add(new StringContent(styleStr), "style");
            byte[] signature = new byte[32];
            var read = await imageStream.ReadAsync(signature, 0, 32);
            if (read == 0)
                throw new ArgumentOutOfRangeException(Resources.Resources.Exception_ImageStreamIsEmpty);
            imageStream.Position = 0;
            string mimeType = "";
            string ext = "";
            if (MimeHelper.ValidateMimeType(signature, SteamGridDbConstants.Mimes.Jpeg))
            {
                ext = "jpg";
                mimeType = SteamGridDbConstants.Mimes.Jpeg;
            }

            if (MimeHelper.ValidateMimeType(signature, SteamGridDbConstants.Mimes.Png))
            {
                ext = "png";
                mimeType = SteamGridDbConstants.Mimes.Png;
            }
            if (string.IsNullOrWhiteSpace(mimeType))
                throw new InvalidMimeTypeException(Resources.Resources.Exception_InvalidMimeType);
            
            var streamContent = new StreamContent(imageStream);
            streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse(mimeType);
            content.Add(streamContent , "asset", "image." + ext);
            var response = await Post("grids", content);
            return response.Success;
        }

        public async Task<bool> UploadGridFromFileAsync(int gameId, string filePath,
            SteamGridDbStyles style = SteamGridDbStyles.Alternate)
        {
            using (FileStream file = File.OpenRead(filePath))
            {
                return await UploadGridAsync(gameId, file);
            }
        }

        public async Task<bool> DeleteGridsAsync(params int[] gridIds)
        {
            string ids = string.Join(",", gridIds);
            var response = await Delete("grids/" + ids);
            return response.Success;
        }

        public async Task<bool> DeleteGridAsync(int gridId)
        {
            return await DeleteGridsAsync(gridId);
        }

        public async Task<SteamGridDbHero[]?> GetHeroesByGameIdAsync(int gameId, bool nsfw = false, bool humorous = false,
            SteamGridDbStyles styles = SteamGridDbStyles.AllHeroes,
            SteamGridDbDimensions dimensions = SteamGridDbDimensions.AllHeroes,
            SteamGridDbFormats formats = SteamGridDbFormats.All, SteamGridDbGridTypes types = SteamGridDbGridTypes.All)
        {
            if (styles.HasFlag(SteamGridDbStyles.AllIcons | SteamGridDbStyles.AllLogos))
                styles &= ~(SteamGridDbStyles.AllIcons | SteamGridDbStyles.AllLogos);
            if (dimensions.HasFlag(SteamGridDbDimensions.AllGrids))
                dimensions &= ~(SteamGridDbDimensions.AllGrids);
            var stylesFilter = SteamGridDbConstants.Styles.GetFromFlags(styles);
            var dimensionsFilter = SteamGridDbConstants.Dimensions.GetFromFlags(dimensions);
            var formatsFilter = SteamGridDbConstants.Mimes.GetFromFlags(formats);
            var typesFilter = SteamGridDbConstants.Types.GetFromFlags(types);
            var response =
                await Get(
                    $"heroes/game/{gameId}?styles={stylesFilter}&dimensions={dimensionsFilter}&mimes={formatsFilter}&types={typesFilter}&nsfw={nsfw.ToString().ToLower()}&humor={humorous.ToString().ToLower()}");
            return response.Data!.ToObject<SteamGridDbHero[]>();
        }

        public async Task<SteamGridDbHero[]?> GetHeroesByPlatformGameIdAsync(SteamGridDbGamePlatform platform,
            int platformGameId,
            bool nsfw = false, bool humorous = false, SteamGridDbStyles styles = SteamGridDbStyles.AllHeroes,
            SteamGridDbDimensions dimensions = SteamGridDbDimensions.AllHeroes,
            SteamGridDbFormats formats = SteamGridDbFormats.All, SteamGridDbGridTypes types = SteamGridDbGridTypes.All)
        {
            if (styles.HasFlag(SteamGridDbStyles.AllIcons | SteamGridDbStyles.AllLogos))
                styles &= ~(SteamGridDbStyles.AllIcons | SteamGridDbStyles.AllLogos);
            if (dimensions.HasFlag(SteamGridDbDimensions.AllGrids))
                dimensions &= ~(SteamGridDbDimensions.AllGrids);
            var platforms = SteamGridDbConstants.Platforms.GetFromFlags(platform);
            var stylesFilter = SteamGridDbConstants.Styles.GetFromFlags(styles);
            var dimensionsFilter = SteamGridDbConstants.Dimensions.GetFromFlags(dimensions);
            var formatsFilter = SteamGridDbConstants.Mimes.GetFromFlags(formats);
            var typesFilter = SteamGridDbConstants.Types.GetFromFlags(types);
            var response = await Get($"heroes/{platforms}/{platformGameId}?styles={stylesFilter}&dimensions={dimensionsFilter}&mimes={formatsFilter}&types={typesFilter}&nsfw={nsfw.ToString().ToLower()}&humor={humorous.ToString().ToLower()}");
            return response.Data!.ToObject<SteamGridDbHero[]>();
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
                    throw new SteamGridDbBadRequestException(Resources.Resources.Exception_BadRequest)
                        { ExceptionType = ExceptionType.BadRequest, SteamGridDbErrorMessages = respObj.Errors };
                case HttpStatusCode.Forbidden:
                    throw new SteamGridDbForbiddenException(Resources.Resources.Exception_Forbidden)
                        { ExceptionType = ExceptionType.Forbidden, SteamGridDbErrorMessages = respObj.Errors };
                case HttpStatusCode.NotFound:
                    throw new SteamGridDbNotFoundException(Resources.Resources.Exception_NotFound)
                        { ExceptionType = ExceptionType.NotFound, SteamGridDbErrorMessages = respObj.Errors };
                case HttpStatusCode.Unauthorized:
                    throw new SteamGridDbUnauthorizedException(Resources.Resources.Exception_Unauthorized)
                        { ExceptionType = ExceptionType.Unauthorized, SteamGridDbErrorMessages = respObj.Errors };
            }

            throw new SteamGridDbException(Resources.Resources.Exception_Unknown);
        }

        private async Task<SteamGridDbResponse> Post(string uri, MultipartFormDataContent content)
        {
            var response = await _httpClient.PostAsync(uri, content);
            var str = await response.Content.ReadAsStringAsync();
            var respObj = JsonConvert.DeserializeObject<SteamGridDbResponse>(str);

            if (respObj!.Success)
                return respObj;

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new SteamGridDbBadRequestException(Resources.Resources.Exception_BadRequest)
                        { ExceptionType = ExceptionType.BadRequest, SteamGridDbErrorMessages = respObj.Errors };
                case HttpStatusCode.Forbidden:
                    throw new SteamGridDbForbiddenException(Resources.Resources.Exception_Forbidden)
                        { ExceptionType = ExceptionType.Forbidden, SteamGridDbErrorMessages = respObj.Errors };
                case HttpStatusCode.NotFound:
                    throw new SteamGridDbNotFoundException(Resources.Resources.Exception_NotFound)
                        { ExceptionType = ExceptionType.NotFound, SteamGridDbErrorMessages = respObj.Errors };
                case HttpStatusCode.Unauthorized:
                    throw new SteamGridDbUnauthorizedException(Resources.Resources.Exception_Unauthorized)
                        { ExceptionType = ExceptionType.Unauthorized, SteamGridDbErrorMessages = respObj.Errors };
            }

            throw new SteamGridDbException(Resources.Resources.Exception_Unknown);
        }

        private async Task<SteamGridDbResponse> Delete(string uri)
        {
            var response = await _httpClient.DeleteAsync(uri);
            var str = await response.Content.ReadAsStringAsync();
            var respObj = JsonConvert.DeserializeObject<SteamGridDbResponse>(str);

            if (respObj!.Success)
                return respObj;

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new SteamGridDbBadRequestException(Resources.Resources.Exception_BadRequest)
                        { ExceptionType = ExceptionType.BadRequest, SteamGridDbErrorMessages = respObj.Errors };
                case HttpStatusCode.Forbidden:
                    throw new SteamGridDbForbiddenException(Resources.Resources.Exception_Forbidden)
                        { ExceptionType = ExceptionType.Forbidden, SteamGridDbErrorMessages = respObj.Errors };
                case HttpStatusCode.NotFound:
                    throw new SteamGridDbNotFoundException(Resources.Resources.Exception_NotFound)
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