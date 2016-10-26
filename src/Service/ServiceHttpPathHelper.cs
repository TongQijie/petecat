using System;
using System.Linq;

using Petecat.Extension;

namespace Petecat.Service
{
    static class ServiceHttpPathHelper
    {
        public static string[] Get(string uri)
        {
            uri = uri.TrimStart('/');
            if (uri.Contains("?"))
            {
                uri = uri.Remove(uri.IndexOf('?'));
            }

            return uri.Split('/').Where(x => !string.IsNullOrWhiteSpace(x.Trim())).ToArray();
        }

        public static bool TryParseServiceUri(string uri, out string serviceName, out string methodName)
        {
            serviceName = null;
            methodName = null;

            var fields = Get(uri);
            var virtualPath = ServiceRoutingManager.Instance.GetRoutingRule("VirtualPath");
            if (!virtualPath.HasValue())
            {
                if (fields.Length > 0)
                {
                    serviceName = fields[0];
                }
                if (fields.Length > 1)
                {
                    methodName = fields[1];
                }
            }
            else
            {
                var paths = virtualPath.SplitByChar('/');
                for (int i = 0; i < paths.Length; i++)
                {
                    if (fields.Length <= i || !string.Equals(paths[i], fields[i], StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }

                if (fields.Length > paths.Length)
                {
                    serviceName = fields[paths.Length];
                }
                if (fields.Length > (paths.Length + 1))
                {
                    methodName = fields[paths.Length + 1];
                }
            }

            return true;
        }
    }
}
