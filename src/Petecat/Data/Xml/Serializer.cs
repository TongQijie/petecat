using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Petecat.Data.Xml
{
    public class Serializer
    {
        public static XmlSerializerNamespaces DefaultXmlSerializerNamespaces { get; private set; }

        static Serializer()
        {
            DefaultXmlSerializerNamespaces = new XmlSerializerNamespaces();
            DefaultXmlSerializerNamespaces.Add("", "");
        }

        public static T ReadObject<T>(string xmlString)
        {
            using (var streamReader = new StringReader(xmlString))
            {
                return (T)(new XmlSerializer(typeof(T)).Deserialize(streamReader));
            }
        }

        public static T ReadObject<T>(string path, Encoding encoding)
        {
            using (var streamReader = new StreamReader(path, encoding))
            {
                return (T)(new XmlSerializer(typeof(T)).Deserialize(streamReader));
            }
        }

        public static string WriteObject(object instance)
        {
            using (var streamWriter = new StringWriter())
            {
                new XmlSerializer(instance.GetType()).Serialize(streamWriter, instance, DefaultXmlSerializerNamespaces);
                return streamWriter.ToString();
            }
        }

        public static void WriteObject(object instance, string path, Encoding encoding)
        {
            using (var streamWriter = new StreamWriter(path, false, encoding))
            {
                new XmlSerializer(instance.GetType()).Serialize(streamWriter, instance, DefaultXmlSerializerNamespaces);
            }
        }
    }
}
