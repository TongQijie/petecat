using System.IO;
namespace Petecat.Data.Formatters.Internal.Json
{
    public class JsonObjectParseArgs
    {
        public Stream Stream { get; set; }

        public JsonObject ExternalObject { get; set; }

        public JsonObject InternalObject { get; set; }

        public bool Handled { get; set; }
    }
}
