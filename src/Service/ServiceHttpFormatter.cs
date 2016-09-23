using Petecat.Data.Formatters;

namespace Petecat.Service
{
    internal static class ServiceHttpFormatter
    {
        public static IObjectFormatter GetFormatter(params string[] contentTypes)
        {
            foreach (var contentType in contentTypes)
            {
                if (contentType.ToLower().Contains("application/xml"))
                {
                    return ObjectFormatterFactory.GetFormatter(ObjectFormatterType.Xml);
                }
                else if (contentType.ToLower().Contains("application/json"))
                {
                    return new JsonFormatter() { OmitDefaultValueProperty = true };
                }
            }

            return null;
        }
    }
}
