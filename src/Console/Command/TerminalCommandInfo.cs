using System;

using Petecat.Collection;

namespace Petecat.Console.Command
{
    internal class TerminalCommandInfo : IKeyedObject<string>
    {
        public TerminalCommandInfo(Type terminalCommandType, string[] supportedCommandCodes)
        {
            TerminalCommandType = terminalCommandType;
            SupportedCommandCodes = supportedCommandCodes;
        }

        public Type TerminalCommandType { get; set; }

        public string[] SupportedCommandCodes { get; set; }

        public string Key
        {
            get { return TerminalCommandType.FullName; }
        }
    }
}
