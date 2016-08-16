using Petecat.IoC.Attributes;

namespace Petecat.Test.IoC
{
    [Resolvable]
    public class BananaClass
    {
        public BananaClass()
        {
        }

        public BananaClass(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
