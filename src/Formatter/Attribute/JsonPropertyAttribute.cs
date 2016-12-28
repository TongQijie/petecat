namespace Petecat.Formatter.Attribute
{
    using System;

    public class JsonPropertyAttribute : Attribute
    {
        public JsonPropertyAttribute() { }

        public JsonPropertyAttribute(string alias)
        {
            Alias = alias;
        }

        public string Alias { get; set; }
    }
}
