using Petecat.IOC.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petecat.Test.IOC
{
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
