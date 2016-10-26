using Petecat.Extension;
using System;

namespace Petecat.Service
{
    static class StaticResourceHttpPathHelper
    {
        public static bool TryParseStaticResourcePath(string url, out string relativePath, out string resourceType)
        {
            url = url.TrimStart('/');
            if (url.Contains("?"))
            {
                url = url.Remove(url.IndexOf('?'));
            }

            var virtualPath = ServiceRoutingManager.Instance.GetRoutingRule("VirtualPath");
            if (!virtualPath.HasValue())
            {
                relativePath = url;
                resourceType = relativePath.Substring(relativePath.LastIndexOf('.') + 1);
            }
            else
            {
                var fields = url.SplitByChar('/');
                var paths = virtualPath.SplitByChar('/');
                for (int i = 0; i < paths.Length; i++)
                {
                    if (fields.Length <= i || !string.Equals(paths[i], fields[i], StringComparison.OrdinalIgnoreCase))
                    {
                        relativePath = string.Empty;
                        resourceType = string.Empty;
                        return false;
                    }
                }

                relativePath = string.Join("/", fields.SubArray(paths.Length));
                resourceType = relativePath.Substring(relativePath.LastIndexOf('.') + 1);
            }

            return true;
        }
    }
}
