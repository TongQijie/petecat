using System;

namespace Petecat.Data.Attributes
{
    public class BinaryPropertyAttribute : Attribute
    {
        public string Alias { get; private set; }

        public int Index { get; private set; }
    }
}
