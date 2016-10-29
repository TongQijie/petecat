using System;

namespace Petecat.Terminal
{
    public static class TerminalManager
    {
        public static T ReadLine<T>()
        {
            return Utility.Converter.Assignable<T>(System.Console.ReadLine().Trim());
        }

        public static T ReadLine<T>(T defaultValue)
        {
            T value;
            if (Utility.Converter.TryBeAssignable<T>(System.Console.ReadLine().Trim(), out value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }
    }
}
