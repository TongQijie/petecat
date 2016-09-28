namespace Petecat.Test.Aop
{
    public class AppleBase
    {
        public virtual string SayHi(string welcome)
        {
            System.Console.WriteLine(welcome);
            return welcome;
        }

        public virtual string SayTo(string welcome, string to)
        {
            System.Console.WriteLine(to + ":" + welcome);
            return to + ":" + welcome;
        }

        public virtual void KeepSilent(string to)
        {
            System.Console.WriteLine(to + ":silent");
        }

        public virtual void DoNothing()
        {
            System.Console.WriteLine("do nothing");
        }

        public virtual string SayTo(string welcome, string toOne, string toAnotherOne)
        {
            System.Console.WriteLine(toOne + "," + toAnotherOne + ":" + welcome);
            return toOne + "," + toAnotherOne + ":" + welcome;
        }
    }
}
