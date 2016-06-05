using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace Petecat.Data.DataContractXml
{
    /// <summary>
    /// [Obsolete] replaced by Formatters.DataContractXmlFormatter
    /// </summary>
    public class Serializer
    {
        public static T ReadObject<T>(string path, Encoding encoding)
        {
            using (var streamReader = new StreamReader(path, encoding))
            {
                return ReadObject<T>(streamReader.ReadToEnd());
            }
        }

        public static T ReadObject<T>(string xmlString)
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
            {
                return ReadObject<T>(memoryStream);
            }
        }

        public static T ReadObject<T>(Stream stream)
        {
            var serializer = new DataContractSerializer(typeof(T));
            return (T)serializer.ReadObject(stream);
        }

        public static void WriteObject(object instance, Stream stream)
        {
            var xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Encoding = Encoding.UTF8;

            using (XmlWriter writer = XmlWriter.Create(stream, xmlWriterSettings))
            {
                var serializer = new DataContractSerializer(instance.GetType());
                serializer.WriteObject(writer, instance);
                writer.Flush();
            }
        }

        public static string WriteObject(object instance)
        {
            using (var memoryStream = new MemoryStream())
            {
                WriteObject(instance, memoryStream);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        public static void WriteObject(object instance, string path, Encoding encoding)
        {
            using (var streamWriter = new StreamWriter(path, false, encoding))
            {
                streamWriter.Write(WriteObject(instance));
            }
        }
    }
}
