using Petecat.Service.Attributes;

namespace Petecat.Test.IOC
{
    [AutoService(typeof(IPearClass))]
    public class PearClass : IPearClass
    {
        [ServiceMethod]
        public string SayHi(string welcome)
        {
            return welcome;
        }
    }
}
