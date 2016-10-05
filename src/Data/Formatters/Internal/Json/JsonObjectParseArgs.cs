using Petecat.IO;

namespace Petecat.Data.Formatters.Internal.Json
{
    internal class JsonObjectParseArgs
    {
        public IStream Stream { get; set; }

        public JsonObject ExternalObject { get; set; }

        public JsonObject InternalObject { get; set; }

        public bool Handled { get; set; }
    }
}
