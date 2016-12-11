using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Concurrent;

using Petecat.Utility;
using Petecat.Extending;
using Petecat.Formatter.Attribute;

namespace Petecat.Formatter.Json
{
    internal class JsonSerializer
    {
        public JsonSerializer(Type type)
        {
            Type = type;
            Initialize();
        }

        public Type Type { get; private set; }

        private ConcurrentDictionary<string, JsonProperty> _JsonProperties = null;

        private ConcurrentDictionary<string, JsonProperty> JsonProperties
        {
            get { return _JsonProperties ?? (_JsonProperties = new ConcurrentDictionary<string, JsonProperty>()); }
        }

        private object _InitLock = new object();

        private bool _IsInitialized = false;

        private void Initialize()
        {
            if (!_IsInitialized)
            {
                lock (_InitLock)
                {
                    if (!_IsInitialized)
                    {
                        var runtimeType = GetRuntimeType(Type);
                        if (runtimeType == RuntimeType.Object)
                        {
                            foreach (var propertyInfo in Type.GetProperties().Where(x => x.CanRead && x.CanWrite 
                                && !Reflector.ContainsCustomAttribute<JsonIngoreAttribute>(x)))
                            {
                                JsonProperty jsonProperty = null;

                                var attribute = Reflector.GetCustomAttribute<JsonPropertyAttribute>(propertyInfo);
                                if (attribute == null)
                                {
                                    jsonProperty = new JsonProperty(propertyInfo, null, false);
                                }
                                else if (attribute is JsonObjectAttribute)
                                {
                                    jsonProperty = new JsonProperty(propertyInfo, attribute.Alias, true); 
                                }
                                else
                                {
                                    jsonProperty = new JsonProperty(propertyInfo, attribute.Alias, false);
                                }
                                JsonProperties.TryAdd(jsonProperty.Key, jsonProperty);
                            }
                        }

                        _IsInitialized = true;
                    }
                }
            }
        }

        #region Serializer Cache

        private static ConcurrentDictionary<Type, JsonSerializer> _CachedSerializers = null;

        private static ConcurrentDictionary<Type, JsonSerializer> CachedSerializers
        {
            get { return _CachedSerializers ?? (_CachedSerializers = new ConcurrentDictionary<Type, JsonSerializer>()); }
        }

        public static JsonSerializer GetSerializer(Type targetType)
        {
            if (!CachedSerializers.ContainsKey(targetType))
            {
                CachedSerializers.TryAdd(targetType, new JsonSerializer(targetType));
            }

            JsonSerializer serializer;
            if (CachedSerializers.TryGetValue(targetType, out serializer))
            {
                return serializer;
            }

            return null;
        }

        #endregion

        #region Serialization

        public void Serialize(object instance, Stream stream, bool omitDefaultValueProperty)
        {
            if (CurrentRuntimeType == RuntimeType.Collection)
            {
                stream.WriteByte(JsonEncoder.Left_Bracket);
            }
            else if (CurrentRuntimeType == RuntimeType.Object)
            {
                stream.WriteByte(JsonEncoder.Left_Brace);
            }
            else
            {
                return;
            }

            var firstElement = true;
            if (CurrentRuntimeType == RuntimeType.Object)
            {
                foreach (var jsonProperty in JsonProperties.Values.ToArray())
                {
                    var propertyValue = jsonProperty.PropertyInfo.GetValue(instance, null);
                    if (omitDefaultValueProperty && jsonProperty.DefaultValue.EqualsWith(propertyValue))
                    {
                        continue;
                    }

                    if (firstElement)
                    {
                        firstElement = false;
                    }
                    else
                    {
                        stream.WriteByte(JsonEncoder.Comma);
                    }

                    var buf = JsonEncoder.GetElementName(jsonProperty.Key);
                    stream.Write(buf, 0, buf.Length);

                    switch (GetRuntimeType(jsonProperty.PropertyInfo.PropertyType))
                    {
                        case RuntimeType.Value:
                            {
                                buf = JsonEncoder.GetPlainValue(propertyValue);
                                stream.Write(buf, 0, buf.Length);
                                break;
                            }
                        default:
                            {
                                if (propertyValue == null)
                                {
                                    buf = JsonEncoder.GetNullValue();
                                    stream.Write(buf, 0, buf.Length);
                                }
                                else
                                {
                                    GetSerializer(jsonProperty.PropertyInfo.PropertyType).Serialize(propertyValue, stream, omitDefaultValueProperty);
                                }
                                break;
                            }
                    }
                }
            }
            else if (CurrentRuntimeType == RuntimeType.Collection)
            {
                var array = instance as ICollection;

                foreach (var value in array)
                {
                    if (firstElement)
                    {
                        firstElement = false;
                    }
                    else
                    {
                        stream.WriteByte(JsonEncoder.Comma);
                    }

                    switch (GetRuntimeType(value.GetType()))
                    {
                        case RuntimeType.Value:
                            {
                                var buf = JsonEncoder.GetPlainValue(value);
                                stream.Write(buf, 0, buf.Length);
                                break;
                            }
                        default:
                            {
                                if (value == null)
                                {
                                    var buf = JsonEncoder.GetNullValue();
                                    stream.Write(buf, 0, buf.Length);
                                }
                                else
                                {
                                    GetSerializer(value.GetType()).Serialize(value, stream, omitDefaultValueProperty);
                                }
                                break;
                            }
                    }
                }
            }
            else
            {
                return;
            }

            if (CurrentRuntimeType == RuntimeType.Collection)
            {
                stream.WriteByte(JsonEncoder.Right_Bracket);
            }
            else if (CurrentRuntimeType == RuntimeType.Object)
            {
                stream.WriteByte(JsonEncoder.Right_Brace);
            }
            else
            {
                return;
            }
        }

        #endregion

        #region Deserialization

        public object Deserialize(Stream stream)
        {
            var args = new JsonObjectParseArgs()
            {
                Stream = new Internal.BufferedStream(stream, 4 * 1024),
            };

            JsonObjectParser.Parse(args);

            return InternalDeserialize(args.InternalObject);
        }

        public object Deserialize(JsonObject jsonObject)
        {
            return InternalDeserialize(jsonObject);
        }

        private object InternalDeserialize(JsonObject jsonObject)
        {
            if (jsonObject is JsonDictionaryObject)
            {
                return DeserializeJsonDictionaryObject(jsonObject as JsonDictionaryObject);
            }
            else if (jsonObject is JsonCollectionObject)
            {
                return DeserializeJsonCollectionObject(jsonObject as JsonCollectionObject, Type);
            }
            else if (jsonObject is JsonValueObject)
            {
                return DeserializeJsonPlainValueObject(jsonObject as JsonValueObject, Type);
            }

            return null;
        }

        private object DeserializeJsonDictionaryObject(JsonDictionaryObject dictionaryObject)
        {
            var instance = Activator.CreateInstance(Type);

            foreach (var element in dictionaryObject.Elements)
            {
                JsonProperty jsonProperty;
                JsonProperties.TryGetValue(element.Key, out jsonProperty);
                if (jsonProperty == null)
                {
                    continue;
                }

                if (jsonProperty.IsJsonObject)
                {
                    jsonProperty.PropertyInfo.SetValue(instance, element.Value, null);
                    continue;
                }

                var runtimeType = GetRuntimeType(jsonProperty.PropertyInfo.PropertyType);
                if (runtimeType == RuntimeType.Object)
                {
                    if (element.Value is JsonDictionaryObject)
                    {
                        var value = GetSerializer(jsonProperty.PropertyInfo.PropertyType).InternalDeserialize(element.Value);
                        jsonProperty.PropertyInfo.SetValue(instance, value, null);
                    }
                    else if (element.Value is JsonValueObject && JsonEncoder.IsNullValue(element.Value))
                    {
                        jsonProperty.PropertyInfo.SetValue(instance, null, null);
                    }
                    else
                    {
                        throw new Errors.JsonSerializeFailedException(element.Key, ".net runtime type does not match json type.");
                    }
                }
                else if (runtimeType == RuntimeType.Collection)
                {
                    if (element.Value is JsonCollectionObject)
                    {
                        var collectionObject = element.Value as JsonCollectionObject;
                        jsonProperty.PropertyInfo.SetValue(instance,
                            DeserializeJsonCollectionObject(collectionObject, jsonProperty.PropertyInfo.PropertyType), null);
                    }
                    else if (element.Value is JsonValueObject && JsonEncoder.IsNullValue(element.Value))
                    {
                        jsonProperty.PropertyInfo.SetValue(instance, null, null);
                    }
                    else
                    {
                        throw new Errors.JsonSerializeFailedException(element.Key, ".net runtime type does not match json type.");
                    }
                }
                else if (runtimeType == RuntimeType.Value && element.Value is JsonValueObject)
                {
                    var value = DeserializeJsonPlainValueObject(element.Value as JsonValueObject, jsonProperty.PropertyInfo.PropertyType);
                    jsonProperty.PropertyInfo.SetValue(instance, value, null);
                }
                else
                {
                    throw new Errors.JsonSerializeFailedException(element.Key, ".net runtime type does not match json type.");
                }
            }

            return instance;
        }

        private object DeserializeJsonCollectionObject(JsonCollectionObject collectionObject, Type targetType)
        {
            if (targetType.IsArray)
            {
                var elementType = targetType.GetElementType();
                var array = Array.CreateInstance(elementType, collectionObject.Elements.Length);
                for (var i = 0; i < array.Length; i++)
                {
                    var jsonObject = collectionObject.Elements[i].Value;
                    if (jsonObject is JsonDictionaryObject)
                    {
                        var value = GetSerializer(elementType).InternalDeserialize(jsonObject);
                        array.SetValue(value, i);
                    }
                    else if (jsonObject is JsonCollectionObject)
                    {
                        // TODO: multi-dimension array
                    }
                    else if (jsonObject is JsonValueObject)
                    {
                        var value = DeserializeJsonPlainValueObject(jsonObject as JsonValueObject, elementType);
                        array.SetValue(value, i);
                    }
                }

                return array;
            }
            else if (typeof(IList).IsAssignableFrom(targetType))
            {
                var elementType = targetType.GetGenericArguments().FirstOrDefault() ?? typeof(object);
                var collection = Activator.CreateInstance(targetType) as IList;
                for (int i = 0; i < collectionObject.Elements.Length; i++)
                {
                    var jsonObject = collectionObject.Elements[i].Value;
                    if (jsonObject is JsonDictionaryObject)
                    {
                        var value = GetSerializer(elementType).InternalDeserialize(jsonObject);
                        collection.Add(value);
                    }
                    else if (jsonObject is JsonCollectionObject)
                    {
                        // TODO: multi-dimension array
                    }
                    else if (jsonObject is JsonValueObject)
                    {
                        var value = DeserializeJsonPlainValueObject(jsonObject as JsonValueObject, elementType);
                        collection.Add(value);
                    }
                }

                return collection;
            }

            return null;
        }

        private object DeserializeJsonPlainValueObject(JsonValueObject plainValueObject, Type targetType)
        {
            if (JsonEncoder.IsNullValue(plainValueObject))
            {
                return null;
            }
            else
            {
                return plainValueObject.ToString().ConvertTo(targetType);
            }
        }

        #endregion

        #region Runtime Type

        private RuntimeType _CurrentRuntimeType = RuntimeType.Unknown;

        public RuntimeType CurrentRuntimeType
        {
            get
            {
                if (_CurrentRuntimeType == RuntimeType.Unknown)
                {
                    _CurrentRuntimeType = GetRuntimeType(Type);
                }

                return _CurrentRuntimeType;
            }
        }

        public RuntimeType GetRuntimeType(Type targetType)
        {
            if (typeof(ICollection).IsAssignableFrom(targetType))
            {
                return RuntimeType.Collection;
            }
            else if (targetType.IsClass && targetType != typeof(String))
            {
                return RuntimeType.Object;
            }
            else
            {
                return RuntimeType.Value;
            }
        }

        public enum RuntimeType
        {
            Unknown,

            Value,

            Collection,

            Object,
        }

        #endregion
    }
}