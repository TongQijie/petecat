using Petecat.IOC.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petecat.Test.IOC
{
    [AutoResolvable(typeof(AppleClass))]
    public class AppleClass
    {
        public AppleClass(BananaClass bananaClass, IPearClass pearClass)
        {
            BananaClass = bananaClass;
            PearClass = pearClass;
        }

        public BananaClass BananaClass { get; set; }

        public IPearClass PearClass { get; set; }

        public string SayHi(string welcome)
        {
            return welcome;
        }
    }
}
