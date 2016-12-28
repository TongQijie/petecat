using System.Reflection;

using Petecat.DependencyInjection;

namespace Petecat.HttpServer.DependencyInjection
{
    public class RestServiceParameterInfo : ParameterInfoBase
    {
        public RestServiceParameterInfo(ParameterInfo parameterInfo, ParameterSource source, string alias)
            : base(parameterInfo)
        {
            Source = source;
            Alias = alias;
        }

        public ParameterSource Source { get; set; }

        public string Alias { get; set; }
    }
}