using Petecat.Threading.Tasks;
using System;
namespace Petecat.Threading.Errors
{
    public class TaskStatusChangeFailedException : Exception
    {
        public TaskStatusChangeFailedException(string taskName, TaskObjectStatus statusFrom, TaskObjectStatus statusTo)
            : base(string.Format("task '{0}' fails to change status '{1}' to '{2}'", taskName, statusFrom, statusTo))
        {
        }
    }
}
