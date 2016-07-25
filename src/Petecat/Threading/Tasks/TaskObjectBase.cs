using System;
using System.Threading;

using Petecat.Logging;

namespace Petecat.Threading.Tasks
{
    public class TaskObjectBase : AbstractTaskObject
    {
        public TaskObjectBase(string name, string description)
            : base(name, description)
        {
        }

        public TaskObjectBase(string name, string description, Func<ITaskObject, bool> implement)
            : base(name, description)
        {
            Implement = implement;
        }

        private Func<ITaskObject, bool> _Implement = null;

        protected Func<ITaskObject, bool> Implement
        {
            get { return _Implement; }
            set
            {
                if (value == null)
                {
                    throw new Exception("implement cannot be null.");
                }

                _Implement = value;
            }
        }

        private Thread _InternalThread = null;

        public override void Execute()
        {
            if (CanExecute)
            {
                CreateInternalThread();
            }
            else if (CanResume)
            {
                TryChangeStatus(TaskObjectStatus.Resuming, TaskObjectStatus.Executing);
            }
        }

        public override void Suspend()
        {
            if (CanSuspend)
            {
                TryChangeStatus(TaskObjectStatus.Suspending, TaskObjectStatus.Suspended);
            }
        }

        public override void Terminate()
        {
            if (CanTerminate)
            {
                TryChangeStatus(TaskObjectStatus.Terminating, TaskObjectStatus.Sleep);
            }
        }

        private void CreateInternalThread()
        {
            if (_InternalThread != null)
            {
                if (_InternalThread.ThreadState == ThreadState.Running)
                {
                    _InternalThread.Abort();
                }
                _InternalThread = null;
            }

            _InternalThread = new Thread(new ThreadStart(() =>
            {
                ChangeStatusTo(TaskObjectStatus.Executing);

                var result = false;
                try
                {
                    result = Implement(this);
                }
                catch (Exception e)
                {
                    LoggerManager.GetLogger().LogEvent("TaskObjectBase", LoggerLevel.Error, e);
                }

                if (!result)
                {
                    LoggerManager.GetLogger().LogEvent("TaskObjectBase", LoggerLevel.Error, string.Format("failed to execute task {0}", Name));
                }

                ChangeStatusTo(TaskObjectStatus.Sleep);

            })) { IsBackground = true };

            _InternalThread.Start();
        }

        private void TryChangeStatus(TaskObjectStatus from, TaskObjectStatus to)
        {
            var oldStatus = Status;
            ChangeStatusTo(from);

            var tryTimes = 1;
            while (Status != to && tryTimes <= 10)
            {
                Thread.Sleep(tryTimes * 100);
                tryTimes++;
            }

            if (Status != to)
            {
                LoggerManager.GetLogger().LogEvent("TaskObjectBase", LoggerLevel.Warn, string.Format("failed to change task status from {0} to {1}", from, to));

                if (Status == from)
                {
                    ChangeStatusTo(oldStatus);
                }
            }
        }

        public override void Dispose()
        {
            if (_InternalThread != null)
            {
                if(_InternalThread.ThreadState == ThreadState.Running)
                {
                    _InternalThread.Abort();
                }
                _InternalThread = null;
            }
        }
    }
}
