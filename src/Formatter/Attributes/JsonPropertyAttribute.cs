using System;

namespace Petecat.Formatter.Attributes
{
    public class JsonPropertyAttribute : Attribute
    {
        public string Alias { get; set; }
    }
}
