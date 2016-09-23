using Petecat.Data.Formatters;

namespace Petecat.Network.Http
{
    public class HttpFormatterSelector
    {
        public static IObjectFormatter Get(params string[] contentTypes)
        {
            foreach (var contentType in contentTypes)
            {
                if (contentType.ToLower().Contains("application/xml"))
                {
                    return ObjectFormatterFactory.GetFormatter(ObjectFormatterType.Xml);
                }
                else if (contentType.ToLower().Contains("application/json"))
                {
                    return ObjectFormatterFactory.GetFormatter(ObjectFormatterType.Json);
                }
            }

            return null;
        }
    }
}
