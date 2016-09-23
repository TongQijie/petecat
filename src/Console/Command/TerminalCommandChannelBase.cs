﻿namespace Petecat.Console.Command
{
    public class TerminalCommandChannelBase : ITerminalCommandChannel
    {
        public virtual void Consume(ITerminalCommand terminalCommand)
        {
            terminalCommand.Execute(this, (text) =>
            {
                FireTerminalCommandExecuting(terminalCommand, text);
            });

            if (terminalCommand.Handled)
            {
                FireTerminalCommandDidExecuted(terminalCommand);
                return;
            }

            foreach (var innerTerminalCommandChannel in InnerTerminalCommandChannels)
            {
                innerTerminalCommandChannel.Consume(terminalCommand);

                if (terminalCommand.Handled)
                {
                    return;
                }
            }
        }

        public event TerminalCommandExecutingDelegate TerminalCommandExecuting;

        public event TerminalCommandDidExecutedDelegate TerminalCommandDidExecuted;

        public virtual ITerminalCommandChannel[] InnerTerminalCommandChannels
        {
            get { return new ITerminalCommandChannel[0]; }
        }

        protected void FireTerminalCommandExecuting(ITerminalCommand terminalCommand, string prompt)
        {
            if (TerminalCommandExecuting != null)
            {
                TerminalCommandExecuting.Invoke(this, terminalCommand, prompt);
            }
        }

        protected void FireTerminalCommandDidExecuted(ITerminalCommand terminalCommand)
        {
            if (TerminalCommandDidExecuted != null)
            {
                TerminalCommandDidExecuted.Invoke(this, terminalCommand);
            }
        }
    }
}
