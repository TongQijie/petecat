using System;

namespace Petecat.Service
{
    [Obsolete]
    public class HttpApplicationConfigManager
    {
        private static HttpApplicationConfigManager _Instance = null;

        public static HttpApplicationConfigManager Instance { get { return _Instance ?? (_Instance = new HttpApplicationConfigManager()); } }

        public string GetStaticResourceContentMapping(string key)
        {
            throw new NotImplementedException();
        }

        public string GetHttpRouting(string key)
        {
            throw new NotImplementedException();
        }
    }
}
