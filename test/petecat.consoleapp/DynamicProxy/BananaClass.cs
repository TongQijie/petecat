namespace Petecat.ConsoleApp.DynamicProxy
{
    public class BananaClass
    {
        public virtual void A()
        {
            Console.ConsoleBridging.WriteLine("run in A");
        }

        public virtual void B(int a)
        {
            Console.ConsoleBridging.WriteLine("run in B with " + a);
        }

        public virtual int C(int a)
        {
            Console.ConsoleBridging.WriteLine("run in C with " + a);
            return a + 1;
        }
    }
}
