using Petecat.Jobs;
using Petecat.Console;
using Petecat.Threading;

namespace Petecat.ConsoleApp.Jobs
{
    public class AppleClass : JobBase
    {
        public AppleClass() : base("appleClass", "")
        {
        }

        protected override void Implement()
        {
            while (true)
            {
                ConsoleBridging.WriteLine(string.Format("{0}: {1}", Name, Status));

                ThreadBridging.Sleep(2000);

                var status = CheckTransitionalStatus();
                if (status == JobStatus.Stopped)
                {
                    break;
                }
            }
        }
    }
}