using System;

namespace Petecat.Console.Errors
{
    public class TerminalCommandNotFoundException : Exception
    {
        public TerminalCommandNotFoundException(string commandCode)
            : base(string.Format("command[{0}] not found.", commandCode))
        {
        }

        public TerminalCommandNotFoundException(string commandCode, Exception innerException)
            : base(string.Format("command({0}) not found.", commandCode), innerException)
        {
        }
    }
}