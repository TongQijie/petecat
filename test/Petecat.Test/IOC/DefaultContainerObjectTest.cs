using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petecat.IoC;

namespace Petecat.Test.IoC
{
    [TestClass]
    public class DefaultContainerObjectTest
    {
        public DefaultContainerObjectTest()
        {
            AppDomainIoCContainer.Initialize().RegisterContainerObjects("./IoC/objects.config");
        }

        [TestMethod]
        public void Resolve()
        {
            var apple1 = AppDomainIoCContainer.Instance.Resolve<AppleClass>("apple1");
            Assert.IsTrue(apple1.BananaClass != null && !string.IsNullOrEmpty(apple1.Welcome));
            var apple2 = AppDomainIoCContainer.Instance.Resolve<AppleClass>("apple2");
            Assert.IsTrue(apple2.BananaClass != null && !string.IsNullOrEmpty(apple2.Welcome));
            var apple3 = AppDomainIoCContainer.Instance.Resolve<AppleClass>("apple3");
            Assert.IsTrue(apple3.BananaClass != null && !string.IsNullOrEmpty(apple3.Welcome));
            var apple4 = AppDomainIoCContainer.Instance.Resolve<AppleClass>("apple4");
            Assert.IsTrue(apple4.Children != null && apple4.Children.Length == 2);
            var apple5 = AppDomainIoCContainer.Instance.Resolve<AppleClass>("apple5");
            Assert.IsTrue(apple5.Children != null && apple5.Children.Length == 2);
        }
    }
}