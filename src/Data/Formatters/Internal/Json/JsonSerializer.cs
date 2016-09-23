using System;
using System.IO;
using System.Reflection;
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
            if (_JsonProperties == null)
            {
                Initialize();
            }

            var elementType = GetElementType(Type);
            if (elementType == JsonElementType.Collection)
            {
                stream.WriteByte(JsonEncoder.Left_Bracket);
            }
            else if (elementType == JsonElementType.Object)
            {
                stream.WriteByte(JsonEncoder.Left_Brace);
            }
            else
            {
                return;
            }

            var firstElement = true;
            if (elementType == JsonElementType.Object)
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

                    switch (GetElementType(property.PropertyInfo.PropertyType))
                    {
                        case JsonElementType.Simple:
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
            else if (elementType == JsonElementType.Collection)
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

                    switch (GetElementType(value.GetType()))
                    {
                        case JsonElementType.Simple:
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

            if (elementType == JsonElementType.Collection)
            {
                stream.WriteByte(JsonEncoder.Right_Bracket);
            }
            else if (elementType == JsonElementType.Object)
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
            var args = new Json.JsonObjectParseArgs()
            {
                Stream = stream,
            };

            Json.JsonObjectParser.Parse(args);

            return InternalDeserialize(args.InternalObject);
        }

        private object InternalDeserialize(Json.JsonObject jsonObject)
        {
            if (_JsonProperties == null)
            {
                Initialize();
            }

            if (jsonObject is Json.JsonDictionaryObject)
            {
                return DeserializeJsonDictionaryObject(jsonObject as Json.JsonDictionaryObject);
            }
            else if (jsonObject is Json.JsonCollectionObject)
            {
                return DeserializeJsonCollectionObject(jsonObject as Json.JsonCollectionObject, Type);
            }
            else if (jsonObject is Json.JsonPlainValueObject)
            {
                return DeserializeJsonPlainValueObject(jsonObject as Json.JsonPlainValueObject, Type);
            }
            else
            {
                throw new Exception("");
            }
        }

        private object DeserializeJsonDictionaryObject(Json.JsonDictionaryObject dictionaryObject)
        {
            var instance = Activator.CreateInstance(Type);

            foreach (var element in dictionaryObject.Elements)
            {
                var jsonProperty = _JsonProperties.Get(element.Key, null);
                if (jsonProperty == null)
                {
                    continue;
                }

                var elementType = GetElementType(jsonProperty.PropertyInfo.PropertyType);
                if (elementType == JsonElementType.Object)
                {
                    if (element.Value is JsonDictionaryObject)
                    {
                        var value = GetSerializer(jsonProperty.PropertyInfo.PropertyType).InternalDeserialize(element.Value);
                        jsonProperty.PropertyInfo.SetValue(instance, value);
                    }
                    else if (element.Value is JsonPlainValueObject && JsonEncoder.IsNullValue(element.Value))
                    {
                        jsonProperty.PropertyInfo.SetValue(instance, null);
                    }
                    else
                    {
                        throw new Exception("");
                    }
                }
                else if (elementType == JsonElementType.Collection)
                {
                    if (element.Value is JsonCollectionObject)
                    {
                        var collectionObject = element.Value as Json.JsonCollectionObject;
                        jsonProperty.PropertyInfo.SetValue(instance,
                            DeserializeJsonCollectionObject(collectionObject, jsonProperty.PropertyInfo.PropertyType));
                    }
                    else if (element.Value is JsonPlainValueObject && JsonEncoder.IsNullValue(element.Value))
                    {
                        jsonProperty.PropertyInfo.SetValue(instance, null);
                    }
                    else
                    {
                        throw new Exception("");
                    }
                }
                else if (elementType == JsonElementType.Simple && element.Value is Json.JsonPlainValueObject)
                {
                    var value = DeserializeJsonPlainValueObject(element.Value as Json.JsonPlainValueObject, jsonProperty.PropertyInfo.PropertyType);
                    jsonProperty.PropertyInfo.SetValue(instance, value);
                }
                else
                {
                    throw new Exception("");
                }
            }

            return instance;
        }

        private object DeserializeJsonCollectionObject(Json.JsonCollectionObject collectionObject, Type targetType)
        {
            if (targetType.IsArray)
            {
                var elementType = targetType.GetElementType();
                var array = Array.CreateInstance(elementType, collectionObject.Elements.Length);
                for (var i = 0; i < array.Length; i++)
                {
                    var jsonObject = collectionObject.Elements[i].Value;
                    if (jsonObject is Json.JsonDictionaryObject)
                    {
                        var value = GetSerializer(elementType).InternalDeserialize(jsonObject);
                        array.SetValue(value, i);
                    }
                    else if (jsonObject is Json.JsonCollectionObject)
                    {
                        // TODO: multi-dimension array
                    }
                    else if (jsonObject is Json.JsonPlainValueObject)
                    {
                        var value = DeserializeJsonPlainValueObject(jsonObject as Json.JsonPlainValueObject, elementType);
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
                    if (jsonObject is Json.JsonDictionaryObject)
                    {
                        var value = GetSerializer(elementType).InternalDeserialize(jsonObject);
                        collection.Add(value);
                    }
                    else if (jsonObject is Json.JsonCollectionObject)
                    {
                        // TODO: multi-dimension array
                    }
                    else if (jsonObject is Json.JsonPlainValueObject)
                    {
                        var value = DeserializeJsonPlainValueObject(jsonObject as Json.JsonPlainValueObject, elementType);
                        collection.Add(value);
                    }
                }

                return collection;
            }

            return null;
        }

        private object DeserializeJsonPlainValueObject(Json.JsonPlainValueObject plainValueObject, Type targetType)
        {
            return Converter.Assignable(plainValueObject.ToString(), targetType);
        }

        #endregion

        #region Json Properties

        private ThreadSafeKeyedObjectCollection<string, JsonProperty> _JsonProperties = null;

        public IEnumerable<JsonProperty> JsonProperties { get { return _JsonProperties.Values; } }

        public void Initialize()
        {
            _JsonProperties = new ThreadSafeKeyedObjectCollection<string, JsonProperty>();

            var elementType = GetElementType(Type);
            if (elementType == JsonElementType.Object)
            {
                foreach (var propertyInfo in Type.GetProperties())
                {
                    var attribute = ReflectionUtility.GetCustomAttribute<JsonPropertyAttribute>(propertyInfo);
                    _JsonProperties.Add(new JsonProperty(propertyInfo, attribute == null ? null : attribute.Alias));
                }
            }
        }

        #endregion

        #region Json Element Type

        public JsonElementType GetElementType(Type targetType)
        {
            if (typeof(ICollection).IsAssignableFrom(targetType))
            {
                return JsonElementType.Collection;
            }
            else if (targetType.IsClass && targetType != typeof(String))
            {
                return JsonElementType.Object;
            }
            else
            {
                return JsonElementType.Simple;
            }
        }

        public enum JsonElementType
        {
            Simple,

            Collection,

            Object,
        }

        #endregion
    }
}
