using System;

namespace Petecat.Data.Attributes
{
    public class BinarySerializableAttribute : Attribute
    {
        public int Index { get; private set; }

        public string Name { get; private set; }

        public bool NonSerialized { get; private set; }

        public BinarySerializableAttribute(int index)
        {
            Index = index;
        }

        public BinarySerializableAttribute(string name)
        {
            Name = name;
        }

        public BinarySerializableAttribute(bool nonSerialized)
        {
            NonSerialized = nonSerialized;
        }
    }
}
