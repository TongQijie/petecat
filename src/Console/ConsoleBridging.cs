namespace Petecat.Console
{
    using System;

    public static class ConsoleBridging
    {
        public static void ReadAnyKey()
        {
            Console.ReadKey();
        }

        public static void WriteNewLine()
        {
            Console.Write(Environment.NewLine);
        }

        public static void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        public static void WriteLine(string format, params object[] values)
        {
            Console.WriteLine(format, values);
        }

        public static void Write(string text)
        {
            Console.Write(text);
        }

        public static void Write(string format, params object[] values)
        {
            Console.Write(format, values);
        }

        public static Command.TerminalCommandLine ReadCommand()
        {
            return Command.TerminalCommandLineUtility.Parse(Console.ReadLine());
        }

        public static string ReadLine()
        {
            return Console.ReadLine();
        }

        public static T ReadLine<T>(T defaultValue)
        {
            try
            {
                return (T)Convert.ChangeType(Console.ReadLine(), typeof(T));
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static bool TryReadLine<T>(out T value)
        {
            try
            {
                value = (T)Convert.ChangeType(Console.ReadLine(), typeof(T));
                return true;
            }
            catch (Exception)
            {
                value = default(T);
                return false;
            }
        }
    }
}
