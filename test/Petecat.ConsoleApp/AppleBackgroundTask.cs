using System;

using Petecat.Threading.Tasks;

namespace Petecat.ConsoleApp
{
    public class AppleBackgroundTask : AlwaysRunBackgroundTask
    {
        public AppleBackgroundTask()
            : base("Apple", (task) =>
            {
                System.Console.WriteLine("{0}: started...", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                while (true)
                {
                    if (task.Status == BackgroundTaskStatus.Executing)
                    {
                        System.Console.WriteLine("{0}: doing something...", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        System.Threading.Thread.Sleep(3000);
                    }
                    else if (task.Status == BackgroundTaskStatus.Suspending)
                    {
                        task.StatusChangeTo(BackgroundTaskStatus.Suspended);
                    }
                    else if (task.Status == BackgroundTaskStatus.Suspended)
                    {
                        System.Threading.Thread.Sleep(5000);
                    }
                    else if (task.Status == BackgroundTaskStatus.Terminating)
                    {
                        task.StatusChangeTo(BackgroundTaskStatus.Sleep);
                        break;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(5000);
                    }
                }

                return false;
            })
        {
        }

        public AppleBackgroundTask(string taskName)
            : this()
        {
            System.Console.WriteLine("{0} loaded.", taskName);
        }
    }
}
