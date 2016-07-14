using System;

namespace Petecat.Service
{
    static class ServiceHttpPathHelper
    {
        public static string[] Get(string uri)
        {
            return uri.Split('/');
        }
    }
}
