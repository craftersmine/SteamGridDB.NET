using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using craftersmine.SteamGridDBNet.Exceptions;
using Newtonsoft.Json;

namespace craftersmine.SteamGridDBNet
{
    /// <summary>
    /// Represents a base object for all SteamGridDB objects, <see cref="SteamGridDbGrid"/>, <see cref="SteamGridDbHero"/>, <see cref="SteamGridDbLogo"/> and <see cref="SteamGridDbIcon"/>
    /// </summary>
    public class SteamGridDbObject
    {
        internal SteamGridDb ApiInstance { get; set; }

        /// <summary>
        /// Gets SteamGridDB item ID
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; private set; }
        /// <summary>
        /// Gets item Score. No official documentation
        /// </summary>
        [JsonProperty("score")]
        public int Score { get; private set; }
        /// <summary>
        /// Gets an item style
        /// </summary>
        [JsonProperty("style")]
        public SteamGridDbStyles Style { get; private set; }
        /// <summary>
        /// Gets an item image width
        /// </summary>
        [JsonProperty("width")]
        public int Width { get; private set; }
        /// <summary>
        /// Gets an item image height
        /// </summary>
        [JsonProperty("height")]
        public int Height { get; private set; }
        /// <summary>
        /// Gets <see langword="true"/> if contains Non-Suitable-For-Work content, otherwise <see langword="false"/>
        /// </summary>
        [JsonProperty("nsfw")]
        public bool IsNsfw { get; private set; }
        /// <summary>
        /// Gets <see langword="true"/> if contains humor content, otherwise <see langword="false"/>
        /// </summary>
        [JsonProperty("humor")]
        public bool IsHumorous { get; private set; }
        /// <summary>
        /// Gets user specified notes for object
        /// </summary>
        [JsonProperty("notes")]
        public string Notes { get; private set; }
        /// <summary>
        /// Gets item image format
        /// </summary>
        [JsonProperty("mime")]
        public SteamGridDbFormats Format { get; private set; }
        /// <summary>
        /// Gets item language
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; private set; }
        /// <summary>
        /// Gets item full image URL
        /// </summary>
        [JsonProperty("url")]
        public string FullImageUrl { get; private set; }
        /// <summary>
        /// Gets item thumbnail image URL
        /// </summary>
        [JsonProperty("thumb")]
        public string ThumbnailImageUrl { get; private set; }
        /// <summary>
        /// Gets <see langword="true"/> if item is locked by user, otherwise <see langword="false"/>. No official documentation
        /// </summary>
        [JsonProperty("lock")]
        public bool IsLocked { get; private set; }
        /// <summary>
        /// Gets <see langword="true"/> if contains content that can cause seizures or epilepsy, otherwise <see langword="false"/>
        /// </summary>
        [JsonProperty("epilepsy")]
        public bool CanCauseEpilepsy { get; private set; }
        /// <summary>
        /// Gets a number of upvotes for item. No official documentation
        /// </summary>
        [JsonProperty("upvotes")]
        public int Upvotes { get; private set; }
        /// <summary>
        /// Gets a number of downvotes for item. No official documentation
        /// </summary>
        [JsonProperty("downvotes")]
        public int Downvotes { get; private set; }
        /// <summary>
        /// Gets a <see cref="SteamAuthor"/> object that created that item
        /// </summary>
        [JsonProperty("author")]
        public SteamAuthor Author { get; private set; }

        /// <summary>
        /// Gets an image data as stream from server
        /// </summary>
        /// <param name="thumbnail">Download full image or thumbnail. If <see langword="true"/>, thumbnail image will be returned as stream, otherwise full image</param>
        /// <returns></returns>
        /// <exception cref="SteamGridDbException">When unknown exception occurred</exception>
        /// <exception cref="SteamGridDbImageException">When error occurred while downloading image</exception>
        /// <exception cref="ArgumentNullException">When image URL is null</exception>
        public async Task<Stream> GetImageAsStreamAsync(bool thumbnail)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage resp;
                    if (thumbnail)
                        resp = await client.GetAsync(ThumbnailImageUrl);
                    else resp = await client.GetAsync(FullImageUrl);

                    if (resp.IsSuccessStatusCode)
                    {
                        return await resp.Content.ReadAsStreamAsync();
                    }

                    var respJson = await resp.Content.ReadAsStringAsync();
                    var respObj = JsonConvert.DeserializeObject<SteamGridDbImageErrorResponse>(respJson);

                    if (respObj is null)
                        throw new SteamGridDbImageException(ExceptionType.Unknown,
                            Resources.Resources.Exception_UnknownImageException);

                    switch (resp.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            throw new SteamGridDbImageException(ExceptionType.NotFound, respObj.Message);
                        case HttpStatusCode.BadRequest:
                            throw new SteamGridDbImageException(ExceptionType.BadRequest, respObj.Message);
                        case HttpStatusCode.Forbidden:
                            throw new SteamGridDbImageException(ExceptionType.Forbidden, respObj.Message);
                        case HttpStatusCode.Unauthorized:
                            throw new SteamGridDbImageException(ExceptionType.Unauthorized, respObj.Message);
                    }
                    throw new SteamGridDbImageException(ExceptionType.Unknown,
                        Resources.Resources.Exception_UnknownImageException);
                }
                catch (Exception e)
                {
                    if (e is HttpRequestException)
                    {
                        throw new SteamGridDbException(Resources.Resources.Exception_UnknownImageException);
                    }

                    throw;
                }
            }
        }

        /// <summary>
        /// Downloads full image to specified file
        /// </summary>
        /// <param name="filePath">Full path of file to download</param>
        /// <exception cref="UnauthorizedAccessException">When access to file is forbidden</exception>
        /// <exception cref="ArgumentException">When path is empty, null, has only whitespaces or has invalid characters</exception>
        /// <exception cref="ArgumentNullException">When path is empty, null or has only whitespaces</exception>
        /// <exception cref="PathTooLongException">When specified path is too long</exception>
        /// <exception cref="DirectoryNotFoundException">When file directory or part of path not found or invalid</exception>
        /// <exception cref="NotSupportedException">When path format has invalid format</exception>
        public async void DownloadToFileAsync(string filePath)
        {
            var stream = await GetImageAsStreamAsync(false);
            using (FileStream fs = File.OpenWrite(filePath))
            {
                await stream.CopyToAsync(fs);
            }
        }

        /// <summary>
        /// Downloads thumbnail image to specified file
        /// </summary>
        /// <param name="filePath">Full path of file to download</param>
        /// <exception cref="UnauthorizedAccessException">When access to file is forbidden</exception>
        /// <exception cref="ArgumentException">When path is empty, null, has only whitespaces or has invalid characters</exception>
        /// <exception cref="ArgumentNullException">When path is empty, null or has only whitespaces</exception>
        /// <exception cref="PathTooLongException">When specified path is too long</exception>
        /// <exception cref="DirectoryNotFoundException">When file directory or part of path not found or invalid</exception>
        /// <exception cref="NotSupportedException">When path format has invalid format</exception>
        public async void DownloadThumbnailToFileAsync(string filePath)
        {
            var stream = await GetImageAsStreamAsync(true);
            using (FileStream fs = File.OpenWrite(filePath))
            {
                await stream.CopyToAsync(fs);
            }
        }

        /// <summary>
        /// Deletes item from server
        /// </summary>
        /// <returns><see langword="true"/> if item is deleted from server, otherwise <see langword="false"/></returns>
        /// <exception cref="SteamGridDbNotFoundException">When item is not found on server</exception>
        /// <exception cref="SteamGridDbUnauthorizedException">When your API key is invalid, not set, or you've reset it on API preferences page and use old one</exception>
        /// <exception cref="SteamGridDbBadRequestException">When library makes invalid request to server due to invalid URI generated</exception>
        /// <exception cref="SteamGridDbForbiddenException">When you don't have permissions to perform action on item, probably because you don't own item</exception>
        /// <exception cref="SteamGridDbException">When unknown exception occurred in request</exception>
        public async Task<bool> DeleteFromServerAsync()
        {
            if (this is SteamGridDbGrid)
                return await ApiInstance.DeleteGridAsync(Id);
            if (this is SteamGridDbHero)
                return await ApiInstance.DeleteHeroAsync(Id);
            if (this is SteamGridDbLogo)
                return await ApiInstance.DeleteLogoAsync(Id);
            if (this is SteamGridDbIcon)
                return await ApiInstance.DeleteIconAsync(Id);

            return false;
        }
    }
}
