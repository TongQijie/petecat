using System;
using System.Threading;

namespace Petecat.Threading
{
    public class ThreadObject : IDisposable
    {
        private Thread _InternalThread = null;

        public ThreadObject(Action action)
        {
            _InternalThread = new Thread(new ThreadStart(action)) { IsBackground = true };
        }

        public ThreadObject Start()
        {
            if (_InternalThread != null && !_InternalThread.IsAlive)
            {
                _InternalThread.Start();
            }

            return this;
        }

        public void Dispose()
        {
            if (_InternalThread.IsAlive)
            {
                _InternalThread.Abort();
            }

            _InternalThread = null;
        }
    }
}
