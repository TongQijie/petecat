using System.Xml.Serialization;
namespace Petecat.Service.Configuration
{
    public class StaticResourceContentMapping
    {
        [XmlElement("add")]
        public KeyValueConfig[] KeyValues { get; set; }
    }
}
