using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;

using Petecat.Collection;
using Petecat.Utility;
using Petecat.Data.Formatters;
using Petecat.Extension;
using Petecat.IoC.Configuration;

namespace Petecat.IoC
{
    public class DefaultIoCContainer : IIoCContainer
    {
        private ThreadSafeKeyedObjectCollection<string, ITypeDefinition> _LoadedTypeDefinitions = new ThreadSafeKeyedObjectCollection<string, ITypeDefinition>();

        public IEnumerable<ITypeDefinition> LoadedTypeDefinitions { get { return _LoadedTypeDefinitions.Values; } }

        private ThreadSafeKeyedObjectCollection<string, IIoCObject> _LoadedContainerObjects = new ThreadSafeKeyedObjectCollection<string, IIoCObject>();

        public IEnumerable<IIoCObject> LoadedContainerObjects { get { return _LoadedContainerObjects.Values; } }

        #region Get Instances

        public object Resolve(Type targetType, params object[] arguments)
        {
            var typeDefinition = GetTypeDefinition(targetType);
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
                var typeDefinition = _LoadedTypeDefinitions.Values.Where(x => x.Info is Type && targetType.IsAssignableFrom(x.Info as Type)).FirstOrDefault(x =>
                {
                    Attributes.AutoResolvableAttribute attribute;
                    return Reflector.TryGetCustomAttribute(x.Info, y => y.SpecifiedType.FullName == targetType.FullName, out attribute);
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

        #endregion

        #region Check & Get Types

        public bool ContainsTypeDefinition(Type targetType)
        {
            var targetTypeKey = targetType.FullName + "," + targetType.Module.FullyQualifiedName;

            return _LoadedTypeDefinitions.ContainsKey(targetTypeKey);
        }

        public ITypeDefinition GetTypeDefinition(Type targetType)
        {
            var targetTypeKey = targetType.FullName + "," + targetType.Module.FullyQualifiedName;

            if (!_LoadedTypeDefinitions.ContainsKey(targetTypeKey))
            {
                RegisterContainerAssembly(targetType.Module.FullyQualifiedName);
            }

            return _LoadedTypeDefinitions.Get(targetTypeKey, null);
        }

        public bool TryGetTypeDefinition(Type targetType, out ITypeDefinition typeDefinition)
        {
            typeDefinition = GetTypeDefinition(targetType);
            return typeDefinition != null;
        }

        public bool TryGetTypeDefinition(string typeString, out ITypeDefinition typeDefinition)
        {
            var fields = typeString.SplitByChar(',');
            if (fields.Length == 1)
            {
                typeDefinition = _LoadedTypeDefinitions.Values.FirstOrDefault(x => x.FullName == fields[0]);
                return typeDefinition != null;
            }
            else if (fields.Length == 2)
            {
                typeDefinition = _LoadedTypeDefinitions.Values.FirstOrDefault(x => x.FullName == fields[0] && x.AssemblyInfo.Name == fields[1]);
                return typeDefinition != null;
            }

            typeDefinition = null;
            return false;
        }

        #endregion

        #region Register Types & Objects

        public void RegisterContainerAssembly(Assembly assembly)
        {
            try
            {
                assembly.GetTypes().Where(x => Reflector.ContainsCustomAttribute<Attributes.ResolvableAttribute>(x)).ToList().ForEach(x => RegisterContainerType(x));
            }
            catch (Exception e)
            {
                throw new Errors.ContainerAssemblyRegisterFailedException(assembly.FullName, e);
            }
        }

        public void RegisterContainerType(Type type)
        {
            var typeDefinition = new DefaultTypeDefinition(type);
            _LoadedTypeDefinitions.Add(typeDefinition);

            if (type.IsClass)
            {
                if (type.BaseType != null && type.BaseType != typeof(object))
                {
                    RegisterContainerType(type.BaseType);
                }

                type.GetInterfaces().ToList().ForEach(x => RegisterContainerType(x));
            }
            else if (type.IsInterface)
            {
                type.GetInterfaces().ToList().ForEach(x => RegisterContainerType(x));
            }
        }

        public void RegisterContainerAssembly(string assemblyPath)
        {
            if (!File.Exists(assemblyPath))
            {
                throw new FileNotFoundException(assemblyPath);
            }

            RegisterContainerAssembly(Assembly.LoadFile(assemblyPath));
        }

        public void RegisterContainerObjects(string objectsFile)
        {
            if (!File.Exists(objectsFile.FullPath()))
            {
                throw new FileNotFoundException();
            }

            var containerObjectsConfig = new XmlFormatter().ReadObject<ContainerObjectsConfig>(objectsFile.FullPath());

            if (containerObjectsConfig.Assemblies != null && containerObjectsConfig.Assemblies.Length > 0)
            {
                foreach (var containerAssemblyConfig in containerObjectsConfig.Assemblies)
                {
                    RegisterContainerAssembly(containerAssemblyConfig);
                }
            }

            if (containerObjectsConfig.Importers != null && containerObjectsConfig.Importers.Length > 0)
            {
                foreach (var containerImporterConfig in containerObjectsConfig.Importers)
                {
                    RegisterContainerObjects(containerImporterConfig.Path);
                }
            }

            if (containerObjectsConfig.Objects != null && containerObjectsConfig.Objects.Length > 0)
            {
                foreach (var containerObjectConfig in containerObjectsConfig.Objects)
                {
                    RegisterContainerObject(containerObjectConfig);
                }
            }
        }

        private void RegisterContainerAssembly(ContainerAssemblyConfig containerAssemblyConfig)
        {
            if (containerAssemblyConfig != null)
            {
                RegisterContainerAssembly(containerAssemblyConfig.Path);
            }
        }

        private void RegisterContainerObject(ContainerObjectConfig containerObjectConfig)
        {
            if (containerObjectConfig == null)
            {
                return;
            }

            ITypeDefinition typeDefinition;
            if (!TryGetTypeDefinition(containerObjectConfig.Type, out typeDefinition))
            {
                return;
            }

            var containerObject = new DefaultIoCObject(containerObjectConfig.Name, containerObjectConfig.Singleton, typeDefinition);

            if (containerObjectConfig.Arguments != null && containerObjectConfig.Arguments.Length > 0)
            {
                containerObject.Arguments = new MethodArgument[containerObjectConfig.Arguments.Length];

                for (int i = 0; i < containerObject.Arguments.Length; i++)
                {
                    containerObject.Arguments[i] = new MethodArgument()
                    {
                        Name = containerObjectConfig.Arguments[i].Name,
                        Index = containerObjectConfig.Arguments[i].Index,
                        ArgumentValue = GetElementValue(containerObjectConfig.Arguments[i]),
                    };
                }
            }

            if (containerObjectConfig.Properties != null && containerObjectConfig.Properties.Length > 0)
            {
                containerObject.Properties = new InstanceProperty[containerObjectConfig.Properties.Length];

                for (int i = 0; i < containerObject.Properties.Length; i++)
                {
                    containerObject.Properties[i] = new InstanceProperty()
                    {
                        Name = containerObjectConfig.Properties[i].Name,
                        PropertyValue = GetElementValue(containerObjectConfig.Properties[i]),
                    };
                }
            }

            _LoadedTypeDefinitions.Add(containerObject.TypeDefinition);
            _LoadedContainerObjects.Add(containerObject);
        }

        private object GetElementValue(Configuration.ContainerObjectValueElementConfig containerObjectValueElementConfig)
        {
            if (containerObjectValueElementConfig.IsReferenceObject)
            {
                var objectValue = _LoadedContainerObjects.Get(containerObjectValueElementConfig.ObjectName, null);
                if (objectValue == null)
                {
                    throw new Errors.ContainerObjectNotFoundException(containerObjectValueElementConfig.ObjectName);
                }

                return objectValue.GetObject();
            }
            else if (containerObjectValueElementConfig.IsDirectObject)
            {
                var config = new XmlFormatter().ReadObject<ContainerObjectConfig>(containerObjectValueElementConfig.ElementValue.OuterXml, Encoding.UTF8);
                if (string.IsNullOrEmpty(config.Name))
                {
                    config.Name = Guid.NewGuid().ToString();
                }
                RegisterContainerObject(config);

                var objectValue = _LoadedContainerObjects.Get(config.Name, null);
                if (objectValue == null)
                {
                    throw new Errors.ContainerObjectNotFoundException(config.Name);
                }

                return objectValue.GetObject();
            }
            else if (containerObjectValueElementConfig.IsValueCollection)
            {
                var config = new XmlFormatter().ReadObject<ContainerObjectValueCollectionConfig>(containerObjectValueElementConfig.ElementValue.OuterXml, Encoding.UTF8);
                if (config == null || config.Elements == null || config.Elements.Length == 0)
                {
                    return null;
                }

                var values = new object[config.Elements.Length];
                for (int i = 0; i < config.Elements.Length; i++)
                {
                    values[i] = GetElementValue(config.Elements[i]);
                }

                return values;
            }
            else
            {
                return containerObjectValueElementConfig.StringValue;
            }
        }

        #endregion
    }
}