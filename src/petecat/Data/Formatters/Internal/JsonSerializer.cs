using Petecat.Collection;
using Petecat.Extension;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
namespace Petecat.Data.Formatters.Internal
{
    internal class JsonSerializer : IKeyedObject<string>
    {
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

        public JsonSerializer(Type type)
        {
            Type = type;
        }

        public string Key { get { return Type.FullName; } }

        public Type Type { get; private set; }

        #region Serialization

        public void Serialize(object instance, Stream stream)
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
                                buf = JsonEncoder.GetSimpleValue(property.PropertyInfo.GetValue(instance, null));
                                stream.Write(buf, 0, buf.Length);
                                break;
                            }
                        default:
                            {
                                var value = property.PropertyInfo.GetValue(instance, null);
                                if (value == null)
                                {
                                    buf = JsonEncoder.GetNullValue();
                                    stream.Write(buf, 0, buf.Length);
                                }
                                else
                                {
                                    GetSerializer(property.PropertyInfo.PropertyType).Serialize(value, stream);
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
                                var buf = JsonEncoder.GetSimpleValue(value);
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
                                    GetSerializer(value.GetType()).Serialize(value, stream);
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
            if (_JsonProperties == null)
            {
                Initialize();
            }

            var elementType = GetElementType(Type);
            if (elementType == JsonElementType.Object)
            {
                var instance = Activator.CreateInstance(Type);

                if (!Seek(stream, JsonEncoder.Left_Brace))
                {
                    throw new Exception("");
                }

                var firstElement = true;
                int b;
                // {
                while ((b = stream.ReadByte()) != -1 && b != JsonEncoder.Right_Brace)
                {
                    if (firstElement)
                    {
                        if (b == JsonEncoder.Double_Quotes)
                        {
                            var name = JsonEncoder.GetElementName(stream).Trim();
                            if (!name.HasValue())
                            {
                                throw new Exception("");
                            }

                            if (!Seek(stream, JsonEncoder.Colon))
                            {
                                throw new Exception("");
                            }

                            if (!_JsonProperties.ContainsKey(name))
                            {

                            }
                        }
                    }
                }

                if (b == -1)
                {
                    throw new Exception("");
                }

                // }
                return instance;
            }

            return null;
        }

        private bool Seek(Stream stream, byte target)
        {
            int b;
            while ((b = stream.ReadByte()) != -1 && b != target)
            {
            }

            if (b == -1)
            {
                return false;
            }

            return true;
        }

        private int Seek(Stream stream, byte[] targets)
        {
            int b = -1;
            while ((b = stream.ReadByte()) != -1 && !targets.Exists(x => x == b))
            {
            }
            return b;
        }

        class JsonObject
        {
            public static JsonObject Parse(Stream stream, JsonObject container = null)
            {
                JsonObject current = null;

                int b;
                while ((b = stream.ReadByte()) != -1)
                {
                    if (b == JsonEncoder.Left_Brace)
                    {
                        // object
                        current = new JsonDictionaryObject().ReadValue(stream);
                    }
                    else if (b == JsonEncoder.Left_Bracket)
                    {
                        // collection
                        current = new JsonCollectionObject().ReadValue(stream);
                    }
                    else if (IsNonEmptyChar(b))
                    {
                        // value
                        current = new JsonValueObject()
                        {
                            IsStringValue = b == JsonEncoder.Double_Quotes,
                            Buffer = b == JsonEncoder.Double_Quotes ? new byte[0] : new byte[1] { (byte)b },
                            Container = container,
                        }.ReadValue(stream);
                    }
                }

                return current;
            }

            public static bool IsNonEmptyChar(int b)
            {
                return b > 0x20 && b <= 0x7E;
            }

            public static bool Seek(Stream stream, byte target)
            {
                int b;
                while ((b = stream.ReadByte()) != -1 && b != target)
                {
                }

                if (b == -1)
                {
                    return false;
                }

                return true;
            }
        }

        class JsonValueObject : JsonObject
        {
            public object Value { get; set; }

            public bool IsStringValue { get; set; }

            public JsonObject Container { get; set; }

            public byte[] Buffer { get; set; }

            public JsonObject ReadValue(Stream stream)
            {
                if (IsStringValue)
                {
                    int before = -1, after = -1;
                    while ((after = stream.ReadByte()) != -1)
                    {
                        if (after == JsonEncoder.Double_Quotes && before != JsonEncoder.Backslash)
                        {
                            break;
                        }
                        else if (after == JsonEncoder.Double_Quotes && before == JsonEncoder.Backslash)
                        {
                            Buffer[Buffer.Length - 1] = JsonEncoder.Double_Quotes;
                        }
                        else
                        {
                            Buffer = Buffer.Append((byte)after);
                        }

                        before = after;
                    }
                }
                else
                {
                    byte[] endBytes;
                    if (Container is JsonDictionaryObject)
                    {
                        endBytes = new byte[] { JsonEncoder.Comma, JsonEncoder.Right_Brace };
                    }
                    else if (Container is JsonCollectionObject)
                    {
                        endBytes = new byte[] { JsonEncoder.Comma, JsonEncoder.Right_Bracket };
                    }
                    else
                    {
                        endBytes = new byte[0];
                    }

                    int b;
                    while ((b = stream.ReadByte()) != -1 && !endBytes.Exists(x => x == b))
                    {
                        Buffer = Buffer.Append((byte)b);
                    }
                }

                return this;
            }
        }

        class JsonDictionaryObject : JsonObject
        {
            public JsonDictionaryElement[] Elements { get; set; }

            public JsonObject ReadValue(Stream stream)
            {
                Elements = new JsonDictionaryElement[0];

                string elementName = null;
                int b;
                while ((b = stream.ReadByte()) != -1 && b != JsonEncoder.Right_Brace)
                {
                    if (b == JsonEncoder.Double_Quotes)
                    {
                        if (elementName == null)
                        {
                            // start to read element's name
                            elementName = JsonEncoder.GetElementName(stream);
                        }
                        else
                        {
                            // start to read element's value
                            Seek(stream, JsonEncoder.Colon);
                            Elements = Elements.Append(new JsonDictionaryElement()
                            {
                                Key = elementName,
                                Value = Parse(stream, this),
                            });

                            elementName = null;
                        }
                    }
                }

                return this;
            }
        }

        class JsonCollectionObject : JsonObject
        {
            public JsonCollectionElement[] Elements { get; set; }

            public JsonObject ReadValue(Stream stream)
            {
                Elements = new JsonCollectionElement[0];

                int b;
                while ((b = stream.ReadByte()) != -1 && b != JsonEncoder.Right_Bracket)
                {
                    if (IsNonEmptyChar(b) && b != JsonEncoder.Comma)
                    {
                        Elements = Elements.Append(new JsonCollectionElement()
                        {
                            Value = Parse(stream, this),
                        });
                    }
                }

                return this;
            }
        }

        class JsonDictionaryElement
        {
            public string Key { get; set; }

            public JsonObject Value { get; set; }
        }

        class JsonCollectionElement
        {
            public JsonObject Value { get; set; } 
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
                    _JsonProperties.Add(new JsonProperty(propertyInfo, null));
                }
            }
        }

        public class JsonProperty : IKeyedObject<string>
        {
            public JsonProperty(PropertyInfo propertyInfo, string alias)
            {
                PropertyInfo = propertyInfo;
                Alias = alias;
            }

            public string Key { get { return Alias.HasValue() ? Alias : PropertyInfo.Name; } }

            public PropertyInfo PropertyInfo { get; private set; }

            public string Alias { get; set; }
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
