namespace Petecat.Threading.Tasks
{
    public abstract class AbstractTaskObject : ITaskObject
    {
        public AbstractTaskObject(string name, string description)
        {
            Name = name;
            Description = description;
            Status = TaskObjectStatus.Sleep;
        }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public abstract void Execute();

        public virtual void Suspend() { }

        public virtual void Terminate() { }

        public TaskObjectStatus Status { get; private set; }

        private object _StatusLocker = new object();

        public void ChangeStatusTo(TaskObjectStatus status)
        {
            if (Status != status)
            {
                lock (_StatusLocker)
                {
                    if (Status != status)
                    {
                        if (StatusChangedFrom != null)
                        {
                            StatusChangedFrom.Invoke(this, Status);
                        }

                        Status = status;

                        if (StatusChangedTo != null)
                        {
                            StatusChangedTo.Invoke(this, Status);
                        }
                    }
                }
            }
        }

        public string Key { get { return Name; } }

        public event TaskObjectStatusChangedFromHandlerDelegate StatusChangedFrom = null;

        public event TaskObjectStatusChangedToHandlerDelegate StatusChangedTo = null;

        public bool CanExecute { get { return Status == TaskObjectStatus.Sleep; } }

        public bool CanResume { get { return Status == TaskObjectStatus.Suspended; } }

        public bool CanSuspend { get { return Status == TaskObjectStatus.Executing; } }

        public bool CanTerminate { get { return Status == TaskObjectStatus.Executing; } }

        public virtual void Dispose() { }
    }
}
