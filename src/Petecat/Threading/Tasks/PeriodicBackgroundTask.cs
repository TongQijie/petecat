using System;

namespace Petecat.Threading.Tasks
{
    public class PeriodicBackgroundTask : AbstractBackgroundTask
    {
        public PeriodicBackgroundTask(string name, Func<PeriodicBackgroundTask, bool> task)
            : base(name)
        {
        }

        public override void Execute()
        {
            
        }
    }
}
