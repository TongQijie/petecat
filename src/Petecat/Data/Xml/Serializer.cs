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

        public static T ReadObject<T>(string xml)
        {
            using (var sr = new StringReader(xml))
            {
                return (T)(new XmlSerializer(typeof(T)).Deserialize(sr));
            }
        }

        public static T ReadObject<T>(string path, Encoding encoding)
        {
            using (var sr = new StreamReader(path, encoding))
            {
                return (T)(new XmlSerializer(typeof(T)).Deserialize(sr));
            }
        }

        public static string WriteObject(object instance)
        {
            using (var sw = new StringWriter())
            {
                new XmlSerializer(instance.GetType()).Serialize(sw, instance, DefaultXmlSerializerNamespaces);
                return sw.ToString();
            }
        }

        public static void WriteObject(object instance, string path, Encoding encoding)
        {
            using (var sw = new StreamWriter(path, false, encoding))
            {
                new XmlSerializer(instance.GetType()).Serialize(sw, instance, DefaultXmlSerializerNamespaces);
            }
        }
    }
}
