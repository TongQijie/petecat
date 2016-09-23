using System;
namespace Petecat.Threading.Errors
{
    public class TaskExecuteFailedException : Exception
    {
        public TaskExecuteFailedException(string taskName)
            : base(string.Format("task '{0}' was aborted unexpectedly.", taskName))
        {
        }
    }
}
