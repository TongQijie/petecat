namespace Petecat.ConsoleApp.Data
{
    public class AppleClass
    {
        public AppleClass()
        {
            _A = 2;
        }

        private int _A;

        public string B { get; set; }

        public BananaClass Banana { get; set; }
    }
}
