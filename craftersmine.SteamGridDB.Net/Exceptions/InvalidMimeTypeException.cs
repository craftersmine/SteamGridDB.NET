using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.SteamGridDBNet.Exceptions
{
    [Serializable]
    public class InvalidMimeTypeException : Exception
    {
        public InvalidMimeTypeException()
        {
        }

        public InvalidMimeTypeException(string message) : base(message)
        {
        }

        public InvalidMimeTypeException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InvalidMimeTypeException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
