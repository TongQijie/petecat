using System;
using System.Reflection;
using System.Linq;

using Petecat.Utility;
using Petecat.Collection;

namespace Petecat.Console.Command
{
    public class TerminalCommandUtility
    {
        private static ThreadSafeKeyedObjectCollection<string, TerminalCommandInfo> _TerminalCommandInfos = null;

        private static ThreadSafeKeyedObjectCollection<string, TerminalCommandInfo> TerminalCommandInfos
        {
            get { return _TerminalCommandInfos ?? (_TerminalCommandInfos = new ThreadSafeKeyedObjectCollection<string, TerminalCommandInfo>()); } 
        }

        public static ITerminalCommand Create(Assembly assembly, string terminalCommandText)
        {
            var terminalCommandLine = TerminalCommandLineUtility.Parse(terminalCommandText);
            if (terminalCommandLine == null)
            {
                throw new Errors.TerminalCommandParseException(terminalCommandText);
            }

            Type terminalCommandType = null;

            foreach (var terminalCommandInfo in TerminalCommandInfos.Values)
            {
                if (terminalCommandInfo.SupportedCommandCodes.Contains(terminalCommandLine.CommandCode))
                {
                    terminalCommandType = terminalCommandInfo.TerminalCommandType;
                    break;
                }
            }

            if (terminalCommandType == null)
            {
                terminalCommandType = assembly.GetTypes().FirstOrDefault(x =>
                {
                    TerminalCommandAttribute terminalCommandAttribute;
                    if (ReflectionUtility.TryGetCustomAttribute(x, y => y.SupportedCommandCodes.Contains(terminalCommandLine.CommandCode), out terminalCommandAttribute))
                    {
                        TerminalCommandInfos.Add(new TerminalCommandInfo(x, terminalCommandAttribute.SupportedCommandCodes));
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
            }

            if (terminalCommandType == null)
            {
                throw new Errors.TerminalCommandNotFoundException(terminalCommandLine.CommandCode);
            }

            return Activator.CreateInstance(terminalCommandType, terminalCommandLine) as ITerminalCommand;
        }
    }
}
