using System;
using System.Reflection;
using System.Threading;

namespace Petecat.Threading.Tasks
{
    public class AlwaysRunBackgroundTask : AbstractBackgroundTask
    {
        public AlwaysRunBackgroundTask(string name) 
            : base(name)
        {
        }

        public AlwaysRunBackgroundTask(string name, Func<AlwaysRunBackgroundTask, bool> task)
            : this(name)
        {
            Task = task;
        }

        protected Func<AlwaysRunBackgroundTask, bool> Task { get; set; }

        private Thread _InnerThread = null;

        public override void Execute()
        {
            if (Status == BackgroundTaskStatus.Sleep)
            {
                _InnerThread = new Thread(new ThreadStart(() =>
                {
                    StatusChangeTo(BackgroundTaskStatus.Executing);

                    if (Task != null)
                    {
                        var success = false;
                        try
                        {
                            success = Task.Invoke(this);
                        }
                        catch (Exception e)
                        {
                            Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("task {0} is terminated exceptionally.", Key), e);
                        }

                        if (!success)
                        {
                            if (Status != BackgroundTaskStatus.Executing)
                            {
                                return;
                            }
                        }
                    }

                    StatusChangeTo(BackgroundTaskStatus.Sleep);

                })) { IsBackground = true };

                _InnerThread.Start();
            }
            else
            {
                Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("Task {0} status is not sleep, cannot execute.", Key));
            }
        }

        public override void Suspend()
        {
            if (Status != BackgroundTaskStatus.Executing)
            {
                Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("Task {0} status is not executing, cannot suspend.", Key));
                return;
            }

            StatusChangeTo(BackgroundTaskStatus.Suspending);

            var sleepTimes = 10;

            while (Status != BackgroundTaskStatus.Suspended && sleepTimes > 0)
            {
                sleepTimes--;
                Thread.Sleep(500);
            }

            if (Status == BackgroundTaskStatus.Suspending)
            {
                StatusChangeTo(BackgroundTaskStatus.Executing);
                Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("Task {0} suspend timeout.", Key));
            }
        }

        public override void Resume()
        {
            if (Status == BackgroundTaskStatus.Suspended)
            {
                StatusChangeTo(BackgroundTaskStatus.Executing);
            }
            else
            {
                Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("Task {0} status is not suspended, cannot resume.", Key));
            }
        }

        public override void Dispose()
        {
            StatusChangeTo(BackgroundTaskStatus.Terminating);

            if (_InnerThread != null)
            {
                var sleepTimes = 10;

                while (Status != BackgroundTaskStatus.Sleep && sleepTimes > 0)
                {
                    sleepTimes--;
                    Thread.Sleep(500);
                }

                if (Status != BackgroundTaskStatus.Sleep)
                {
                    _InnerThread.Abort();
                    StatusChangeTo(BackgroundTaskStatus.Sleep);
                    Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("Task {0} was force absorted.", Key));
                }

                _InnerThread = null;
            }
        }
    }
}
