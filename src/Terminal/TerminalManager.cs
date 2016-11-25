using Petecat.Extending;

namespace Petecat.Terminal
{
    public static class TerminalManager
    {
        public static T ReadLine<T>()
        {
            return System.Console.ReadLine().Trim().ConvertTo<T>();
        }

        public static T ReadLine<T>(T defaultValue)
        {
            object value;
            if (System.Console.ReadLine().Trim().Convertible<T>(out value))
            {
                return (T)value;
            }
            else
            {
                return defaultValue;
            }
        }
    }
}
