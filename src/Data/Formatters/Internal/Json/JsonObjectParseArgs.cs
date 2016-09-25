namespace Petecat.Data.Formatters.Internal.Json
{
    internal class JsonObjectParseArgs
    {
        public IBufferStream Stream { get; set; }

        public JsonObject ExternalObject { get; set; }

        public JsonObject InternalObject { get; set; }

        public bool Handled { get; set; }
    }
}
