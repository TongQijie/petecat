using System.Reflection;

using Petecat.DependencyInjection;

namespace Petecat.HttpServer.DependencyInjection
{
    public class RestServiceInstanceMethodInfo : InstanceMethodInfoBase
    {
        public RestServiceInstanceMethodInfo(ITypeDefinition typeDefinition, MethodInfo methodInfo, string serviceMethodName, 
            bool isDefaultMethod = false, RestServiceDataFormat dataFormat = RestServiceDataFormat.Any)
            : base(typeDefinition, methodInfo)
        {
            ServiceMethodName = serviceMethodName;
            IsDefaultMethod = isDefaultMethod;
            DataFormat = dataFormat;
        }

        public string ServiceMethodName { get; set; }

        public bool IsDefaultMethod { get; set; }

        public RestServiceDataFormat DataFormat { get; set; }
    }
}
