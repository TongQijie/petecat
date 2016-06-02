using System;

namespace Petecat.Console.Command
{
    public class TerminalCommandAttribute : Attribute
    {
        public TerminalCommandAttribute(string[] supportedCommandCodes)
        {
            SupportedCommandCodes = supportedCommandCodes;
        }

        public string[] SupportedCommandCodes { get; set; }
    }
}