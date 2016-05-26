using System;
namespace Petecat.Threading.Tasks
{
    public delegate void BackgroundTaskStatusChangingFrom(IBackgroundTask backgroundTask, BackgroundTaskStatus status);

    public delegate void BackgroundTaskStatusChangedTo(IBackgroundTask backgroundTask, BackgroundTaskStatus status);

    public interface IBackgroundTask : Collection.IKeyedObject<string>, IDisposable
    {
        BackgroundTaskStatus Status { get; }

        event BackgroundTaskStatusChangingFrom BackgroundTaskStatusChangingFrom;

        event BackgroundTaskStatusChangedTo BackgroundTaskStatusChangedTo;
    }
}