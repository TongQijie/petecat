namespace Petecat.Console.Command
{
    public delegate void TerminalCommandExecutingDelegate(ITerminalCommandChannel terminalCommandChannel, ITerminalCommand terminalCommand, string prompt);

    public delegate void TerminalCommandDidExecutedDelegate(ITerminalCommandChannel terminalCommandChannel, ITerminalCommand terminalCommand);

    public interface ITerminalCommandChannel
    {
        void Consume(ITerminalCommand terminalCommand);

        /// <summary>
        /// 终端命令执行时事件
        /// </summary>
        event TerminalCommandExecutingDelegate TerminalCommandExecuting;

        /// <summary>
        /// 终端命令执行完成事件
        /// </summary>
        event TerminalCommandDidExecutedDelegate TerminalCommandDidExecuted;

        ITerminalCommandChannel[] InnerTerminalCommandChannels { get; }
    }
}