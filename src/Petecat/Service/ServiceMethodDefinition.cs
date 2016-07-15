using Petecat.IOC;
using Petecat.Utility;

using System;

namespace Petecat.Service
{
    public class ServiceMethodDefinition
    {
        public ServiceMethodDefinition(IInstanceMethodDefinition instanceMethodDefinition)
        {
            Attributes.ServiceMethodAttribute attribute;
            if (!ReflectionUtility.TryGetCustomAttribute<Attributes.ServiceMethodAttribute>(instanceMethodDefinition.Info, null, out attribute))
            {
                throw new Exception("methods without ServiceMethodAttribute cannot be defined into ServiceMethod.");
            }

            ServiceMethod = instanceMethodDefinition;

            if (!string.IsNullOrWhiteSpace(attribute.MethodName))
            {
                MethodName = attribute.MethodName;
            }
            else
            {
                MethodName = instanceMethodDefinition.MethodName;
            }

            Method = attribute.AccessMethods;
            IsDefaultMethod = attribute.IsDefaultMethod;
        }

        public string MethodName { get; private set; }

        public ServiceAccessMethod Method { get; private set; }

        public bool IsDefaultMethod { get; private set; }

        public IInstanceMethodDefinition ServiceMethod { get; private set; }
    }
}
