﻿using Petecat.IOC.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petecat.Test.IOC
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