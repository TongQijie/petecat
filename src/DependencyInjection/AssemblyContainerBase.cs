using System;
using System.Linq;

using Petecat.Extension;

namespace Petecat.DependencyInjection
{
    public class AssemblyContainerBase : ContainerBase, IAssemblyContainer
    {
        public object GetObject(Type targetType)
        {
            throw new NotImplementedException();
        }

        public T GetObject<T>()
        {
            throw new NotImplementedException();
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

        private object InternalResolve(Type targetType)
        {
            if (targetType.IsClass)
            {
                ITypeDefinition typeDefinition;
                if (RegisteredTypes.TryGetValue(targetType, out typeDefinition))
                {
                    var defaultConstructor = typeDefinition.ConstructorMethods[0];

                    var parameterValues = new object[defaultConstructor.ParameterInfos.Length];
                    for (int i = 0; i < parameterValues.Length; i++)
                    {
                        var parameterInfo = defaultConstructor.ParameterInfos.FirstOrDefault(x => x.Index == i);
                        if (parameterInfo == null)
                        {
                            // TODO: throw
                        }

                        parameterValues[i] = InternalResolve(parameterInfo.TypeDefinition.Info as Type);
                    }

                    return typeDefinition.GetInstance(parameterValues);
                }

                return null;
            }
            else if (targetType.IsInterface)
            {
                var typeDefinition = RegisteredTypes.Values.ToArray().FirstOrDefault(x => x.Inference.Equals(targetType));
                if (typeDefinition == null)
                {
                    // TODO: throw
                }

                return InternalResolve(typeDefinition.Info as Type);
            }
            else
            {
                // TODO: throw
                return null;
            }
        }
    }
}