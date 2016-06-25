using System;
using System.Reflection;
using System.Threading;

namespace Petecat.Threading.Tasks
{
    public class PeriodicBackgroundTask : AbstractBackgroundTask
    {
        public PeriodicBackgroundTask(string name, TimeSpan interval)
            : base(name)
        {
            Interval = interval;
        }

        public PeriodicBackgroundTask(string name, TimeSpan interval, Func<PeriodicBackgroundTask, bool> task)
            : this(name, interval)
        {
            Task = task;
        }

        public TimeSpan Interval { get; private set; }

        protected Func<PeriodicBackgroundTask, bool> Task { get; set; }

        private Thread _InnerThread = null;

        public override void Execute()
        {
            if (_InnerThread == null)
            {
                _InnerThread = new Thread(new ThreadStart(() =>
                {
                    while (true)
                    {
                        if (Status == BackgroundTaskStatus.Sleep)
                        {
                            StatusChangeTo(BackgroundTaskStatus.Executing);

                            if (Task != null)
                            {
                                try
                                {
                                    Task.Invoke(this);
                                }
                                catch (Exception e)
                                {
                                    Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("task {0} is terminated exceptionally.", Key), e);
                                }
                            }

                            StatusChangeTo(BackgroundTaskStatus.Sleep);
                        }
                        else if (Status == BackgroundTaskStatus.Suspending)
                        {
                            StatusChangeTo(BackgroundTaskStatus.Suspended);
                        }

                        if (Status == BackgroundTaskStatus.Sleep)
                        {
                            Thread.Sleep(Interval);
                        }
                        else if (Status == BackgroundTaskStatus.Terminating)
                        {
                            StatusChangeTo(BackgroundTaskStatus.Sleep);
                            break;
                        }
                        else
                        {
                            Thread.Sleep(1000);
                        }
                    }

                })) { IsBackground = true };

                _InnerThread.Start();
            }
            else
            {
                Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("Task {0} is running, cannot execute.", Key));
            }
        }

        public override void Suspend()
        {
            if (Status != BackgroundTaskStatus.Executing && Status == BackgroundTaskStatus.Sleep)
            {
                Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("Task {0} status is not executing, cannot suspend.", Key));
                return;
            }

            StatusChangeTo(BackgroundTaskStatus.Suspending);

            int sleepTimes = 10;

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

                if (Status == BackgroundTaskStatus.Executing)
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
