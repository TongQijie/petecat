using System.Collections.Generic;

using Petecat.Data.Formatters;

namespace Petecat.Network.Http
{
    static class HttpConstants
    {
        public static Dictionary<HttpContentType, string> HttpContentTypeStringMapping { get; set; }

        public static Dictionary<HttpContentType, DataFormatterContent> HttpContentTypeFormatterMapping { get; set; }

        static HttpConstants()
        {
            HttpContentTypeStringMapping = new Dictionary<HttpContentType, string>();
            HttpContentTypeStringMapping.Add(HttpContentType.Xml, "application/xml");
            HttpContentTypeStringMapping.Add(HttpContentType.Json, "application/json");
            HttpContentTypeStringMapping.Add(HttpContentType.FormUrlEncoded, "application/x-www-form-urlencoded");

            HttpContentTypeFormatterMapping = new Dictionary<HttpContentType, DataFormatterContent>();
            HttpContentTypeFormatterMapping.Add(HttpContentType.Xml, DataFormatterContent.Xml);
            HttpContentTypeFormatterMapping.Add(HttpContentType.DataContractXml, DataFormatterContent.DataContractXml);
            HttpContentTypeFormatterMapping.Add(HttpContentType.Json, DataFormatterContent.DataContractJson);
        }
    }
}
