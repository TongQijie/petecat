namespace Petecat.Threading.Tasks
{
    public abstract class AbstractBackgroundTask : IBackgroundTask
    {
        public AbstractBackgroundTask(string name)
        {
            Key = name;
            Status = BackgroundTaskStatus.Sleep;
        }

        public string Key { get; private set; }

        public BackgroundTaskStatus Status { get; private set; }

        public event BackgroundTaskStatusChangingFrom BackgroundTaskStatusChangingFrom;

        public event BackgroundTaskStatusChangedTo BackgroundTaskStatusChangedTo;

        public abstract void Execute();

        public virtual void Suspend()
        {
            // do nothing
        }

        public virtual void Resume()
        {
            // do nothing
        }

        private object _StatusLocker = new object();

        public void StatusChangeTo(BackgroundTaskStatus status)
        {
            if (Status == status)
            {
                return;
            }

            lock (_StatusLocker)
            {
                if (Status != status)
                {
                    if (BackgroundTaskStatusChangingFrom != null)
                    {
                        BackgroundTaskStatusChangingFrom.Invoke(this, Status);
                    }

                    Status = status;

                    if (BackgroundTaskStatusChangedTo != null)
                    {
                        BackgroundTaskStatusChangedTo.Invoke(this, Status);
                    }
                }
            }
        }

        public bool CanContinue
        {
            get
            {
                return Status == BackgroundTaskStatus.Executing;
            }
        }

        public bool CanResume
        {
            get
            {
                return Status == BackgroundTaskStatus.Suspended;
            }
        }

        public virtual void Dispose()
        {
            // do nothing
        }
    }
}