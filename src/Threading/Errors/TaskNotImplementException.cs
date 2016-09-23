using System;

namespace Petecat.Threading.Errors
{
    public class TaskNotImplementException : Exception
    {
        public TaskNotImplementException(string taskName)
            : base(string.Format("task '{0}' was not implemented.", taskName))
        {
        }
    }
}
