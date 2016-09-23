using System;

namespace Petecat.Console.Errors
{
    public class TerminalCommandParseException : Exception
    {
        public TerminalCommandParseException(string commandText)
            : base(string.Format("\"{0}\" failed to parse.", commandText))
        {
        }
    }
}
