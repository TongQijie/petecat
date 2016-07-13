using System;

namespace Petecat.IOC
{
    public class MethodArgument
    {
        public int Index { get; set; }

        public string Name { get; set; }

        public Type ArgumentType { get; set; }

        public object ArgumentValue { get; set; }
    }
}