using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Petecat.Collection;
using Petecat.Utility;
using Petecat.Data.Formatters;
using Petecat.Extension;

namespace Petecat.IOC
{
    public class DefaultContainer : IContainer
    {
        private ThreadSafeKeyedObjectCollection<string, ITypeDefinition> _LoadedTypeDefinitions = new ThreadSafeKeyedObjectCollection<string, ITypeDefinition>();

        public IEnumerable<ITypeDefinition> LoadedTypeDefinitions { get { return _LoadedTypeDefinitions.Values; } }

        private ThreadSafeKeyedObjectCollection<string, IContainerObject> _LoadedContainerObjects = new ThreadSafeKeyedObjectCollection<string, IContainerObject>();

        public IEnumerable<IContainerObject> LoadedContainerObjects { get { return _LoadedContainerObjects.Values; } }

        public object Resolve(Type targetType, params object[] arguments)
        {
            var typeDefinition = _LoadedTypeDefinitions.Get(targetType.FullName, null);
            if (typeDefinition == null)
            {
                return null;
            }

            return typeDefinition.GetInstance(arguments);
        }

        public T Resolve<T>(params object[] arguments)
        {
            return (T)Resolve(typeof(T), arguments);
        }

        public object Resolve(Type targetType)
        {
            return InternalAutoResolve(targetType);
        }

        public T Resolve<T>()
        {
            return (T)InternalAutoResolve(typeof(T));
        }

        public object Resolve(string objectName)
        {
            var containerObject = _LoadedContainerObjects.Get(objectName, null);
            if (containerObject == null)
            {
                return null;
            }

            return containerObject.GetObject();
        }

        public T Resolve<T>(string objectName)
        {
            return (T)Resolve(objectName);
        }

        public bool ContainTypeDefinition(Type targetType)
        {
            return _LoadedTypeDefinitions.Values.ToList().Exists(x => targetType.FullName == x.Key);
        }

        public bool TryGetTypeDefinition(Type targetType, out ITypeDefinition typeDefinition)
        {
            typeDefinition = _LoadedTypeDefinitions.Values.FirstOrDefault(x => x.Key == targetType.FullName);
            return typeDefinition != null;
        }

        public void Register(params ITypeDefinition[] typeDefinitions)
        {
            if (typeDefinitions == null || typeDefinitions.Length == 0)
            {
                return;
            }

            typeDefinitions.Where(x => ReflectionUtility.ContainsCustomAttribute<Attributes.ResolvableAttribute>(x.Info)).ToList().ForEach(x => _LoadedTypeDefinitions.Add(x));
        }

        private object InternalAutoResolve(Type targetType)
        {
            if (targetType.IsClass)
            {
                ITypeDefinition typeDefinition;
                if (TryGetTypeDefinition(targetType, out typeDefinition))
                {
                    var defaultConstructor = typeDefinition.Constructors.FirstOrDefault();

                    foreach (var argument in defaultConstructor.MethodArguments)
                    {
                        argument.ArgumentValue = InternalAutoResolve(argument.ArgumentType);
                    }

                    return typeDefinition.GetInstance(defaultConstructor.MethodArguments.Select(x => x.ArgumentValue).ToArray());
                }

                return null;
            }
            else if (targetType.IsInterface)
            {
                var typeDefinition = _LoadedTypeDefinitions.Values.Where(x => x.IsImplementInterface(targetType)).FirstOrDefault(x =>
                {
                    Attributes.AutoResolvableAttribute attribute;
                    return ReflectionUtility.TryGetCustomAttribute<Attributes.AutoResolvableAttribute>(x.Info, y => y.SpecifiedType.FullName == targetType.FullName, out attribute);
                });

                if (typeDefinition == null)
                {
                    return null;
                }

                return InternalAutoResolve(typeDefinition.Info as Type);
            }
            else
            {
                return null;
            }
        }

        public void Register(string configFile)
        {
            if (!File.Exists(configFile.FullPath()))
            {
                throw new FileNotFoundException();
            }

            var instanceConfig = new XmlFormatter().ReadObject<Configuration.ContainerInstanceConfig>(configFile, Encoding.UTF8);

            foreach (var objectConfig in instanceConfig.Objects)
            {
                var containerObject = new DefaultContainerObject(objectConfig);

                if (objectConfig.Arguments != null && objectConfig.Arguments.Length > 0)
                {
                    containerObject.Arguments = new MethodArgument[objectConfig.Arguments.Length];

                    for (int i = 0; i < containerObject.Arguments.Length; i++)
                    {
                        var argument = new MethodArgument()
                        {
                            Name = objectConfig.Arguments[i].Name,
                            Index = objectConfig.Arguments[i].Index,
                        };

                        if (!objectConfig.Arguments[i].IsObjectValue)
                        {
                            argument.ArgumentValue = objectConfig.Arguments[i].StringValue;
                        }
                        else
                        {
                            var objectValue = _LoadedContainerObjects.Get(objectConfig.Arguments[i].ObjectName, null);
                            if (objectValue == null)
                            {
                                throw new Exception(string.Format("argument object {0} not found.", objectConfig.Arguments[i].ObjectName));
                            }

                            argument.ArgumentValue = objectValue.GetObject();
                        }

                        containerObject.Arguments[i] = argument;
                    }
                }

                if (objectConfig.Properties != null && objectConfig.Properties.Length > 0)
                {
                    containerObject.Properties = new InstanceProperty[objectConfig.Properties.Length];

                    for (int i = 0; i < containerObject.Properties.Length; i++)
                    {
                        var property = new InstanceProperty() { Name = objectConfig.Properties[i].Name };

                        if (!objectConfig.Properties[i].IsObjectValue)
                        {
                            property.PropertyValue = objectConfig.Properties[i].StringValue;
                        }
                        else
                        {
                            var objectValue = _LoadedContainerObjects.Get(objectConfig.Arguments[i].ObjectName, null);
                            if (objectValue == null)
                            {
                                throw new Exception(string.Format("argument object {0} not found.", objectConfig.Arguments[i].ObjectName));
                            }

                            property.PropertyValue = objectValue.GetObject();
                        }

                        containerObject.Properties[i] = property;
                    }
                }

                _LoadedTypeDefinitions.Add(containerObject.TypeDefinition);
                _LoadedContainerObjects.Add(containerObject);
            }
        }
    }
}