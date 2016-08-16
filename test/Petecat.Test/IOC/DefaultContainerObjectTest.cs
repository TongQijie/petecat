using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petecat.IoC;

namespace Petecat.Test.IoC
{
    [TestClass]
    public class DefaultContainerObjectTest
    {
        public DefaultContainerObjectTest()
        {
            AppDomainContainer.Initialize().RegisterContainerObjects("./objects.config");
        }

        [TestMethod]
        public void Resolve()
        {
            var apple1 = AppDomainContainer.Instance.Resolve<AppleClass>("apple1");
            Assert.IsTrue(apple1.BananaClass != null && !string.IsNullOrEmpty(apple1.Welcome));
            var apple2 = AppDomainContainer.Instance.Resolve<AppleClass>("apple2");
            Assert.IsTrue(apple2.BananaClass != null && !string.IsNullOrEmpty(apple2.Welcome));
            var apple3 = AppDomainContainer.Instance.Resolve<AppleClass>("apple3");
            Assert.IsTrue(apple3.BananaClass != null && !string.IsNullOrEmpty(apple3.Welcome));
            var apple4 = AppDomainContainer.Instance.Resolve<AppleClass>("apple4");
            Assert.IsTrue(apple4.Children != null && apple4.Children.Length == 2);
            var apple5 = AppDomainContainer.Instance.Resolve<AppleClass>("apple5");
            Assert.IsTrue(apple5.Children != null && apple5.Children.Length == 2);
        }
    }
}