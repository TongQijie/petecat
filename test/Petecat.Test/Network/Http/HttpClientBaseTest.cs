using Microsoft.VisualStudio.TestTools.UnitTesting;

using Petecat.Network.Http;
using System.Collections.Generic;

namespace Petecat.Test.Network.Http
{
    [TestClass]
    public class HttpClientBaseTest
    {
        [TestMethod]
        public void GetObject()
        {
            var queryStringKeyValues = new Dictionary<string, string>();
            queryStringKeyValues.Add("u", "hey, <world>.");
            queryStringKeyValues.Add("p", "world");

            var httpClientRequest = new HttpClientRequest(HttpVerb.GET, "http://localhost:60932/service.svc/getva0", queryStringKeyValues);
            using (var httpClientResponse = httpClientRequest.GetResponse())
            {
                var globalSession = httpClientResponse.GetObject<GlobalSession>();
            }
        }

        [TestMethod]
        public void PostObject()
        {
            var httpClientRequest = new HttpClientRequest(HttpVerb.POST, "http://localhost:60932/service.svc/getva1");
            using (var httpClientResponse = httpClientRequest.GetResponse(HttpContentType.Json, new Identification() { Username = "hey", Password = "world!" }))
            {
                var globalSession = httpClientResponse.GetObject<GlobalSession>();
            }
        }
    }
}