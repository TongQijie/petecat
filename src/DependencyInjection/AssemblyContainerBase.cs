using System;
using System.Linq;
using System.Reflection;

using Petecat.Extending;

namespace Petecat.DependencyInjection
{
    public class AssemblyContainerBase : ContainerBase, IAssemblyContainer
    {
        public Assembly[] Assemblies { get; protected set; }

        public override object GetObject(Type targetType)
        {
            return InternalResolve(targetType);
        }

        public IAssemblyContainer RegisterAssemblies<T>() where T : IAssemblyInfo
        {
            Assemblies.Each(x => RegisterAssembly(typeof(T).CreateInstance<T>(x)));
            return this;
        }

        public void RegisterAssembly(IAssemblyInfo assemblyInfo)
        {
            var typeDefinitions = assemblyInfo.GetTypeDefinitions();
            if (typeDefinitions != null && typeDefinitions.Length > 0)
            {
                foreach (var typeDefinition in typeDefinitions)
                {
                    RegisterType(typeDefinition);
                }
            }
        }

        public bool CanInfer(Type targetType)
        {
            if (targetType.IsClass)
            {
                return RegisteredTypes.ContainsKey(targetType);
            }

            return RegisteredTypes.Values.ToArray().Exists(x => x.Inference != null && x.Inference.Equals(targetType));
        }

        private object InternalResolve(Type targetType)
        {
            if (targetType.IsClass)
            {
                ITypeDefinition typeDefinition;
                if (RegisteredTypes.TryGetValue(targetType, out typeDefinition))
                {
                    if (typeDefinition.ConstructorMethods == null || typeDefinition.ConstructorMethods.Length == 0)
                    {
                        throw new Exception(string.Format("constructor method cannot be found in type '{0}'", targetType.FullName));
                    }

                    var defaultConstructor = typeDefinition.ConstructorMethods[0];

                    var parameterValues = new object[defaultConstructor.ParameterInfos.Length];
                    for (int i = 0; i < parameterValues.Length; i++)
                    {
                        var parameterInfo = defaultConstructor.ParameterInfos.FirstOrDefault(x => x.Index == i);
                        if (parameterInfo == null)
                        {
                            throw new Exception(string.Format("parameter is missing in contructor method."));
                        }

                        parameterValues[i] = InternalResolve(parameterInfo.TypeDefinition.Info as Type);
                    }

                    return typeDefinition.GetInstance(parameterValues);
                }

                return null;
            }
            else if (targetType.IsInterface)
            {
                var typeDefinition = RegisteredTypes.Values.ToArray().Where(x => x.Inference != null && x.Inference.Equals(targetType))
                    .OrderByDescending(x => x.Priority).FirstOrDefault();
                if (typeDefinition == null)
                {
                    throw new Exception(string.Format("failed to find registered type inferred from interface '{0}'.", targetType.FullName));
                }

                return InternalResolve(typeDefinition.Info as Type);
            }
            else
            {
                return null;
            }
        }
    }
}