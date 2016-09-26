using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using Petecat.Utility;
using Petecat.Extension;
using Petecat.Collection;
using Petecat.Data.Attributes;

namespace Petecat.Data.Formatters.Internal.Json
{
    internal class JsonSerializer : IKeyedObject<string>
    {
        public JsonSerializer(Type type)
        {
            Type = type;
            Initialize();
        }

        public string Key { get { return Type.FullName; } }

        public Type Type { get; private set; }

        #region Serializer Cache

        private static ThreadSafeKeyedObjectCollection<string, JsonSerializer> _Serializers = null;

        private static ThreadSafeKeyedObjectCollection<string, JsonSerializer> Serializers
        {
            get { return _Serializers ?? (_Serializers = new ThreadSafeKeyedObjectCollection<string, JsonSerializer>()); }
        }

        public static JsonSerializer GetSerializer(Type targetType)
        {
            if (!Serializers.ContainsKey(targetType.FullName))
            {
                Serializers.Add(new JsonSerializer(targetType));
            }

            return _Serializers.Get(targetType.FullName, null);
        }

        #endregion

        #region Serialization

        public void Serialize(object instance, Stream stream, bool omitDefaultValueProperty)
        {
            if (CurrentDotNetRuntimeType == DotNetRuntimeType.Collection)
            {
                stream.WriteByte(JsonEncoder.Left_Bracket);
            }
            else if (CurrentDotNetRuntimeType == DotNetRuntimeType.Object)
            {
                stream.WriteByte(JsonEncoder.Left_Brace);
            }
            else
            {
                return;
            }

            var firstElement = true;
            if (CurrentDotNetRuntimeType == DotNetRuntimeType.Object)
            {
                foreach (var property in JsonProperties)
                {
                    var propertyValue = property.PropertyInfo.GetValue(instance, null);
                    if (omitDefaultValueProperty && Comparator.Equal(property.DefaultValue, propertyValue))
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

                    var buf = JsonEncoder.GetElementName(property.Key);
                    stream.Write(buf, 0, buf.Length);

                    switch (GetDotNetRuntimeType(property.PropertyInfo.PropertyType))
                    {
                        case DotNetRuntimeType.Simple:
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
                                    GetSerializer(property.PropertyInfo.PropertyType).Serialize(propertyValue, stream, omitDefaultValueProperty);
                                }
                                break;
                            }
                    }
                }
            }
            else if (CurrentDotNetRuntimeType == DotNetRuntimeType.Collection)
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

                    switch (GetDotNetRuntimeType(value.GetType()))
                    {
                        case DotNetRuntimeType.Simple:
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

            if (CurrentDotNetRuntimeType == DotNetRuntimeType.Collection)
            {
                stream.WriteByte(JsonEncoder.Right_Bracket);
            }
            else if (CurrentDotNetRuntimeType == DotNetRuntimeType.Object)
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
                Stream = new BufferStream(stream, 4 * 1024 ),
            };

            JsonObjectParser.Parse(args, true);

            return InternalDeserialize(args.InternalObject);
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
            else if (jsonObject is JsonPlainValueObject)
            {
                return DeserializeJsonPlainValueObject(jsonObject as JsonPlainValueObject, Type);
            }

            return null;
        }

        private object DeserializeJsonDictionaryObject(JsonDictionaryObject dictionaryObject)
        {
            var instance = Activator.CreateInstance(Type);

            foreach (var element in dictionaryObject.Elements)
            {
                var jsonProperty = _JsonProperties.Get(element.Key, null);
                if (jsonProperty == null)
                {
                    continue;
                }

                var dotNetRuntimeType = GetDotNetRuntimeType(jsonProperty.PropertyInfo.PropertyType);
                if (dotNetRuntimeType == DotNetRuntimeType.Object)
                {
                    if (element.Value is JsonDictionaryObject)
                    {
                        var value = GetSerializer(jsonProperty.PropertyInfo.PropertyType).InternalDeserialize(element.Value);
                        jsonProperty.PropertyInfo.SetValue(instance, value, null);
                    }
                    else if (element.Value is JsonPlainValueObject && JsonEncoder.IsNullValue(element.Value))
                    {
                        jsonProperty.PropertyInfo.SetValue(instance, null, null);
                    }
                    else
                    {
                        throw new Errors.JsonSerializeFailedException(element.Key, ".net runtime type does not match json type.");
                    }
                }
                else if (dotNetRuntimeType == DotNetRuntimeType.Collection)
                {
                    if (element.Value is JsonCollectionObject)
                    {
                        var collectionObject = element.Value as JsonCollectionObject;
                        jsonProperty.PropertyInfo.SetValue(instance,
                            DeserializeJsonCollectionObject(collectionObject, jsonProperty.PropertyInfo.PropertyType), null);
                    }
                    else if (element.Value is JsonPlainValueObject && JsonEncoder.IsNullValue(element.Value))
                    {
                        jsonProperty.PropertyInfo.SetValue(instance, null, null);
                    }
                    else
                    {
                        throw new Errors.JsonSerializeFailedException(element.Key, ".net runtime type does not match json type.");
                    }
                }
                else if (dotNetRuntimeType == DotNetRuntimeType.Simple && element.Value is JsonPlainValueObject)
                {
                    var value = DeserializeJsonPlainValueObject(element.Value as JsonPlainValueObject, jsonProperty.PropertyInfo.PropertyType);
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
                    else if (jsonObject is JsonPlainValueObject)
                    {
                        var value = DeserializeJsonPlainValueObject(jsonObject as JsonPlainValueObject, elementType);
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
                    else if (jsonObject is JsonPlainValueObject)
                    {
                        var value = DeserializeJsonPlainValueObject(jsonObject as JsonPlainValueObject, elementType);
                        collection.Add(value);
                    }
                }

                return collection;
            }

            return null;
        }

        private object DeserializeJsonPlainValueObject(JsonPlainValueObject plainValueObject, Type targetType)
        {
            if (JsonEncoder.IsNullValue(plainValueObject))
            {
                return null;
            }
            else
            {
                return Converter.Assignable(plainValueObject.ToString(), targetType);
            }
        }

        #endregion

        #region Json Properties

        private ThreadSafeKeyedObjectCollection<string, JsonProperty> _JsonProperties = null;

        public IEnumerable<JsonProperty> JsonProperties { get { return _JsonProperties.Values; } }

        private object _SyncLocker = new object();

        public void Initialize()
        {
            lock (_SyncLocker)
            {
                _JsonProperties = new ThreadSafeKeyedObjectCollection<string, JsonProperty>();

                var dotNetRuntimeType = GetDotNetRuntimeType(Type);
                if (dotNetRuntimeType == DotNetRuntimeType.Object)
                {
                    foreach (var propertyInfo in Type.GetProperties())
                    {
                        var attribute = ReflectionUtility.GetCustomAttribute<JsonPropertyAttribute>(propertyInfo);
                        _JsonProperties.Add(new JsonProperty(propertyInfo, attribute == null ? null : attribute.Alias));
                    }
                }
            }
        }

        #endregion

        #region .Net Type

        private DotNetRuntimeType _CurrentDotNetRuntimeType = DotNetRuntimeType.Unknown;

        public DotNetRuntimeType CurrentDotNetRuntimeType
        {
            get
            {
                if (_CurrentDotNetRuntimeType == DotNetRuntimeType.Unknown)
                {
                    _CurrentDotNetRuntimeType = GetDotNetRuntimeType(Type);
                }

                return _CurrentDotNetRuntimeType;
            }
        }

        public DotNetRuntimeType GetDotNetRuntimeType(Type targetType)
        {
            if (typeof(ICollection).IsAssignableFrom(targetType))
            {
                return DotNetRuntimeType.Collection;
            }
            else if (targetType.IsClass && targetType != typeof(String))
            {
                return DotNetRuntimeType.Object;
            }
            else
            {
                return DotNetRuntimeType.Simple;
            }
        }

        public enum DotNetRuntimeType
        {
            Unknown,

            Simple,

            Collection,

            Object,
        }

        #endregion
    }
}
