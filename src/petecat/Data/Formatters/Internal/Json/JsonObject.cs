using System.IO;
using Petecat.Extension;

namespace Petecat.Data.Formatters.Internal.Json
{
    public class JsonObject
    {
        public virtual bool Fill(Stream stream, byte[] seperators, byte[] terminators)
        {
            return false;
        }
    }
}
