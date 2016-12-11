using Petecat.Formatter;
using Petecat.Extending;
using Petecat.Configuring;
using Petecat.Formatter.Json;
using Petecat.DependencyInjection.Configuration;

using System;
using System.Text.RegularExpressions;

namespace Petecat.DependencyInjection
{
    public class ConfigurableFileInfoBase : IConfigurableFileInfo
    {
        public ConfigurableFileInfoBase(string path)
        {
            Path = path;
        }

        public string Path { get; private set; }

        private IInstanceInfo[] _InstanceInfos = new IInstanceInfo[0];

        public IInstanceInfo[] InstanceInfos { get { return _InstanceInfos; } }

        public IInstanceInfo[] GetInstanceInfos()
        {
            _InstanceInfos = new IInstanceInfo[0];

            var container = DependencyInjector.GetObject<IStaticFileConfigurer>().GetValue(Path) as ConfigurableContainerConfiguration;
            if (container == null)
            {
                // TODO: throw
            }

            if (container.Containers != null && container.Containers.Length > 0)
            {
                foreach (var includedContainer in container.Containers)
                {
                    _InstanceInfos = _InstanceInfos.Append(new ConfigurableFileInfoBase(includedContainer.RelativePath).GetInstanceInfos());
                }
            }

            if (container.Instances != null && container.Instances.Length > 0)
            {
                foreach (var instance in container.Instances)
                {
                    if (instance.Name.HasValue() && instance.Type.HasValue())
                    {
                        var type = Type.GetType(instance.Type);

                        var instanceInfo = new InstanceInfoBase(instance.Name, new TypeDefinitionBase(type), instance.Singleton);

                        if (instance.Parameters != null && instance.Parameters.Length > 0)
                        {
                            foreach (var constructorMethod in instanceInfo.TypeDefinition.ConstructorMethods)
                            {
                                var parameterInfos = MatchParameter(constructorMethod, instance.Parameters);
                                if (parameterInfos != null)
                                {
                                    instanceInfo.ParameterInfos = parameterInfos;
                                    break;
                                }
                            }

                            if (instanceInfo.ParameterInfos == null)
                            {
                                // TODO: throw
                            }
                        }

                        if (instance.Properties != null && instance.Properties.Length > 0)
                        {
                            var propertyInfos = MatchProperty(instanceInfo, instance.Properties);
                            if (propertyInfos == null)
                            {
                                // TODO: throw
                            }

                            instanceInfo.PropertyInfos = propertyInfos;
                        }

                        _InstanceInfos = _InstanceInfos.Append(instanceInfo);
                    }
                }
            }

            return _InstanceInfos;
        }

        private IPropertyInfo[] MatchProperty(IInstanceInfo instanceInfo, InstancePropertyConfiguration[] properties)
        {
            var propertyInfos = new IPropertyInfo[properties.Length];

            for (var i = 0; i < properties.Length; i++)
            {
                var propertyInfo = (instanceInfo.TypeDefinition.Info as Type).GetProperty(properties[i].Name);
                if (propertyInfo == null)
                {
                    return null;
                }

                propertyInfos[i] = new PropertyInfoBase(instanceInfo.TypeDefinition, propertyInfo)
                {
                    PropertyValue = Convert(properties[i].Value, propertyInfo.PropertyType),
                };
            }

            return propertyInfos;
        }

        private IParameterInfo[] MatchParameter(IMethodInfo methodInfo, InstanceParameterConfiguration[] parameters)
        {
            if (methodInfo.ParameterInfos.Length != parameters.Length)
            {
                return null;
            }

            var parameterInfos = new IParameterInfo[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameterInfo = methodInfo.ParameterInfos.FirstOrDefault(x => string.Equals(parameters[i].Name, x.ParameterName)
                    || parameters[i].Index == x.Index);
                if (parameterInfo == null)
                {
                    break;
                }

                var parameterType = parameterInfo.TypeDefinition.Info as Type;

                try
                {
                    parameterInfos[i] = new ParameterInfoBase()
                    {
                        Index = parameterInfo.Index,
                        ParameterName = parameterInfo.ParameterName,
                        TypeDefinition = new TypeDefinitionBase(parameterType),
                        ParameterValue = Convert(parameters[i].Value, parameterType),
                    };
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return parameterInfos;
        }

        private object Convert(JsonObject jsonObject, Type targetType)
        {
            if (jsonObject is JsonDictionaryObject && targetType.IsClass)
            {
                return DependencyInjector.GetObject<IJsonFormatter>().ReadObject(targetType, jsonObject);
            }
            else if (jsonObject is JsonCollectionObject && targetType.IsArray)
            {
                var collection = jsonObject as JsonCollectionObject;

                var values = Array.CreateInstance(targetType.GetElementType(), collection.Elements.Length);
                for (var i = 0; i < collection.Elements.Length; i++)
                {
                    values.SetValue(Convert(collection.Elements[i].Value, targetType.GetElementType()), i);
                }

                return values;
            }
            else if (jsonObject is JsonValueObject)
            {
                var stringValue = (jsonObject as JsonValueObject).ToString();

                if (Regex.IsMatch(stringValue.Trim(), @"^\x24\x7B\S+\x7D$"))
                {
                    var objectName = stringValue.Trim().Substring(2, stringValue.Trim().Length - 3);

                    var instanceInfo = InstanceInfos.FirstOrDefault(x => string.Equals(x.Name, objectName, StringComparison.OrdinalIgnoreCase));
                    if (instanceInfo == null)
                    {
                        throw new Exception("");
                    }

                    return instanceInfo.GetInstance();
                }
                else
                {
                    return stringValue.ConvertTo(targetType);
                }
            }
            else
            {
                throw new Exception("");
            }
        }
    }
}
