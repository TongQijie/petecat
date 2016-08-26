using System.Xml.Serialization;
namespace Petecat.Network.Configuration
{
    [XmlRoot("templates")]
    public class MailTemplateCollectionConfig
    {
        [XmlElement("template")]
        public MailTemplateConfig[] Templates { get; set; }
    }
}
