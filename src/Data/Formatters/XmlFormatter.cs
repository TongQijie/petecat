using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using Petecat.Extension;

namespace Petecat.Data.Formatters
{
    public class XmlFormatter : AbstractObjectFormatter, IObjectFormatter
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

        public override object ReadObject(Type targetType, Stream stream)
        {
            return new XmlSerializer(targetType).Deserialize(stream);
        }

        public override void WriteObject(object instance, Stream stream)
        {
            new XmlSerializer(instance.GetType()).Serialize(stream, instance);
        }
    }
}
