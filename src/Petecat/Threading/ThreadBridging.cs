using System;
using System.Threading;

namespace Petecat.Threading
{
    public static class ThreadBridging
    {
        public static void Sleep(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        public static void Retry(int maxTimes, Func<bool> action)
        {
            var tryTimes = 1;
            while (!action() && tryTimes <= maxTimes)
            {
                Sleep(100 * tryTimes);
                tryTimes++;
            }
        }
    }
}
