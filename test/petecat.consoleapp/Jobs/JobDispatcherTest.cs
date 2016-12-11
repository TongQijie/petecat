using Petecat.Console;
using Petecat.DependencyInjection;
using Petecat.Jobs;

namespace Petecat.ConsoleApp.Jobs
{
    public class JobDispatcherTest
    {
        public void Run()
        {
            var dispatcher = DependencyInjector.GetObject<IJobDispatcher>();
            dispatcher.Setup(new AppleClass());

            dispatcher.StartAll();

            ConsoleBridging.ReadAnyKey();

            dispatcher.StopAll();
        }
    }
}
