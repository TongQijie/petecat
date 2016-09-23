using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Runtime.Serialization;

using Petecat.Extension;

namespace Petecat.Data.Formatters
{
    public class DataContractXmlFormatter : AbstractObjectFormatter, IObjectFormatter
    {
        public override object ReadObject(Type targetType, Stream stream)
        {
            return new DataContractSerializer(targetType).ReadObject(stream);
        }

        public override void WriteObject(object instance, Stream stream)
        {
            var xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Encoding = Encoding.UTF8;

            using (XmlWriter writer = XmlWriter.Create(stream, xmlWriterSettings))
            {
                var serializer = new DataContractSerializer(instance.GetType());
                serializer.WriteObject(writer, instance);
            }
        }
    }
}
