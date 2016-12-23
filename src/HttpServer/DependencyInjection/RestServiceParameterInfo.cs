using System.Reflection;

using Petecat.DependencyInjection;

namespace Petecat.HttpServer.DependencyInjection
{
    public class RestServiceParameterInfo : ParameterInfoBase
    {
        public RestServiceParameterInfo(ParameterInfo parameterInfo, RestServiceParameterSource source, string alias)
            : base(parameterInfo)
        {
            Source = source;
            Alias = alias;
        }

        public RestServiceParameterSource Source { get; set; }

        public string Alias { get; set; }
    }
}