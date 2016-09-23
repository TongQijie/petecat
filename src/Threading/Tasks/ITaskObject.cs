using Petecat.Collection;

using System;

namespace Petecat.Threading.Tasks
{
    public interface ITaskObject : IKeyedObject<string>, IDisposable
    {
        string Name { get; }

        string Description { get; }

        void Execute();

        void Suspend();

        void Terminate();

        TaskObjectStatus Status { get; }

        event TaskObjectStatusChangedFromHandlerDelegate StatusChangedFrom;

        event TaskObjectStatusChangedToHandlerDelegate StatusChangedTo;

        bool CanExecute { get; }

        bool CanResume { get; }

        bool CanSuspend { get; }

        bool CanTerminate { get; }
    }
}
