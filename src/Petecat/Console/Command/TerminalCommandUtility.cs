using System.Reflection;
using System.Linq;
using System;

namespace Petecat.Console.Command
{
    public class TerminalCommandUtility
    {
        public static ITerminalCommand Create(string terminalCommandText)
        {
            var terminalCommandLine = TerminalCommandLineUtility.Parse(terminalCommandText);
            if (terminalCommandLine == null)
            {
                return null;
            }

            var terminalCommandType = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(
                x => x.GetCustomAttribute<TerminalCommandAttribute>(false) != null 
                    && x.GetCustomAttribute<TerminalCommandAttribute>(false).SupportedCommandCodes.Contains(terminalCommandLine.CommandCode));
            if (terminalCommandType == null)
            {
                return null;
            }

            return Activator.CreateInstance(terminalCommandType, terminalCommandLine) as ITerminalCommand;
        }

        public static ITerminalCommand Create(Assembly assembly, string terminalCommandText)
        {
            var terminalCommandLine = TerminalCommandLineUtility.Parse(terminalCommandText);
            if (terminalCommandLine == null)
            {
                return null;
            }

            var terminalCommandType = assembly.GetTypes().FirstOrDefault(
                x => x.GetCustomAttribute<TerminalCommandAttribute>(false) != null
                    && x.GetCustomAttribute<TerminalCommandAttribute>(false).SupportedCommandCodes.Contains(terminalCommandLine.CommandCode));
            if (terminalCommandType == null)
            {
                return null;
            }

            return Activator.CreateInstance(terminalCommandType, terminalCommandLine) as ITerminalCommand;
        }
    }
}
