using Petecat.Threading.Tasks;
using System;
using System.Threading;

namespace Petecat.ConsoleApp
{
    public class BananaTaskObject : TaskObjectBase
    {
        public BananaTaskObject(string name, string description)
            : base(name, description)
        {
            Implement = (t) =>
            {
                while (true)
                {
                    Petecat.Console.ConsoleBridging.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Thread.Sleep(3000);

                    if (CheckTransitionalStatus() == TaskObjectStatus.Sleep)
                    {
                        return true;
                    }
                }
            };
        }

        protected override void OnResume()
        {
            Console.ConsoleBridging.WriteLine("Resuming...");
        }

        protected override void OnSuspend()
        {
            Console.ConsoleBridging.WriteLine("suspending...");
        }

        protected override void OnTerminate()
        {
            Console.ConsoleBridging.WriteLine("Terminating...");
        }
    }
}
