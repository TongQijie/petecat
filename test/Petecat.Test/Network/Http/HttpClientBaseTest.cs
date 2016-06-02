using Microsoft.VisualStudio.TestTools.UnitTesting;

using Petecat.Network.Http;
using System.Net;
using System.Text;

namespace Petecat.Test.Network.Http
{
    [TestClass]
    public class HttpClientBaseTest
    {
        [TestMethod]
        public void Get()
        {
            var httpClient = new HttpClientBase("https://hephap.com");
            httpClient.Proxy = new WebProxy("s1firewall", 8080);
            httpClient.Proxy.Credentials = new NetworkCredential("jt69", "newegg@1234", "buyabs.corp");
            var html = httpClient.Get(Encoding.UTF8);
        }
    }
}