using Petecat.IOC.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petecat.Test.IOC
{
    [AutoResolvable(typeof(BananaClass))]
    public class BananaClass
    {
        public string Name { get; set; }
    }
}
