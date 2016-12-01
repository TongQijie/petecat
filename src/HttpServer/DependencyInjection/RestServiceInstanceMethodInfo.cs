using System.Reflection;

using Petecat.DependencyInjection;

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
    }
}
