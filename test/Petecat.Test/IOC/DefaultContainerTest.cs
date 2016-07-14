using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Reflection;
using System.Linq;

using Petecat.IOC;
using Petecat.Service;

namespace Petecat.Test.IOC
{
    [TestClass]
    public class DefaultContainerTest
    {
        [TestMethod]
        public void AutoResolve()
        {
            var container = new DefaultContainer();
            container.Register(Assembly.GetExecutingAssembly().GetTypes().Select(x => new DefaultTypeDefinition(x)).ToArray());

            var apple = container.AutoResolve<AppleClass>();
            var banana = container.AutoResolve<BananaClass>();
            var pear = container.AutoResolve<IPearClass>();
        }
    }
}
