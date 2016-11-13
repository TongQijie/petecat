﻿using Petecat.Utility;
using Petecat.Formatter;
using Petecat.Extension;
using Petecat.Formatter.Json;
using Petecat.DependencyInjection.Configuration;

using System;
using System.Text.RegularExpressions;
using Petecat.Configuring;

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

            var container = DependencyInjector.GetObject<IStaticFileConfigurer>().GetValue<ConfigurableContainerConfiguration>(Path);
            if (container == null)
            {
                // TODO: throw
            }

            if (container.Instances != null && container.Instances.Length > 0)
            {
                foreach (var instance in container.Instances)
                {
                    if (instance.Name.HasValue() && instance.Type.HasValue())
                    {
                        Type type;
                        if (Reflector.TryGetType(instance.Type, out type))
                        {
                            var instanceInfo = new InstanceInfoBase(instance.Name, new TypeDefinitionBase(type), instance.Singleton);

                            if (instance.Parameters != null && instance.Parameters.Length > 0)
                            {
                                foreach (var constructorMethod in instanceInfo.TypeDefinition.ConstructorMethods)
                                {
                                    var parameterInfos = Match(constructorMethod, instance.Parameters);
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
                                var propertyInfos = Match(instanceInfo, instance.Properties);
                                if (propertyInfos == null)
                                {
                                    // TODO: throw
                                }

                                instanceInfo.PropertyInfos = propertyInfos;
                            }

                            _InstanceInfos = _InstanceInfos.Append(instanceInfo);
                        }
                        else
                        {
                            // TODO: throw
                        }
                    }
                }
            }

            return _InstanceInfos;
        }

        private IPropertyInfo[] Match(IInstanceInfo instanceInfo, InstancePropertyConfiguration[] properties)
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

        private IParameterInfo[] Match(IMethodInfo methodInfo, InstanceParameterConfiguration[] parameters)
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
                return new JsonFormatter().ReadObject(targetType, jsonObject);
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
                    object value;
                    if (Converter.TryBeAssignable(stringValue, targetType, out value))
                    {
                        return value;
                    }
                    else
                    {
                        throw new Exception("");
                    }
                }
            }
            else
            {
                throw new Exception("");
            }
        }
    }
}