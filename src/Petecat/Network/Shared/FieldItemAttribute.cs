using System;

namespace Petecat.Network.Shared
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FieldItemAttribute : Attribute
    {
        private int _Index = -1;

        public int Index
        {
            get { return _Index; }
            private set { _Index = value; }
        }

        public FieldItemAttribute(int index)
        {
            Index = index;
        }
    }
}
