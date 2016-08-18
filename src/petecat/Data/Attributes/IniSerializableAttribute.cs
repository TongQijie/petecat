using System;

namespace Petecat.Data.Attributes
{
    public class IniSerializableAttribute : Attribute
    {
        public string Name { get; set; }

        public bool NonSerialized { get; set; }
    }
}
