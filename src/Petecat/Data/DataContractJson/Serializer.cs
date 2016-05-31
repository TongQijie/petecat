using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;

namespace Petecat.Data.DataContractJson
{
    public class Serializer
    {
        public static T ReadObject<T>(string path, Encoding encoding)
        {
            using (var streamReader = new StreamReader(path, encoding))
            {
                return ReadObject<T>(streamReader.ReadToEnd());
            }
        }

        public static T ReadObject<T>(string jsonString)
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                return ReadObject<T>(memoryStream);
            }
        }

        public static T ReadObject<T>(Stream stream)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            return (T)serializer.ReadObject(stream);
        }

        public static void WriteObject(object instance, Stream stream)
        {
            using (var writer = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8))
            {
                var serializer = new DataContractJsonSerializer(instance.GetType());
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