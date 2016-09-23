using System;

namespace Petecat.IoC
{
    public class MethodArgument
    {
        public int Index { get; set; }

        public string Name { get; set; }

        public Type ArgumentType { get; set; }

        public object ArgumentValue { get; set; }
    }
}