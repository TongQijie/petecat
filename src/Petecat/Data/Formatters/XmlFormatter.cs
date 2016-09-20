using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Petecat.Data.Formatters
{
    public class XmlFormatter : IObjectFormatter
    {
        public XmlFormatter()
        {
        }

        public XmlFormatter(XmlSerializerNamespaces namespaces)
        {
            Namespaces = namespaces;
        }

        private static XmlSerializerNamespaces _EmptyXmlSerializerNamespaces = null;

        public static XmlSerializerNamespaces EmptyXmlSerializerNamespaces
        {
            get
            {
                if (_EmptyXmlSerializerNamespaces == null)
                {
                    _EmptyXmlSerializerNamespaces = new XmlSerializerNamespaces();
                    _EmptyXmlSerializerNamespaces.Add("", "");
                }
                return _EmptyXmlSerializerNamespaces;
            }
        }

        public XmlSerializerNamespaces Namespaces { get; private set; }

        public object ReadObject(Type targetType, Stream stream)
        {
            return new XmlSerializer(targetType).Deserialize(stream);
        }

        public object ReadObject(Type targetType, string stringValue)
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(stringValue)))
            {
                return ReadObject(targetType, memoryStream);
            }
        }

        public object ReadObject(Type targetType, string path, Encoding encoding)
        {
            using (var streamReader = new StreamReader(path, encoding))
            {
                return ReadObject(targetType, streamReader.ReadToEnd());
            }
        }

        public object ReadObject(Type targetType, byte[] byteValues, int offset, int count)
        {
            return ReadObject(targetType, Encoding.UTF8.GetString(byteValues, offset, count));
        }

        public T ReadObject<T>(Stream stream)
        {
            return (T)ReadObject(typeof(T), stream);
        }

        public T ReadObject<T>(string stringValue)
        {
            return (T)ReadObject(typeof(T), stringValue);
        }

        public T ReadObject<T>(string path, Encoding encoding)
        {
            return (T)ReadObject(typeof(T), path, encoding);
        }

        public T ReadObject<T>(byte[] byteValues, int offset, int count)
        {
            return (T)ReadObject(typeof(T), Encoding.UTF8.GetString(byteValues, offset, count));
        }

        /// <summary>
        /// [Obsolete] replaced by WriteString(object instance);
        /// </summary>
        public string WriteObject(object instance)
        {
            using (var memoryStream = new MemoryStream())
            {
                WriteObject(instance, memoryStream);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        public void WriteObject(object instance, Stream stream)
        {
            new XmlSerializer(instance.GetType()).Serialize(stream, instance);
        }

        public void WriteObject(object instance, string path, Encoding encoding)
        {
            using (var streamWriter = new StreamWriter(path, false, encoding))
            {
                streamWriter.Write(WriteObject(instance));
            }
        }

        public string WriteString(object instance)
        {
            return Encoding.UTF8.GetString(WriteBytes(instance));
        }

        public byte[] WriteBytes(object instance)
        {
            using (var memoryStream = new MemoryStream())
            {
                WriteObject(instance, memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
