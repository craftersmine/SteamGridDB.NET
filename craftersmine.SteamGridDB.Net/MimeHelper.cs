using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.SteamGridDBNet
{
    internal static class MimeHelper
    {
        private static readonly Dictionary<string, byte[]> _mimeTypes = new Dictionary<string, byte[]>
        {
            {"image/jpeg", new byte[] {255, 216, 255}},
            {"image/png", new byte[] {137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82}}
        };

        public static bool ValidateMimeType(byte[] file, string contentType)
        {
            var imageType = _mimeTypes.SingleOrDefault(x => x.Key.Equals(contentType));

            return file.Take(imageType.Value.Length).SequenceEqual(imageType.Value);
        }
    }
}
