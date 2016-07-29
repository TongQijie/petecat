namespace Petecat.Test.IoC
{
    public class AppleClass
    {
        public AppleClass() { }

        public AppleClass(BananaClass bananaClass)
        {
            BananaClass = bananaClass;
        }

        public AppleClass(BananaClass bananaClass, string welcome)
        {
            BananaClass = bananaClass;
            Welcome = welcome;
        }

        public BananaClass BananaClass { get; set; }

        //public IPearClass PearClass { get; set; }

        public string SayHi(string welcome)
        {
            return welcome;
        }

        public string Welcome { get; set; }
    }
}
