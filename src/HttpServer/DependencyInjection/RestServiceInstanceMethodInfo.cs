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
            DataFormat requestDataFormat = DataFormat.Any, 
            DataFormat responseDataFormat = DataFormat.Any,
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

        public DataFormat RequestDataFormat { get; set; }

        public DataFormat ResponseDataFormat { get; set; }

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
                        var attribute = parameterInfo.GetCustomAttribute<ParameterInfoAttribute>();
                        if (attribute == null)
                        {
                            _ParameterInfos = _ParameterInfos.Append(new RestServiceParameterInfo(parameterInfo, ParameterSource.Any, null));
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
