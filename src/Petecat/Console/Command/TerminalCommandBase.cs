namespace Petecat.Console.Command
{
    public abstract class TerminalCommandBase : ITerminalCommand
    {
        public TerminalCommandBase(TerminalCommandLine terminalCommandLine)
        {
            TerminalCommandLine = terminalCommandLine;
        }

        public TerminalCommandLine TerminalCommandLine { get; private set; }

        public bool Wait { get; set; }

        public object Result { get; protected set; }

        public bool Handled { get; protected set; }

        public abstract void Execute(ITerminalCommandChannel terminalCommandChannel, System.Action<string> prompt);
    }
}
