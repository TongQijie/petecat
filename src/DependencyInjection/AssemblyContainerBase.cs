using System;
using System.Linq;

using Petecat.Extending;

namespace Petecat.DependencyInjection
{
    public class AssemblyContainerBase : ContainerBase, IAssemblyContainer
    {
        public override object GetObject(Type targetType)
        {
            return InternalResolve(targetType);
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
                var typeDefinition = RegisteredTypes.Values.ToArray().Where(x => x.Inference != null && x.Inference.Equals(targetType))
                    .OrderByDescending(x => x.Priority).FirstOrDefault();
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