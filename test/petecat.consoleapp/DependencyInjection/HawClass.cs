namespace Petecat.ConsoleApp.DependencyInjection
{
    public class HawClass
    {
        public HawClass() { }

        public HawClass(GrapeClass grapeClass)
        {
            GrapeClass = grapeClass;
        }

        public GrapeClass GrapeClass { get; set; }
    }
}
