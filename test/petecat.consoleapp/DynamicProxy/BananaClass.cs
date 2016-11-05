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

        public virtual void C(int a, int b)
        {
            Console.ConsoleBridging.WriteLine("run in C with " + (int)(a + b));
        }

        public virtual int D()
        {
            Console.ConsoleBridging.WriteLine("run in D");
            return 1;
        }

        public virtual int E(int a)
        {
            Console.ConsoleBridging.WriteLine("run in E with " + a);
            return a + 1;
        }

        public virtual int F(int a, int b)
        {
            Console.ConsoleBridging.WriteLine("run in F with " + (int)(a + b));
            return a + b;
        }
    }
}
