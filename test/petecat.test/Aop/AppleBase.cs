namespace Petecat.Test.Aop
{
    public class AppleBase
    {
        public virtual string SayHi(string welcome)
        {
            System.Console.WriteLine(welcome.ToString());
            return welcome;
        }

        public virtual string SayTo(string welcome, string to)
        {
            System.Console.WriteLine(to + ":" + welcome);
            return to + ":" + welcome;
        }
    }
}
