using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using craftersmine.SteamGridDBNet.Exceptions;

using Newtonsoft.Json;

namespace craftersmine.SteamGridDBNet
{
    /// <summary>
    /// SteamGridDB API main class, get, upload, delete grids/heroes/icons/grids, search for games and get info about games. This class cannot be inherited
    /// </summary>
    public sealed class SteamGridDb : IDisposable
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Base SteamGridDB API URI address
        /// </summary>
        public const string BaseAddress = "https://www.steamgriddb.com/api/v2/";

        /// <summary>
        /// Gets current API key string
        /// </summary>
        public string ApiKey { get; }
        /// <summary>
        /// Gets or sets connection timeout time span
        /// </summary>
        public TimeSpan Timeout
        {
            get => _httpClient.Timeout;
            set => _httpClient.Timeout = value;
        }

        /// <summary>
        /// Instantiates new instance of SteamGridDB API class.
        /// </summary>
        /// <param name="apiKey">Your API key to authorize you do things</param>
        /// <exception cref="ArgumentNullException">When API key is empty, null or whitespace</exception>
        public SteamGridDb(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentNullException(nameof(apiKey));

            ApiKey = apiKey;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseAddress);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
        }

        /// <summary>
        /// Cancels all HTTP client pending requests
        /// </summary>
        public void CancelPendingRequests()
        {
            _httpClient.CancelPendingRequests();
        }
        

        /// <summary>
        /// Gets game information from SteamGridDB server by SteamGridDB game ID
        /// </summary>
        /// <param name="id">SteamGridDB specific game ID</param>
        /// <returns>Game information <see cref="SteamGridDbGame"/> object if succeeded</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        public async Task<SteamGridDbGame> GetGameByIdAsync(int id)
        {
            var response = await Get($"games/id/{id}");
            if (response.Data != null) return response.Data.ToObject<SteamGridDbGame>();
            return null;
        }

        /// <summary>
        /// Gets game information from SteamGridDB server by game Steam app ID
        /// </summary>
        /// <param name="steamId">Steam specific App ID. Can be seen in URL of game Steam store page</param>
        /// <returns>Game information <see cref="SteamGridDbGame"/> object if succeeded</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        public async Task<SteamGridDbGame> GetGameBySteamIdAsync(int steamId)
        {
            var response = await Get($"games/steam/{steamId}");
            if (response.Data != null) return response.Data.ToObject<SteamGridDbGame>();
            return null;
        }

        #region Grids

        /// <summary>
        /// Gets <see cref="SteamGridDbGrid"/> array for specified game with specified filters
        /// </summary>
        /// <param name="gameId">SteamGridDB game ID of game</param>
        /// <param name="nsfw">Include Non-Suitable-For-Work results, default <see langword="false"/></param>
        /// <param name="humorous">Include humorous results, default <see langword="false"/></param>
        /// <param name="epilepsy">Include content that can cause epilepsy</param>
        /// <param name="page">Page index to request data</param>
        /// <param name="tags">Bitmask for tags filter.</param>
        /// <param name="styles">Bitmask for styles filter. Allowed values see in <see cref="SteamGridDbStyles.AllGrids"/></param>
        /// <param name="dimensions">Bitmask for dimensions filter. Allowed values see in <see cref="SteamGridDbDimensions.AllGrids"/></param>
        /// <param name="formats">Bitmask for formats/mimes filter. Allowed values see in <see cref="SteamGridDbFormats.All"/></param>
        /// <param name="types">Bitmask for type of image, animated or static. <see cref="SteamGridDbTypes.All"/></param>
        /// <returns><see cref="SteamGridDbGrid"/> array of results</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        public async Task<SteamGridDbGrid[]> GetGridsByGameIdAsync(int gameId, bool nsfw = false, bool humorous = false,
            bool epilepsy = false, int page = 0, SteamGridDbTags tags = SteamGridDbTags.None,
            SteamGridDbStyles styles = SteamGridDbStyles.AllGrids,
            SteamGridDbDimensions dimensions = SteamGridDbDimensions.AllGrids,
            SteamGridDbFormats formats = SteamGridDbFormats.All, SteamGridDbTypes types = SteamGridDbTypes.All)
        {
            if (styles.HasFlag(SteamGridDbStyles.AllHeroes | SteamGridDbStyles.AllIcons | SteamGridDbStyles.AllLogos))
                styles &= ~(SteamGridDbStyles.AllHeroes | SteamGridDbStyles.AllIcons | SteamGridDbStyles.AllLogos);

            if (dimensions.HasFlag(SteamGridDbDimensions.AllHeroes)) dimensions &= ~(SteamGridDbDimensions.AllHeroes);
            var stylesFilter = SteamGridDbConstants.Styles.GetFromFlags(styles);
            var dimensionsFilter = SteamGridDbConstants.Dimensions.GetFromFlags(dimensions);
            var formatsFilter = SteamGridDbConstants.Mimes.GetFromFlags(formats);
            var typesFilter = SteamGridDbConstants.Types.GetFromFlags(types);

            string tagsParam = "";
            if (tags != SteamGridDbTags.None)
            {
                tagsParam = "&tags=" + SteamGridDbConstants.Tags.GetFromFlags(tags);
            }

            var response = await Get($"grids/game/{gameId}?styles={stylesFilter}&dimensions={dimensionsFilter}&mimes={formatsFilter}&types={typesFilter}&nsfw={nsfw.ToString().ToLower()}&humor={humorous.ToString().ToLower()}&epilepsy={epilepsy.ToString().ToLower()}&page={page}{tagsParam}");
            if (response.Data != null)
            {
                var objects = response.Data.ToObject<SteamGridDbGrid[]>();
                foreach (var obj in objects)
                {
                    obj.ApiInstance = this;
                }

                return objects;
            }
            return null;
        }

        /// <summary>
        /// Gets <see cref="SteamGridDbGrid"/> array for specified game by selected platform and platform specific Game ID (like Steam App ID) with specified filters
        /// </summary>
        /// <param name="platform">Platform of which items get</param>
        /// <param name="platformGameId">Platform specific game ID of game</param>
        /// <param name="nsfw">Include Non-Suitable-For-Work results, default <see langword="false"/></param>
        /// <param name="humorous">Include humorous results, default <see langword="false"/></param>
        /// <param name="epilepsy">Include content that can cause epilepsy</param>
        /// <param name="page">Page index to request data</param>
        /// <param name="tags">Bitmask for tags filter.</param>
        /// <param name="styles">Bitmask for styles filter. Allowed values see in <see cref="SteamGridDbStyles.AllGrids"/></param>
        /// <param name="dimensions">Bitmask for dimensions filter. Allowed values see in <see cref="SteamGridDbDimensions.AllGrids"/></param>
        /// <param name="formats">Bitmask for formats/mimes filter. Allowed values see in <see cref="SteamGridDbFormats.All"/></param>
        /// <param name="types">Bitmask for type of image, animated or static. <see cref="SteamGridDbTypes.All"/></param>
        /// <returns><see cref="SteamGridDbGrid"/> array of results</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        /// <exception cref="ArgumentException">When more than one platform selected</exception>
        public async Task<SteamGridDbGrid[]> GetGridsByPlatformGameIdAsync(SteamGridDbGamePlatform platform,
            int platformGameId, bool nsfw = false, bool humorous = false, bool epilepsy = false, int page = 0,
            SteamGridDbTags tags = SteamGridDbTags.None, SteamGridDbStyles styles = SteamGridDbStyles.AllGrids,
            SteamGridDbDimensions dimensions = SteamGridDbDimensions.AllGrids,
            SteamGridDbFormats formats = SteamGridDbFormats.All, SteamGridDbTypes types = SteamGridDbTypes.All)
        {
            if (styles.HasFlag(SteamGridDbStyles.AllHeroes | SteamGridDbStyles.AllIcons | SteamGridDbStyles.AllLogos))
                styles &= ~(SteamGridDbStyles.AllHeroes | SteamGridDbStyles.AllIcons | SteamGridDbStyles.AllLogos);

            if (dimensions.HasFlag(SteamGridDbDimensions.AllHeroes)) dimensions &= ~(SteamGridDbDimensions.AllHeroes);

            if (platform.MoreThanOneFlag())
                throw new ArgumentException(Resources.Resources.Exception_MoreThanOnePlatformSelected, nameof(platform));

            var platforms = SteamGridDbConstants.Platforms.GetFromFlags(platform);
            var stylesFilter = SteamGridDbConstants.Styles.GetFromFlags(styles);
            var dimensionsFilter = SteamGridDbConstants.Dimensions.GetFromFlags(dimensions);
            var formatsFilter = SteamGridDbConstants.Mimes.GetFromFlags(formats);
            var typesFilter = SteamGridDbConstants.Types.GetFromFlags(types);

            string tagsParam = "";
            if (tags != SteamGridDbTags.None)
            {
                tagsParam = "&tags=" + SteamGridDbConstants.Tags.GetFromFlags(tags);
            }

            var response = await Get($"grids/{platforms}/{platformGameId}?styles={stylesFilter}&dimensions={dimensionsFilter}&mimes={formatsFilter}&types={typesFilter}&nsfw={nsfw.ToString().ToLower()}&humor={humorous.ToString().ToLower()}&epilepsy={epilepsy.ToString().ToLower()}&page={page}{tagsParam}");
            if (response.Data != null)
            {
                var objects = response.Data.ToObject<SteamGridDbGrid[]>();
                foreach (var obj in objects)
                {
                    obj.ApiInstance = this;
                }

                return objects;
            }
            return null;
        }

        /// <summary>
        /// Gets <see cref="SteamGridDbGrid"/> array for specified game with specified filters
        /// </summary>
        /// <param name="game"><see cref="SteamGridDbGame"/> object for game data</param>
        /// <param name="nsfw">Include Non-Suitable-For-Work results, default <see langword="false"/></param>
        /// <param name="humorous">Include humorous results, default <see langword="false"/></param>
        /// <param name="epilepsy">Include content that can cause epilepsy</param>
        /// <param name="page">Page index to request data</param>
        /// <param name="tags">Bitmask for tags filter.</param>
        /// <param name="styles">Bitmask for styles filter. Allowed values see in <see cref="SteamGridDbStyles.AllGrids"/></param>
        /// <param name="dimensions">Bitmask for dimensions filter. Allowed values see in <see cref="SteamGridDbDimensions.AllGrids"/></param>
        /// <param name="formats">Bitmask for formats/mimes filter. Allowed values see in <see cref="SteamGridDbFormats.All"/></param>
        /// <param name="types">Bitmask for type of image, animated or static. <see cref="SteamGridDbTypes.All"/></param>
        /// <returns><see cref="SteamGridDbGrid"/> array of results</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        public async Task<SteamGridDbGrid[]> GetGridsForGameAsync(SteamGridDbGame game, bool nsfw = false,
            bool humorous = false, bool epilepsy = false, int page = 0, SteamGridDbTags tags = SteamGridDbTags.None,
            SteamGridDbStyles styles = SteamGridDbStyles.AllGrids,
            SteamGridDbDimensions dimensions = SteamGridDbDimensions.AllGrids,
            SteamGridDbFormats formats = SteamGridDbFormats.All, SteamGridDbTypes types = SteamGridDbTypes.All)
        {
            return await GetGridsByGameIdAsync(game.Id, nsfw, humorous, epilepsy, page, tags, styles, dimensions,
                formats, types);
        }

        /// <summary>
        /// Uploads image from <see cref="Stream"/> as <see cref="SteamGridDbGrid"/> to SteamGridDB
        /// </summary>
        /// <param name="gameId">SteamGridDB Game ID</param>
        /// <param name="imageStream"><see cref="Stream"/> of data, that represents an image. Must contain data following MIME types: image/png, image/jpeg</param>
        /// <param name="style">Style of Grid image for filters</param>
        /// <returns><see langword="true"/> if image uploaded correctly, otherwise <see langword="false"/> or exception will be thrown</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        /// <exception cref="ArgumentException">When more then one style selected</exception>
        /// <exception cref="ArgumentOutOfRangeException">When stream is empty or has length of 0</exception>
        /// <exception cref="InvalidMimeTypeException">When data in stream doesn't represent correct MIME type</exception>
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
                throw new InvalidMimeTypeException(string.Format(Resources.Resources.Exception_InvalidMimeType, string.Join(",", SteamGridDbConstants.Mimes.Png, SteamGridDbConstants.Mimes.Png)));
            
            var streamContent = new StreamContent(imageStream);
            streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse(mimeType);
            content.Add(streamContent , "asset", "image." + ext);
            var response = await Post("grids", content);
            return response.Success;
        }

        /// <summary>
        /// Uploads image from <see cref="Stream"/> as <see cref="SteamGridDbGrid"/> to SteamGridDB
        /// </summary>
        /// <param name="game"><see cref="SteamGridDbGame"/> object for game data</param>
        /// <param name="imageStream"><see cref="Stream"/> of data, that represents an image. Must contain data following MIME types: image/png, image/jpeg</param>
        /// <param name="style">Style of Grid image for filters</param>
        /// <returns><see langword="true"/> if image uploaded correctly, otherwise <see langword="false"/> or exception will be thrown</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        /// <exception cref="ArgumentException">When more then one style selected</exception>
        /// <exception cref="ArgumentOutOfRangeException">When stream is empty or has length of 0</exception>
        /// <exception cref="InvalidMimeTypeException">When data in stream doesn't represent correct MIME type</exception>
        public async Task<bool> UploadGridAsync(SteamGridDbGame game, Stream imageStream,
            SteamGridDbStyles style = SteamGridDbStyles.Alternate)
        {
            return await UploadGridAsync(game.Id, imageStream, style);
        }

        /// <summary>
        /// Uploads image from file as <see cref="SteamGridDbGrid"/> to SteamGridDB
        /// </summary>
        /// <param name="gameId">SteamGridDB Game ID</param>
        /// <param name="filePath">Full path to the file that represents an image. Must contain data following MIME types: image/png, image/jpeg</param>
        /// <param name="style">Style of Grid image for filters</param>
        /// <returns><see langword="true"/> if image uploaded correctly, otherwise <see langword="false"/> or exception will be thrown</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        /// <exception cref="ArgumentException">When more then one style selected</exception>
        /// <exception cref="ArgumentOutOfRangeException">When stream is empty or has length of 0</exception>
        /// <exception cref="InvalidMimeTypeException">When data in stream doesn't represent correct MIME type</exception>
        public async Task<bool> UploadGridFromFileAsync(int gameId, string filePath,
            SteamGridDbStyles style = SteamGridDbStyles.Alternate)
        {
            using (FileStream file = File.OpenRead(filePath))
            {
                return await UploadGridAsync(gameId, file, style);
            }
        }

        /// <summary>
        /// Uploads image from file as <see cref="SteamGridDbGrid"/> to SteamGridDB
        /// </summary>
        /// <param name="game"><see cref="SteamGridDbGame"/> object for game data</param>
        /// <param name="filePath">Full path to the file that represents an image. Must contain data following MIME types: image/png, image/jpeg</param>
        /// <param name="style">Style of Grid image for filters</param>
        /// <returns><see langword="true"/> if image uploaded correctly, otherwise <see langword="false"/> or exception will be thrown</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        /// <exception cref="ArgumentException">When more then one style selected</exception>
        /// <exception cref="ArgumentOutOfRangeException">When stream is empty or has length of 0</exception>
        /// <exception cref="InvalidMimeTypeException">When data in stream doesn't represent correct MIME type</exception>
        public async Task<bool> UploadGridFromFileAsync(SteamGridDbGame game, string filePath,
            SteamGridDbStyles style = SteamGridDbStyles.Alternate)
        {
            return await UploadGridFromFileAsync(game.Id, filePath, style);
        }

        /// <summary>
        /// Removes <see cref="SteamGridDbGrid"/> with specified IDs from server
        /// </summary>
        /// <param name="gridIds">List of IDs of <see cref="SteamGridDbGrid"/> objects</param>
        /// <returns><see langword="true"/> if successfully removed, otherwise <see langword="false"/> or thrown exception</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        public async Task<bool> DeleteGridsAsync(params int[] gridIds)
        {
            string ids = string.Join(",", gridIds);
            var response = await Delete("grids/" + ids);
            return response.Success;
        }

        /// <summary>
        /// Removes single <see cref="SteamGridDbGrid"/> with specified ID from server
        /// </summary>
        /// <param name="gridId">ID of <see cref="SteamGridDbGrid"/> object to remove</param>
        /// <returns><see langword="true"/> if successfully removed, otherwise <see langword="false"/> or thrown exception</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        public async Task<bool> DeleteGridAsync(int gridId)
        {
            return await DeleteGridsAsync(gridId);
        }

        #endregion

        #region Heroes

        /// <summary>
        /// Gets <see cref="SteamGridDbHero"/> array for specified game with specified filters
        /// </summary>
        /// <param name="gameId">SteamGridDB game ID of game</param>
        /// <param name="nsfw">Include Non-Suitable-For-Work results, default <see langword="false"/></param>
        /// <param name="humorous">Include humorous results, default <see langword="false"/></param>
        /// <param name="epilepsy">Include content that can cause epilepsy</param>
        /// <param name="page">Page index to request data</param>
        /// <param name="tags">Bitmask for tags filter.</param>
        /// <param name="styles">Bitmask for styles filter. Allowed values see in <see cref="SteamGridDbStyles.AllHeroes"/></param>
        /// <param name="dimensions">Bitmask for dimensions filter. Allowed values see in <see cref="SteamGridDbDimensions.AllHeroes"/></param>
        /// <param name="formats">Bitmask for formats/mimes filter. Allowed values see in <see cref="SteamGridDbFormats.All"/></param>
        /// <param name="types">Bitmask for type of image, animated or static. <see cref="SteamGridDbTypes.All"/></param>
        /// <returns><see cref="SteamGridDbGrid"/> array of results</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        public async Task<SteamGridDbHero[]> GetHeroesByGameIdAsync(int gameId, bool nsfw = false,
            bool humorous = false, bool epilepsy = false, int page = 0, SteamGridDbTags tags = SteamGridDbTags.None,
            SteamGridDbStyles styles = SteamGridDbStyles.AllHeroes,
            SteamGridDbDimensions dimensions = SteamGridDbDimensions.AllHeroes,
            SteamGridDbFormats formats = SteamGridDbFormats.All, SteamGridDbTypes types = SteamGridDbTypes.All)
        {
            if (styles.HasFlag(SteamGridDbStyles.AllIcons | SteamGridDbStyles.AllLogos))
                styles &= ~(SteamGridDbStyles.AllIcons | SteamGridDbStyles.AllLogos);
            if (dimensions.HasFlag(SteamGridDbDimensions.AllGrids))
                dimensions &= ~(SteamGridDbDimensions.AllGrids);
            var stylesFilter = SteamGridDbConstants.Styles.GetFromFlags(styles);
            var dimensionsFilter = SteamGridDbConstants.Dimensions.GetFromFlags(dimensions);
            var formatsFilter = SteamGridDbConstants.Mimes.GetFromFlags(formats);
            var typesFilter = SteamGridDbConstants.Types.GetFromFlags(types);

            string tagsParam = "";
            if (tags != SteamGridDbTags.None)
            {
                tagsParam = "&tags=" + SteamGridDbConstants.Tags.GetFromFlags(tags);
            }

            var response =
                await Get(
                    $"heroes/game/{gameId}?styles={stylesFilter}&dimensions={dimensionsFilter}&mimes={formatsFilter}&types={typesFilter}&nsfw={nsfw.ToString().ToLower()}&humor={humorous.ToString().ToLower()}&epilepsy={epilepsy.ToString().ToLower()}&page={page}{tagsParam}");
            if (response.Data != null)
            {
                var objects = response.Data.ToObject<SteamGridDbHero[]>();
                foreach (var obj in objects)
                {
                    obj.ApiInstance = this;
                }

                return objects;
            }
            return null;
        }

        /// <summary>
        /// Gets <see cref="SteamGridDbHero"/> array for specified game by selected platform and platform specific Game ID (like Steam App ID) with specified filters
        /// </summary>
        /// <param name="platform">Platform of which items get</param>
        /// <param name="platformGameId">Platform specific game ID of game</param>
        /// <param name="nsfw">Include Non-Suitable-For-Work results, default <see langword="false"/></param>
        /// <param name="humorous">Include humorous results, default <see langword="false"/></param>
        /// <param name="epilepsy">Include content that can cause epilepsy</param>
        /// <param name="page">Page index to request data</param>
        /// <param name="tags">Bitmask for tags filter.</param>
        /// <param name="styles">Bitmask for styles filter. Allowed values see in <see cref="SteamGridDbStyles.AllHeroes"/></param>
        /// <param name="dimensions">Bitmask for dimensions filter. Allowed values see in <see cref="SteamGridDbDimensions.AllHeroes"/></param>
        /// <param name="formats">Bitmask for formats/mimes filter. Allowed values see in <see cref="SteamGridDbFormats.All"/></param>
        /// <param name="types">Bitmask for type of image, animated or static. <see cref="SteamGridDbTypes.All"/></param>
        /// <returns><see cref="SteamGridDbGrid"/> array of results</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        /// <exception cref="ArgumentException">When more than one platform selected</exception>
        public async Task<SteamGridDbHero[]> GetHeroesByPlatformGameIdAsync(SteamGridDbGamePlatform platform,
            int platformGameId, bool nsfw = false, bool humorous = false, bool epilepsy = false, int page = 0,
            SteamGridDbTags tags = SteamGridDbTags.None, SteamGridDbStyles styles = SteamGridDbStyles.AllHeroes,
            SteamGridDbDimensions dimensions = SteamGridDbDimensions.AllHeroes,
            SteamGridDbFormats formats = SteamGridDbFormats.All, SteamGridDbTypes types = SteamGridDbTypes.All)
        {
            if (styles.HasFlag(SteamGridDbStyles.AllIcons | SteamGridDbStyles.AllLogos))
                styles &= ~(SteamGridDbStyles.AllIcons | SteamGridDbStyles.AllLogos);
            if (dimensions.HasFlag(SteamGridDbDimensions.AllGrids))
                dimensions &= ~(SteamGridDbDimensions.AllGrids);
            if (platform.MoreThanOneFlag())
                throw new ArgumentException(Resources.Resources.Exception_MoreThanOnePlatformSelected, nameof(platform));
            var platforms = SteamGridDbConstants.Platforms.GetFromFlags(platform);
            var stylesFilter = SteamGridDbConstants.Styles.GetFromFlags(styles);
            var dimensionsFilter = SteamGridDbConstants.Dimensions.GetFromFlags(dimensions);
            var formatsFilter = SteamGridDbConstants.Mimes.GetFromFlags(formats);
            var typesFilter = SteamGridDbConstants.Types.GetFromFlags(types);

            string tagsParam = "";
            if (tags != SteamGridDbTags.None)
            {
                tagsParam = "&tags=" + SteamGridDbConstants.Tags.GetFromFlags(tags);
            }

            var response = await Get($"heroes/{platforms}/{platformGameId}?styles={stylesFilter}&dimensions={dimensionsFilter}&mimes={formatsFilter}&types={typesFilter}&nsfw={nsfw.ToString().ToLower()}&humor={humorous.ToString().ToLower()}&epilepsy={epilepsy.ToString().ToLower()}&page={page}{tagsParam}");
            if (response.Data != null)
            {
                var objects = response.Data.ToObject<SteamGridDbHero[]>();
                foreach (var obj in objects)
                {
                    obj.ApiInstance = this;
                }

                return objects;
            }
            return null;
        }

        /// <summary>
        /// Gets <see cref="SteamGridDbHero"/> array for specified game with specified filters
        /// </summary>
        /// <param name="game"><see cref="SteamGridDbGame"/> object for game data</param>
        /// <param name="nsfw">Include Non-Suitable-For-Work results, default <see langword="false"/></param>
        /// <param name="humorous">Include humorous results, default <see langword="false"/></param>
        /// <param name="epilepsy">Include content that can cause epilepsy</param>
        /// <param name="page">Page index to request data</param>
        /// <param name="tags">Bitmask for tags filter.</param>
        /// <param name="styles">Bitmask for styles filter. Allowed values see in <see cref="SteamGridDbStyles.AllGrids"/></param>
        /// <param name="dimensions">Bitmask for dimensions filter. Allowed values see in <see cref="SteamGridDbDimensions.AllGrids"/></param>
        /// <param name="formats">Bitmask for formats/mimes filter. Allowed values see in <see cref="SteamGridDbFormats.All"/></param>
        /// <param name="types">Bitmask for type of image, animated or static. <see cref="SteamGridDbTypes.All"/></param>
        /// <returns><see cref="SteamGridDbGrid"/> array of results</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        public async Task<SteamGridDbHero[]> GetHeroesForGameAsync(SteamGridDbGame game, bool nsfw = false,
            bool humorous = false, bool epilepsy = false, int page = 0, SteamGridDbTags tags = SteamGridDbTags.None,
            SteamGridDbStyles styles = SteamGridDbStyles.AllGrids,
            SteamGridDbDimensions dimensions = SteamGridDbDimensions.AllGrids,
            SteamGridDbFormats formats = SteamGridDbFormats.All, SteamGridDbTypes types = SteamGridDbTypes.All)
        {
            return await GetHeroesByGameIdAsync(game.Id, nsfw, humorous, epilepsy, page, tags, styles, dimensions,
                formats, types);
        }

        /// <summary>
        /// Uploads image from <see cref="Stream"/> as <see cref="SteamGridDbHero"/> to SteamGridDB
        /// </summary>
        /// <param name="gameId">SteamGridDB Game ID</param>
        /// <param name="imageStream"><see cref="Stream"/> of data, that represents an image. Must contain data following MIME types: image/png, image/jpeg</param>
        /// <param name="style">Style of Hero image for filters</param>
        /// <returns><see langword="true"/> if image uploaded correctly, otherwise <see langword="false"/> or exception will be thrown</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        /// <exception cref="ArgumentException">When more then one style selected</exception>
        /// <exception cref="ArgumentOutOfRangeException">When stream is empty or has length of 0</exception>
        /// <exception cref="InvalidMimeTypeException">When data in stream doesn't represent correct MIME type</exception>
        public async Task<bool> UploadHeroAsync(int gameId, Stream imageStream,
            SteamGridDbStyles style = SteamGridDbStyles.Alternate)
        {
            if (style.MoreThanOneFlag())
                throw new ArgumentException(Resources.Resources.Exception_MoreThanOneStyleSelected, nameof(style));
            if (!style.HasFlag(SteamGridDbStyles.Alternate) && !style.HasFlag(SteamGridDbStyles.Blurred) && !style.HasFlag(SteamGridDbStyles.Material))
                throw new ArgumentException(string.Format(Resources.Resources.Exception_InvalidStyleSelected,
                    style.ToString()));

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
                throw new InvalidMimeTypeException(string.Format(Resources.Resources.Exception_InvalidMimeType, string.Join(",", SteamGridDbConstants.Mimes.Png, SteamGridDbConstants.Mimes.Png)));

            var streamContent = new StreamContent(imageStream);
            streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse(mimeType);
            content.Add(streamContent, "asset", "image." + ext);
            var response = await Post("heroes", content);
            return response.Success;
        }

        /// <summary>
        /// Uploads image from <see cref="Stream"/> as <see cref="SteamGridDbHero"/> to SteamGridDB
        /// </summary>
        /// <param name="game"><see cref="SteamGridDbGame"/> object for game data</param>
        /// <param name="imageStream"><see cref="Stream"/> of data, that represents an image. Must contain data following MIME types: image/png, image/jpeg</param>
        /// <param name="style">Style of Hero image for filters</param>
        /// <returns><see langword="true"/> if image uploaded correctly, otherwise <see langword="false"/> or exception will be thrown</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        /// <exception cref="ArgumentException">When more then one style selected</exception>
        /// <exception cref="ArgumentOutOfRangeException">When stream is empty or has length of 0</exception>
        /// <exception cref="InvalidMimeTypeException">When data in stream doesn't represent correct MIME type</exception>
        public async Task<bool> UploadHeroAsync(SteamGridDbGame game, Stream imageStream,
            SteamGridDbStyles style = SteamGridDbStyles.Alternate)
        {
            return await UploadHeroAsync(game.Id, imageStream, style);
        }

        /// <summary>
        /// Uploads image from file as <see cref="SteamGridDbHero"/> to SteamGridDB
        /// </summary>
        /// <param name="gameId">SteamGridDB Game ID</param>
        /// <param name="filePath">Full path to the file that represents an image. Must contain data following MIME types: image/png, image/jpeg</param>
        /// <param name="style">Style of Hero image for filters</param>
        /// <returns><see langword="true"/> if image uploaded correctly, otherwise <see langword="false"/> or exception will be thrown</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        /// <exception cref="ArgumentException">When more then one style selected</exception>
        /// <exception cref="ArgumentOutOfRangeException">When stream is empty or has length of 0</exception>
        /// <exception cref="InvalidMimeTypeException">When data in stream doesn't represent correct MIME type</exception>
        public async Task<bool> UploadHeroFromFileAsync(int gameId, string filePath,
            SteamGridDbStyles style = SteamGridDbStyles.Alternate)
        {
            using (FileStream fileStream = File.OpenRead(filePath))
                return await UploadHeroAsync(gameId, fileStream, style);
        }

        /// <summary>
        /// Uploads image from file as <see cref="SteamGridDbHero"/> to SteamGridDB
        /// </summary>
        /// <param name="game"><see cref="SteamGridDbGame"/> object for game data</param>
        /// <param name="filePath">Full path to the file that represents an image. Must contain data following MIME types: image/png, image/jpeg</param>
        /// <param name="style">Style of Hero image for filters</param>
        /// <returns><see langword="true"/> if image uploaded correctly, otherwise <see langword="false"/> or exception will be thrown</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        /// <exception cref="ArgumentException">When more then one style selected</exception>
        /// <exception cref="ArgumentOutOfRangeException">When stream is empty or has length of 0</exception>
        /// <exception cref="InvalidMimeTypeException">When data in stream doesn't represent correct MIME type</exception>
        public async Task<bool> UploadHeroFromFileAsync(SteamGridDbGame game, string filePath,
            SteamGridDbStyles style = SteamGridDbStyles.Alternate)
        {
            return await UploadHeroFromFileAsync(game.Id, filePath, style);
        }

        /// <summary>
        /// Removes <see cref="SteamGridDbHero"/> with specified IDs from server
        /// </summary>
        /// <param name="heroIds">List of IDs of <see cref="SteamGridDbHero"/> objects</param>
        /// <returns><see langword="true"/> if successfully removed, otherwise <see langword="false"/> or thrown exception</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        public async Task<bool> DeleteHeroesAsync(params int[] heroIds)
        {
            string ids = string.Join(",", heroIds);
            var response = await Delete("heroes/" + ids);
            return response.Success;
        }

        /// <summary>
        /// Removes single <see cref="SteamGridDbHero"/> with specified ID from server
        /// </summary>
        /// <param name="heroId">ID of <see cref="SteamGridDbHero"/> object to remove</param>
        /// <returns><see langword="true"/> if successfully removed, otherwise <see langword="false"/> or thrown exception</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        public async Task<bool> DeleteHeroAsync(int heroId)
        {
            return await DeleteHeroesAsync(heroId);
        }

        #endregion

        #region Logos

        /// <summary>
        /// Gets <see cref="SteamGridDbLogo"/> array for specified game with specified filters
        /// </summary>
        /// <param name="gameId">SteamGridDB game ID of game</param>
        /// <param name="nsfw">Include Non-Suitable-For-Work results, default <see langword="false"/></param>
        /// <param name="humorous">Include humorous results, default <see langword="false"/></param>
        /// <param name="epilepsy">Include content that can cause epilepsy</param>
        /// <param name="page">Page index to request data</param>
        /// <param name="tags">Bitmask for tags filter.</param>
        /// <param name="styles">Bitmask for styles filter. Allowed values see in <see cref="SteamGridDbStyles.AllLogos"/></param>
        /// <param name="formats">Bitmask for formats/mimes filter. Allowed values see in <see cref="SteamGridDbFormats.All"/></param>
        /// <param name="types">Bitmask for type of image, animated or static. <see cref="SteamGridDbTypes.All"/></param>
        /// <returns><see cref="SteamGridDbGrid"/> array of results</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        public async Task<SteamGridDbLogo[]> GetLogosByGameIdAsync(int gameId, bool nsfw = false, bool humorous = false,
            bool epilepsy = false, int page = 0, SteamGridDbTags tags = SteamGridDbTags.None,
            SteamGridDbStyles styles = SteamGridDbStyles.AllLogos,
            SteamGridDbFormats formats = SteamGridDbFormats.AllLogos, SteamGridDbTypes types = SteamGridDbTypes.All)
        {
            if (styles.HasFlag(SteamGridDbStyles.AllHeroes | SteamGridDbStyles.AllGrids))
                styles &= ~(SteamGridDbStyles.AllHeroes | SteamGridDbStyles.AllGrids);

            if (formats.HasFlag(SteamGridDbFormats.Jpeg))
                formats &= ~(SteamGridDbFormats.Jpeg);

            var stylesFilter = SteamGridDbConstants.Styles.GetFromFlags(styles);
            var formatsFilter = SteamGridDbConstants.Mimes.GetFromFlags(formats);
            var typesFilter = SteamGridDbConstants.Types.GetFromFlags(types);

            string tagsParam = "";
            if (tags != SteamGridDbTags.None)
            {
                tagsParam = "&tags=" + SteamGridDbConstants.Tags.GetFromFlags(tags);
            }

            var response = await Get($"logos/game/{gameId}?styles={stylesFilter}&mimes={formatsFilter}&types={typesFilter}&nsfw={nsfw.ToString().ToLower()}&humor={humorous.ToString().ToLower()}&epilepsy={epilepsy.ToString().ToLower()}&page={page}{tagsParam}");
            if (response.Data != null)
            {
                var objects = response.Data.ToObject<SteamGridDbLogo[]>();
                foreach (var obj in objects)
                {
                    obj.ApiInstance = this;
                }

                return objects;
            }
            return null;
        }

        /// <summary>
        /// Gets <see cref="SteamGridDbLogo"/> array for specified game by selected platform and platform specific Game ID (like Steam App ID) with specified filters
        /// </summary>
        /// <param name="platform">Platform of which items get</param>
        /// <param name="platformGameId">Platform specific game ID of game</param>
        /// <param name="nsfw">Include Non-Suitable-For-Work results, default <see langword="false"/></param>
        /// <param name="humorous">Include humorous results, default <see langword="false"/></param>/// <param name="epilepsy">Include content that can cause epilepsy</param>
        /// <param name="page">Page index to request data</param>
        /// <param name="tags">Bitmask for tags filter.</param>
        /// <param name="styles">Bitmask for styles filter. Allowed values see in <see cref="SteamGridDbStyles.AllLogos"/></param>
        /// <param name="formats">Bitmask for formats/mimes filter. Allowed values see in <see cref="SteamGridDbFormats.All"/></param>
        /// <param name="types">Bitmask for type of image, animated or static. <see cref="SteamGridDbTypes.All"/></param>
        /// <returns><see cref="SteamGridDbGrid"/> array of results</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        /// <exception cref="ArgumentException">When more than one platform selected</exception>
        public async Task<SteamGridDbLogo[]> GetLogosByPlatformGameIdAsync(SteamGridDbGamePlatform platform,
            int platformGameId, bool nsfw = false, bool humorous = false, bool epilepsy = false, int page = 0,
            SteamGridDbTags tags = SteamGridDbTags.None, SteamGridDbStyles styles = SteamGridDbStyles.AllLogos,
            SteamGridDbFormats formats = SteamGridDbFormats.AllLogos, SteamGridDbTypes types = SteamGridDbTypes.All)
        {
            if (styles.HasFlag(SteamGridDbStyles.AllHeroes | SteamGridDbStyles.AllGrids))
                styles &= ~(SteamGridDbStyles.AllHeroes | SteamGridDbStyles.AllGrids);

            if (formats.HasFlag(SteamGridDbFormats.Jpeg))
                formats &= ~(SteamGridDbFormats.Jpeg);

            if (platform.MoreThanOneFlag())
                throw new ArgumentException(Resources.Resources.Exception_MoreThanOnePlatformSelected, nameof(platform));

            var platforms = SteamGridDbConstants.Platforms.GetFromFlags(platform);
            var stylesFilter = SteamGridDbConstants.Styles.GetFromFlags(styles);
            var formatsFilter = SteamGridDbConstants.Mimes.GetFromFlags(formats);
            var typesFilter = SteamGridDbConstants.Types.GetFromFlags(types);

            string tagsParam = "";
            if (tags != SteamGridDbTags.None)
            {
                tagsParam = "&tags=" + SteamGridDbConstants.Tags.GetFromFlags(tags);
            }

            var response = await Get($"logos/{platforms}/{platformGameId}?styles={stylesFilter}&mimes={formatsFilter}&types={typesFilter}&nsfw={nsfw.ToString().ToLower()}&humor={humorous.ToString().ToLower()}&epilepsy={epilepsy.ToString().ToLower()}&page={page}{tagsParam}");
            if (response.Data != null)
            {
                var objects = response.Data.ToObject<SteamGridDbLogo[]>();
                foreach (var obj in objects)
                {
                    obj.ApiInstance = this;
                }

                return objects;
            }
            return null;
        }

        /// <summary>
        /// Gets <see cref="SteamGridDbLogo"/> array for specified game with specified filters
        /// </summary>
        /// <param name="game"><see cref="SteamGridDbGame"/> object for game data</param>
        /// <param name="nsfw">Include Non-Suitable-For-Work results, default <see langword="false"/></param>
        /// <param name="humorous">Include humorous results, default <see langword="false"/></param>
        /// <param name="epilepsy">Include content that can cause epilepsy</param>
        /// <param name="page">Page index to request data</param>
        /// <param name="tags">Bitmask for tags filter.</param>
        /// <param name="styles">Bitmask for styles filter. Allowed values see in <see cref="SteamGridDbStyles.AllGrids"/></param>
        /// <param name="dimensions">Bitmask for dimensions filter. Allowed values see in <see cref="SteamGridDbDimensions.AllGrids"/></param>
        /// <param name="formats">Bitmask for formats/mimes filter. Allowed values see in <see cref="SteamGridDbFormats.All"/></param>
        /// <param name="types">Bitmask for type of image, animated or static. <see cref="SteamGridDbTypes.All"/></param>
        /// <returns><see cref="SteamGridDbLogo"/> array of results</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        public async Task<SteamGridDbLogo[]> GetLogosForGameAsync(SteamGridDbGame game, bool nsfw = false,
            bool humorous = false, bool epilepsy = false, int page = 0, SteamGridDbTags tags = SteamGridDbTags.None,
            SteamGridDbStyles styles = SteamGridDbStyles.AllGrids,
            SteamGridDbFormats formats = SteamGridDbFormats.All, SteamGridDbTypes types = SteamGridDbTypes.All)
        {
            return await GetLogosByGameIdAsync(game.Id, nsfw, humorous, epilepsy, page, tags, styles, formats, types);
        }

        /// <summary>
        /// Uploads image from <see cref="Stream"/> as <see cref="SteamGridDbLogo"/> to SteamGridDB
        /// </summary>
        /// <param name="gameId">SteamGridDB Game ID</param>
        /// <param name="imageStream"><see cref="Stream"/> of data, that represents an image. Must contain data following MIME types: image/png</param>
        /// <param name="style">Style of Hero image for filters</param>
        /// <returns><see langword="true"/> if image uploaded correctly, otherwise <see langword="false"/> or exception will be thrown</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        /// <exception cref="ArgumentOutOfRangeException">When stream is empty or has length of 0</exception>
        /// <exception cref="InvalidMimeTypeException">When data in stream doesn't represent correct MIME type</exception>
        public async Task<bool> UploadLogoAsync(int gameId, Stream imageStream, SteamGridDbStyles style = SteamGridDbStyles.Custom)
        {
            if (style.MoreThanOneFlag())
                throw new ArgumentException(Resources.Resources.Exception_MoreThanOneStyleSelected, nameof(style));
            if (!style.HasFlag(SteamGridDbStyles.Official) && !style.HasFlag(SteamGridDbStyles.Black) && !style.HasFlag(SteamGridDbStyles.White) && !style.HasFlag(SteamGridDbStyles.Custom))
                throw new ArgumentException(string.Format(Resources.Resources.Exception_InvalidStyleSelected,
                    style.ToString()));

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

            if (MimeHelper.ValidateMimeType(signature, SteamGridDbConstants.Mimes.Png))
            {
                ext = "png";
                mimeType = SteamGridDbConstants.Mimes.Png;
            }
            if (string.IsNullOrWhiteSpace(mimeType))
                throw new InvalidMimeTypeException(string.Format(Resources.Resources.Exception_InvalidMimeType, string.Join(",", SteamGridDbConstants.Mimes.Png, SteamGridDbConstants.Mimes.Png)));

            var streamContent = new StreamContent(imageStream);
            streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse(mimeType);
            content.Add(streamContent, "asset", "image." + ext);
            var response = await Post("logos", content);
            return response.Success;
        }

        /// <summary>
        /// Uploads image from <see cref="Stream"/> as <see cref="SteamGridDbLogo"/> to SteamGridDB
        /// </summary>
        /// <param name="game"><see cref="SteamGridDbGame"/> object for game data</param>
        /// <param name="imageStream"><see cref="Stream"/> of data, that represents an image. Must contain data following MIME types: image/png</param>
        /// <param name="style">Style of Hero image for filters</param>
        /// <returns><see langword="true"/> if image uploaded correctly, otherwise <see langword="false"/> or exception will be thrown</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        /// <exception cref="ArgumentOutOfRangeException">When stream is empty or has length of 0</exception>
        /// <exception cref="InvalidMimeTypeException">When data in stream doesn't represent correct MIME type</exception>
        public async Task<bool> UploadLogoAsync(SteamGridDbGame game, Stream imageStream,
            SteamGridDbStyles style = SteamGridDbStyles.Custom)
        {
            return await UploadLogoAsync(game.Id, imageStream, style);
        }

        /// <summary>
        /// Uploads image from file as <see cref="SteamGridDbLogo"/> to SteamGridDB
        /// </summary>
        /// <param name="gameId">SteamGridDB Game ID</param>
        /// <param name="filePath">Full path to the file that represents an image. Must contain data following MIME types: image/png</param>
        /// <param name="style">Style of Hero image for filters</param>
        /// <returns><see langword="true"/> if image uploaded correctly, otherwise <see langword="false"/> or exception will be thrown</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        /// <exception cref="ArgumentOutOfRangeException">When stream is empty or has length of 0</exception>
        /// <exception cref="InvalidMimeTypeException">When data in stream doesn't represent correct MIME type</exception>
        public async Task<bool> UploadLogoFromFileAsync(int gameId, string filePath, SteamGridDbStyles style = SteamGridDbStyles.Custom)
        {
            using (FileStream fileStream = File.OpenRead(filePath))
                return await UploadLogoAsync(gameId, fileStream, style);
        }

        /// <summary>
        /// Uploads image from file as <see cref="SteamGridDbLogo"/> to SteamGridDB
        /// </summary>
        /// <param name="game"><see cref="SteamGridDbGame"/> object for game data</param>
        /// <param name="filePath">Full path to the file that represents an image. Must contain data following MIME types: image/png</param>
        /// <param name="style">Style of Hero image for filters</param>
        /// <returns><see langword="true"/> if image uploaded correctly, otherwise <see langword="false"/> or exception will be thrown</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        /// <exception cref="ArgumentOutOfRangeException">When stream is empty or has length of 0</exception>
        /// <exception cref="InvalidMimeTypeException">When data in stream doesn't represent correct MIME type</exception>
        public async Task<bool> UploadLogoFromFileAsync(SteamGridDbGame game, string filePath,
            SteamGridDbStyles style = SteamGridDbStyles.Custom)
        {
            return await UploadLogoFromFileAsync(game.Id, filePath, style);
        }

        /// <summary>
        /// Removes <see cref="SteamGridDbLogo"/> with specified IDs from server
        /// </summary>
        /// <param name="logoIds">List of IDs of <see cref="SteamGridDbLogo"/> objects</param>
        /// <returns><see langword="true"/> if successfully removed, otherwise <see langword="false"/> or thrown exception</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        public async Task<bool> DeleteLogosAsync(params int[] logoIds)
        {
            string ids = string.Join(",", logoIds);
            var response = await Delete("logos/" + ids);
            return response.Success;
        }

        /// <summary>
        /// Removes single <see cref="SteamGridDbLogo"/> with specified ID from server
        /// </summary>
        /// <param name="logoId">ID of <see cref="SteamGridDbLogo"/> object to remove</param>
        /// <returns><see langword="true"/> if successfully removed, otherwise <see langword="false"/> or thrown exception</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        public async Task<bool> DeleteLogoAsync(int logoId)
        {
            return await DeleteLogosAsync(logoId);
        }

        #endregion

        #region Icons

        /// <summary>
        /// Gets <see cref="SteamGridDbIcon"/> array for specified game with specified filters
        /// </summary>
        /// <param name="gameId">SteamGridDB game ID of game</param>
        /// <param name="nsfw">Include Non-Suitable-For-Work results, default <see langword="false"/></param>
        /// <param name="humorous">Include humorous results, default <see langword="false"/></param>
        /// <param name="epilepsy">Include content that can cause epilepsy</param>
        /// <param name="page">Page index to request data</param>
        /// <param name="tags">Bitmask for tags filter.</param>
        /// <param name="styles">Bitmask for styles filter. Allowed values see in <see cref="SteamGridDbStyles.AllIcons"/></param>
        /// <param name="formats">Bitmask for formats/mimes filter. Allowed values see in <see cref="SteamGridDbFormats.AllIcons"/></param>
        /// <param name="types">Bitmask for type of image, animated or static. <see cref="SteamGridDbTypes.All"/></param>
        /// <returns><see cref="SteamGridDbGrid"/> array of results</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        public async Task<SteamGridDbIcon[]> GetIconsByGameIdAsync(int gameId, bool nsfw = false, bool humorous = false,
            bool epilepsy = false, int page = 0, SteamGridDbTags tags = SteamGridDbTags.None,
            SteamGridDbStyles styles = SteamGridDbStyles.AllIcons,
            SteamGridDbFormats formats = SteamGridDbFormats.AllIcons, SteamGridDbTypes types = SteamGridDbTypes.All)
        {
            if (styles.HasFlag(SteamGridDbStyles.AllGrids | SteamGridDbStyles.Black | SteamGridDbStyles.White))
                styles &= ~(SteamGridDbStyles.AllGrids | SteamGridDbStyles.Black | SteamGridDbStyles.White);

            if (formats.HasFlag(SteamGridDbFormats.Jpeg | SteamGridDbFormats.Webp))
                formats &= ~(SteamGridDbFormats.Jpeg | SteamGridDbFormats.Webp);

            var stylesFilter = SteamGridDbConstants.Styles.GetFromFlags(styles);
            var formatsFilter = SteamGridDbConstants.Mimes.GetFromFlags(formats);
            var typesFilter = SteamGridDbConstants.Types.GetFromFlags(types);

            string tagsParam = "";
            if (tags != SteamGridDbTags.None)
            {
                tagsParam = "&tags=" + SteamGridDbConstants.Tags.GetFromFlags(tags);
            }

            var response = await Get($"icons/game/{gameId}?styles={stylesFilter}&mimes={formatsFilter}&types={typesFilter}&nsfw={nsfw.ToString().ToLower()}&humor={humorous.ToString().ToLower()}&epilepsy={epilepsy.ToString().ToLower()}&page={page}{tagsParam}");
            if (response.Data != null)
            {
                var objects = response.Data.ToObject<SteamGridDbIcon[]>();
                foreach (var obj in objects)
                {
                    obj.ApiInstance = this;
                }

                return objects;
            }
            return null;
        }

        /// <summary>
        /// Gets <see cref="SteamGridDbIcon"/> array for specified game by selected platform and platform specific Game ID (like Steam App ID) with specified filters
        /// </summary>
        /// <param name="platform">Platform of which items get</param>
        /// <param name="platformGameId">Platform specific game ID of game</param>
        /// <param name="nsfw">Include Non-Suitable-For-Work results, default <see langword="false"/></param>
        /// <param name="humorous">Include humorous results, default <see langword="false"/></param>
        /// <param name="epilepsy">Include content that can cause epilepsy</param>
        /// <param name="page">Page index to request data</param>
        /// <param name="tags">Bitmask for tags filter.</param>
        /// <param name="styles">Bitmask for styles filter. Allowed values see in <see cref="SteamGridDbStyles.AllIcons"/></param>
        /// <param name="formats">Bitmask for formats/mimes filter. Allowed values see in <see cref="SteamGridDbFormats.AllIcons"/></param>
        /// <param name="types">Bitmask for type of image, animated or static. <see cref="SteamGridDbTypes.All"/></param>
        /// <returns><see cref="SteamGridDbGrid"/> array of results</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        /// <exception cref="ArgumentException">When more than one platform selected</exception>
        public async Task<SteamGridDbIcon[]> GetIconsByPlatformGameIdAsync(SteamGridDbGamePlatform platform,
            int platformGameId, bool nsfw = false, bool humorous = false, bool epilepsy = false, int page = 0,
            SteamGridDbTags tags = SteamGridDbTags.None, SteamGridDbStyles styles = SteamGridDbStyles.AllIcons,
            SteamGridDbFormats formats = SteamGridDbFormats.AllIcons, SteamGridDbTypes types = SteamGridDbTypes.All)
        {
            if (styles.HasFlag(SteamGridDbStyles.AllGrids | SteamGridDbStyles.Black | SteamGridDbStyles.White))
                styles &= ~(SteamGridDbStyles.AllGrids | SteamGridDbStyles.Black | SteamGridDbStyles.White);

            if (formats.HasFlag(SteamGridDbFormats.Jpeg | SteamGridDbFormats.Webp))
                formats &= ~(SteamGridDbFormats.Jpeg | SteamGridDbFormats.Webp);

            if (platform.MoreThanOneFlag())
                throw new ArgumentException(Resources.Resources.Exception_MoreThanOnePlatformSelected, nameof(platform));

            var platforms = SteamGridDbConstants.Platforms.GetFromFlags(platform);
            var stylesFilter = SteamGridDbConstants.Styles.GetFromFlags(styles);
            var formatsFilter = SteamGridDbConstants.Mimes.GetFromFlags(formats);
            var typesFilter = SteamGridDbConstants.Types.GetFromFlags(types);

            string tagsParam = "";
            if (tags != SteamGridDbTags.None)
            {
                tagsParam = "&tags=" + SteamGridDbConstants.Tags.GetFromFlags(tags);
            }

            var response = await Get($"icons/{platforms}/{platformGameId}?styles={stylesFilter}&mimes={formatsFilter}&types={typesFilter}&nsfw={nsfw.ToString().ToLower()}&humor={humorous.ToString().ToLower()}&epilepsy={epilepsy.ToString().ToLower()}&page={page}{tagsParam}");
            if (response.Data != null)
            {
                var objects = response.Data.ToObject<SteamGridDbIcon[]>();
                foreach (var obj in objects)
                {
                    obj.ApiInstance = this;
                }

                return objects;
            }
            return null;
        }

        /// <summary>
        /// Gets <see cref="SteamGridDbIcon"/> array for specified game with specified filters
        /// </summary>
        /// <param name="game"><see cref="SteamGridDbGame"/> object for game data</param>
        /// <param name="nsfw">Include Non-Suitable-For-Work results, default <see langword="false"/></param>
        /// <param name="humorous">Include humorous results, default <see langword="false"/></param>
        /// <param name="epilepsy">Include content that can cause epilepsy</param>
        /// <param name="page">Page index to request data</param>
        /// <param name="tags">Bitmask for tags filter.</param>
        /// <param name="styles">Bitmask for styles filter. Allowed values see in <see cref="SteamGridDbStyles.AllIcons"/></param>
        /// <param name="formats">Bitmask for formats/mimes filter. Allowed values see in <see cref="SteamGridDbFormats.AllIcons"/></param>
        /// <param name="types">Bitmask for type of image, animated or static. <see cref="SteamGridDbTypes.All"/></param>
        /// <returns><see cref="SteamGridDbGrid"/> array of results</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        public async Task<SteamGridDbIcon[]> GetIconsForGameAsync(SteamGridDbGame game, bool nsfw = false,
            bool humorous = false, bool epilepsy = false, int page = 0, SteamGridDbTags tags = SteamGridDbTags.None,
            SteamGridDbStyles styles = SteamGridDbStyles.AllIcons,
            SteamGridDbFormats formats = SteamGridDbFormats.AllIcons, SteamGridDbTypes types = SteamGridDbTypes.All)
        {
            return await GetIconsByGameIdAsync(game.Id, nsfw, humorous, epilepsy, page, tags, styles, formats, types);
        }

        /// <summary>
        /// Uploads image from <see cref="Stream"/> as <see cref="SteamGridDbIcon"/> to SteamGridDB
        /// </summary>
        /// <param name="gameId">SteamGridDB Game ID</param>
        /// <param name="imageStream"><see cref="Stream"/> of data, that represents an image. Must contain data following MIME types: image/png, image/vnd.microsoft.icon (.ico file)</param>
        /// <param name="style">Style of Icon image for filters</param>
        /// <returns><see langword="true"/> if image uploaded correctly, otherwise <see langword="false"/> or exception will be thrown</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        /// <exception cref="ArgumentOutOfRangeException">When stream is empty or has length of 0</exception>
        /// <exception cref="InvalidMimeTypeException">When data in stream doesn't represent correct MIME type</exception>
        public async Task<bool> UploadIconAsync(int gameId, Stream imageStream, SteamGridDbStyles style = SteamGridDbStyles.Custom)
        {
            if (style.MoreThanOneFlag())
                throw new ArgumentException(Resources.Resources.Exception_MoreThanOneStyleSelected, nameof(style));
            if (!style.HasFlag(SteamGridDbStyles.Official) && !style.HasFlag(SteamGridDbStyles.Custom))
                throw new ArgumentException(string.Format(Resources.Resources.Exception_InvalidStyleSelected,
                    style.ToString()));

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

            if (MimeHelper.ValidateMimeType(signature, SteamGridDbConstants.Mimes.Png))
            {
                ext = "png";
                mimeType = SteamGridDbConstants.Mimes.Png;
            }
            if (MimeHelper.ValidateMimeType(signature, SteamGridDbConstants.Mimes.Ico))
            {
                ext = "ico";
                mimeType = SteamGridDbConstants.Mimes.Ico;
            }
            if (string.IsNullOrWhiteSpace(mimeType))
                throw new InvalidMimeTypeException(string.Format(Resources.Resources.Exception_InvalidMimeType, string.Join(",", SteamGridDbConstants.Mimes.Png, SteamGridDbConstants.Mimes.Png)));

            var streamContent = new StreamContent(imageStream);
            streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse(mimeType);
            content.Add(streamContent, "asset", "image." + ext);
            var response = await Post("icons", content);
            return response.Success;
        }

        /// <summary>
        /// Uploads image from <see cref="Stream"/> as <see cref="SteamGridDbIcon"/> to SteamGridDB
        /// </summary>
        /// <param name="game"><see cref="SteamGridDbGame"/> object for game data</param>
        /// <param name="imageStream"><see cref="Stream"/> of data, that represents an image. Must contain data following MIME types: image/png, image/vnd.microsoft.icon (.ico file)</param>
        /// <param name="style">Style of Icon image for filters</param>
        /// <returns><see langword="true"/> if image uploaded correctly, otherwise <see langword="false"/> or exception will be thrown</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        /// <exception cref="ArgumentOutOfRangeException">When stream is empty or has length of 0</exception>
        /// <exception cref="InvalidMimeTypeException">When data in stream doesn't represent correct MIME type</exception>
        public async Task<bool> UploadIconAsync(SteamGridDbGame game, Stream imageStream,
            SteamGridDbStyles style = SteamGridDbStyles.Custom)
        {
            return await UploadIconAsync(game.Id, imageStream, style);
        }

        /// <summary>
        /// Uploads image from file as <see cref="SteamGridDbGrid"/> to SteamGridDB
        /// </summary>
        /// <param name="gameId">SteamGridDB Game ID</param>
        /// <param name="filePath">Full path to the file that represents an image. Must contain data following MIME types: image/png, image/vnd.microsoft.icon (.ico file)</param>
        /// <param name="style">Style of Hero image for filters</param>
        /// <returns><see langword="true"/> if image uploaded correctly, otherwise <see langword="false"/> or exception will be thrown</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        /// <exception cref="ArgumentOutOfRangeException">When stream is empty or has length of 0</exception>
        /// <exception cref="InvalidMimeTypeException">When data in stream doesn't represent correct MIME type</exception>
        public async Task<bool> UploadIconFromFileAsync(int gameId, string filePath, SteamGridDbStyles style = SteamGridDbStyles.Custom)
        {
            using (FileStream fileStream = File.OpenRead(filePath))
                return await UploadIconAsync(gameId, fileStream, style);
        }

        /// <summary>
        /// Uploads image from file as <see cref="SteamGridDbGrid"/> to SteamGridDB
        /// </summary>
        /// <param name="game"><see cref="SteamGridDbGame"/> object for game data</param>
        /// <param name="filePath">Full path to the file that represents an image. Must contain data following MIME types: image/png, image/vnd.microsoft.icon (.ico file)</param>
        /// <param name="style">Style of Hero image for filters</param>
        /// <returns><see langword="true"/> if image uploaded correctly, otherwise <see langword="false"/> or exception will be thrown</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        /// <exception cref="ArgumentOutOfRangeException">When stream is empty or has length of 0</exception>
        /// <exception cref="InvalidMimeTypeException">When data in stream doesn't represent correct MIME type</exception>
        public async Task<bool> UploadIconFromFileAsync(SteamGridDbGame game, string filePath,
            SteamGridDbStyles style = SteamGridDbStyles.Custom)
        {
            return await UploadIconFromFileAsync(game.Id, filePath, style);
        }

        /// <summary>
        /// Removes <see cref="SteamGridDbIcon"/> with specified IDs from server
        /// </summary>
        /// <param name="iconIds">List of IDs of <see cref="SteamGridDbIcon"/> objects</param>
        /// <returns><see langword="true"/> if successfully removed, otherwise <see langword="false"/> or thrown exception</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        public async Task<bool> DeleteIconsAsync(params int[] iconIds)
        {
            string ids = string.Join(",", iconIds);
            var response = await Delete("icons/" + ids);
            return response.Success;
        }

        /// <summary>
        /// Removes single <see cref="SteamGridDbIcon"/> with specified ID from server
        /// </summary>
        /// <param name="iconId">ID of <see cref="SteamGridDbIcon"/> object to remove</param>
        /// <returns><see langword="true"/> if successfully removed, otherwise <see langword="false"/> or thrown exception</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        public async Task<bool> DeleteIconAsync(int iconId)
        {
            return await DeleteIconsAsync(iconId);
        }

        #endregion

        /// <summary>
        /// Searches for <see cref="SteamGridDbGame"/> object array by specified search term
        /// </summary>
        /// <param name="searchTerm">Search term query string</param>
        /// <returns>Array of <see cref="SteamGridDbGame"/> objects, otherwise empty array or null</returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbRateLimitedException">When you've been rate limited by the server</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        public async Task<SteamGridDbGame[]> SearchForGamesAsync(string searchTerm)
        {
            var response = await Get($"search/autocomplete/{searchTerm}");
            if (response.Data != null) return response.Data.ToObject<SteamGridDbGame[]>();
            return null;
        }

        #region Internal

        private async Task<SteamGridDbResponse> Get(string uri)
        {
            var response = await _httpClient.GetAsync(uri);
            var str = await response.Content.ReadAsStringAsync();
            var respObj = JsonConvert.DeserializeObject<SteamGridDbResponse>(str);
            var errors = new string[] { };

            if (respObj != null && respObj.Success)
                return respObj;

            if (respObj != null)
                errors = respObj.Errors;

            if (!(response.Headers.RetryAfter is null) && response.Headers.RetryAfter.Delta.HasValue)
            {
                throw new SteamGridDbRateLimitedException(
                    string.Format(Resources.Resources.Exception_RateLimited,
                        response.Headers.RetryAfter.Delta.ToString()), response.Headers.RetryAfter.Delta.Value) {ExceptionType = ExceptionType.RateLimited, SteamGridDbErrorMessages = errors };
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new SteamGridDbBadRequestException(Resources.Resources.Exception_BadRequest)
                        { ExceptionType = ExceptionType.BadRequest, SteamGridDbErrorMessages = errors };
                case HttpStatusCode.Forbidden:
                    throw new SteamGridDbForbiddenException(Resources.Resources.Exception_Forbidden)
                        { ExceptionType = ExceptionType.Forbidden, SteamGridDbErrorMessages = errors };
                case HttpStatusCode.NotFound:
                    throw new SteamGridDbNotFoundException(Resources.Resources.Exception_NotFound)
                        { ExceptionType = ExceptionType.NotFound, SteamGridDbErrorMessages = errors };
                case HttpStatusCode.Unauthorized:
                    throw new SteamGridDbUnauthorizedException(Resources.Resources.Exception_Unauthorized)
                        { ExceptionType = ExceptionType.Unauthorized, SteamGridDbErrorMessages = errors };
            }

            throw new SteamGridDbException(Resources.Resources.Exception_Unknown);
        }

        private async Task<SteamGridDbResponse> Post(string uri, MultipartFormDataContent content)
        {
            var response = await _httpClient.PostAsync(uri, content);
            var str = await response.Content.ReadAsStringAsync();
            var respObj = JsonConvert.DeserializeObject<SteamGridDbResponse>(str);
            var errors = new string[] { };

            if (respObj != null && respObj.Success)
                return respObj;

            if (respObj != null)
                errors = respObj.Errors;
            
            if (!(response.Headers.RetryAfter is null) && response.Headers.RetryAfter.Delta.HasValue)
            {
                throw new SteamGridDbRateLimitedException(
                    string.Format(Resources.Resources.Exception_RateLimited,
                        response.Headers.RetryAfter.Delta.ToString()), response.Headers.RetryAfter.Delta.Value) {ExceptionType = ExceptionType.RateLimited, SteamGridDbErrorMessages = errors };
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new SteamGridDbBadRequestException(Resources.Resources.Exception_BadRequest)
                        { ExceptionType = ExceptionType.BadRequest, SteamGridDbErrorMessages = errors };
                case HttpStatusCode.Forbidden:
                    throw new SteamGridDbForbiddenException(Resources.Resources.Exception_Forbidden)
                        { ExceptionType = ExceptionType.Forbidden, SteamGridDbErrorMessages = errors };
                case HttpStatusCode.NotFound:
                    throw new SteamGridDbNotFoundException(Resources.Resources.Exception_NotFound)
                        { ExceptionType = ExceptionType.NotFound, SteamGridDbErrorMessages = errors };
                case HttpStatusCode.Unauthorized:
                    throw new SteamGridDbUnauthorizedException(Resources.Resources.Exception_Unauthorized)
                        { ExceptionType = ExceptionType.Unauthorized, SteamGridDbErrorMessages = errors };
            }

            throw new SteamGridDbException(Resources.Resources.Exception_Unknown);
        }

        private async Task<SteamGridDbResponse> Delete(string uri)
        {
            var response = await _httpClient.DeleteAsync(uri);
            var str = await response.Content.ReadAsStringAsync();
            var respObj = JsonConvert.DeserializeObject<SteamGridDbResponse>(str);
            var errors = new string[] { };

            if (respObj != null && respObj.Success)
                return respObj;

            if (respObj != null)
                errors = respObj.Errors;

            if (!(response.Headers.RetryAfter is null) && response.Headers.RetryAfter.Delta.HasValue)
            {
                throw new SteamGridDbRateLimitedException(
                    string.Format(Resources.Resources.Exception_RateLimited,
                        response.Headers.RetryAfter.Delta.ToString()), response.Headers.RetryAfter.Delta.Value) {ExceptionType = ExceptionType.RateLimited, SteamGridDbErrorMessages = errors };
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new SteamGridDbBadRequestException(Resources.Resources.Exception_BadRequest)
                        { ExceptionType = ExceptionType.BadRequest, SteamGridDbErrorMessages = errors };
                case HttpStatusCode.Forbidden:
                    throw new SteamGridDbForbiddenException(Resources.Resources.Exception_Forbidden)
                        { ExceptionType = ExceptionType.Forbidden, SteamGridDbErrorMessages = errors };
                case HttpStatusCode.NotFound:
                    throw new SteamGridDbNotFoundException(Resources.Resources.Exception_NotFound)
                        { ExceptionType = ExceptionType.NotFound, SteamGridDbErrorMessages = errors };
                case HttpStatusCode.Unauthorized:
                    throw new SteamGridDbUnauthorizedException(Resources.Resources.Exception_Unauthorized)
                        { ExceptionType = ExceptionType.Unauthorized, SteamGridDbErrorMessages = errors };
            }

            throw new SteamGridDbException(Resources.Resources.Exception_Unknown);
        }

        #endregion

        /// <summary>
        /// Cancels all pending requests, closes HTTP client and releases all resources used by object
        /// </summary>
        public void Dispose()
        {
            CancelPendingRequests();
            _httpClient.Dispose();
        }
    }
}