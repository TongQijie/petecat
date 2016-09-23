using System;

namespace Petecat.Data.Attributes
{
    public class JsonPropertyAttribute : Attribute
    {
        public string Alias { get; set; }
    }
}
