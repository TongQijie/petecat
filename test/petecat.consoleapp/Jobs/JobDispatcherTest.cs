using Petecat.DependencyInjection;
using Petecat.Jobs;

using System;

namespace Petecat.ConsoleApp.Jobs
{
    public class JobDispatcherTest
    {
        public void Run()
        {
            var dispatcher = DependencyInjector.GetObject<IJobDispatcher>();
            dispatcher.Setup(new AppleClass());

            dispatcher.StartAll();

            Console.ReadKey();

            dispatcher.StopAll();
        }
    }
}
