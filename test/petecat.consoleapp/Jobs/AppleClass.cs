using Petecat.Jobs;
using Petecat.Threading;
using System;

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
                Console.WriteLine(string.Format("{0}: {1}", Name, Status));

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