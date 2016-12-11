using System;

using Petecat.Threading;

namespace Petecat.Jobs
{
    public abstract class JobBase : IJob, IDisposable
    {
        public JobBase(string name, string description)
        {
            Name = name;
            Description = description;
            Status = JobStatus.Stopped;
        }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public JobStatus Status { get; private set; }

        public void Execute()
        {
            if (Status == JobStatus.Stopped)
            {
                StartInternalThread();
            }
            else if (Status == JobStatus.Suspended)
            {
                TryChangeStatus(JobStatus.Resuming, JobStatus.Executing);
            }
        }

        public void Suspend()
        {
            if (Status == JobStatus.Executing)
            {
                TryChangeStatus(JobStatus.Suspending, JobStatus.Suspended);
            }
        }

        public void Terminate()
        {
            if (Status == JobStatus.Executing)
            {
                TryChangeStatus(JobStatus.Terminating, JobStatus.Stopped);
            }
        }

        private object _StatusLocker = new object();

        protected void ChangeStatus(JobStatus toStatus)
        {
            if (Status != toStatus)
            {
                lock (_StatusLocker)
                {
                    if (Status != toStatus)
                    {
                        Status = toStatus;
                    }
                }
            }
        }

        protected abstract void Implement();

        public void Dispose()
        {
            if (_InternalThread != null)
            {
                _InternalThread.Dispose();
            }

            _InternalThread = null;
        }

        #region Private Methods

        private ThreadObject _InternalThread = null;
        
        private void StartInternalThread()
        {
            Dispose();

            _InternalThread = new ThreadObject(() =>
            {
                ChangeStatus(JobStatus.Executing);

                try
                {
                    Implement();
                }
                catch (Exception)
                {
                    // log here
                }

                ChangeStatus(JobStatus.Stopped);
            }).Start();
        }

        private void TryChangeStatus(JobStatus fromStatus, JobStatus toStatus)
        {
            var previousStatus = Status;
            ChangeStatus(fromStatus);

            ThreadBridging.Retry(10, () => Status == toStatus);

            if (Status != toStatus)
            {
                // TODO: log here

                if (Status == fromStatus)
                {
                    ChangeStatus(previousStatus);
                }
            }
        }

        #endregion

        protected JobStatus CheckTransitionalStatus()
        {
            if (Status == JobStatus.Suspending)
            {
                OnSuspending();
                ChangeStatus(JobStatus.Suspended);

                while (Status != JobStatus.Resuming)
                {
                    ThreadBridging.Sleep(1000);
                }
            }

            if (Status == JobStatus.Resuming)
            {
                OnResuming();
                ChangeStatus(JobStatus.Executing);
            }

            if (Status == JobStatus.Terminating)
            {
                OnTerminating();
                ChangeStatus(JobStatus.Stopped);
            }

            return Status;
        }

        public virtual void OnSuspending() { }

        public virtual void OnResuming() { }

        public virtual void OnTerminating() { }
    }
}