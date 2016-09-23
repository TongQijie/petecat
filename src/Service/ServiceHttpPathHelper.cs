using System.Linq;

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
    }
}
