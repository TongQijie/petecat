using Petecat.DependencyInjection;
using System.Reflection;

namespace Petecat.HttpServer.DependencyInjection
{
    public class RestServiceInstanceMethodInfo : InstanceMethodInfoBase
    {
        public RestServiceInstanceMethodInfo(ITypeDefinition typeDefinition, MethodInfo methodInfo, string serviceMethodName, bool isDefaultMethod = false)
            : base(typeDefinition, methodInfo)
        {
            ServiceMethodName = serviceMethodName;
            IsDefaultMethod = isDefaultMethod;
        }

        public string ServiceMethodName { get; set; }

        public bool IsDefaultMethod { get; set; }
    }
}
