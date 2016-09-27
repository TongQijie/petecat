namespace Petecat.Test.Aop
{
    public class AppleBase
    {
        public virtual object SayHi(object welcome)
        {
            System.Console.WriteLine(welcome.ToString());
            return welcome;
        }
    }
}
