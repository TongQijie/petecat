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
                    Petecat.Console.CommonUtility.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
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
            Console.CommonUtility.WriteLine("Resuming...");
        }

        protected override void OnSuspend()
        {
            Console.CommonUtility.WriteLine("suspending...");
        }

        protected override void OnTerminate()
        {
            Console.CommonUtility.WriteLine("Terminating...");
        }
    }
}
