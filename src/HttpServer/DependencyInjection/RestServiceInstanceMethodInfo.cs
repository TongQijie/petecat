using System.Reflection;

using Petecat.Extending;
using Petecat.DependencyInjection;
using Petecat.HttpServer.Attribute;

namespace Petecat.HttpServer.DependencyInjection
{
    public class RestServiceInstanceMethodInfo : InstanceMethodInfoBase
    {
        public RestServiceInstanceMethodInfo(ITypeDefinition typeDefinition, MethodInfo methodInfo, string serviceMethodName, 
            bool isDefaultMethod = false, 
            RestServiceDataFormat requestDataFormat = RestServiceDataFormat.Any, 
            RestServiceDataFormat responseDataFormat = RestServiceDataFormat.Any,
            HttpVerb httpVerb = HttpVerb.Get)
            : base(typeDefinition, methodInfo)
        {
            ServiceMethodName = serviceMethodName;
            IsDefaultMethod = isDefaultMethod;
            RequestDataFormat = requestDataFormat;
            ResponseDataFormat = responseDataFormat;
            HttpVerb = httpVerb;
        }

        public string ServiceMethodName { get; set; }

        public bool IsDefaultMethod { get; set; }

        public RestServiceDataFormat RequestDataFormat { get; set; }

        public RestServiceDataFormat ResponseDataFormat { get; set; }

        public HttpVerb HttpVerb { get; set; }

        public override IParameterInfo[] ParameterInfos
        {
            get
            {
                if (_ParameterInfos == null)
                {
                    var methodInfo = MethodDefinition.Info as MethodInfo;

                    _ParameterInfos = new ParameterInfoBase[0];
                    foreach (var parameterInfo in methodInfo.GetParameters())
                    {
                        var attribute = parameterInfo.GetCustomAttribute<RestServiceParameterAttribute>();
                        if (attribute == null)
                        {
                            _ParameterInfos = _ParameterInfos.Append(new RestServiceParameterInfo(parameterInfo, RestServiceParameterSource.Any, null));
                        }
                        else
                        {
                            _ParameterInfos = _ParameterInfos.Append(new RestServiceParameterInfo(parameterInfo, attribute.Source, attribute.Alias));
                        }
                    }
                }

                return _ParameterInfos;
            }
        }
    }
}
