using System.Xml.Serialization;
namespace Petecat.Network.Configuration
{
    public class MailTemplateConfig
    {
        [XmlElement("id")]
        public string TemplateId { get; set; }

        [XmlElement("server")]
        public string Server { get; set; }

        [XmlElement("port")]
        public int Port { get; set; }

        [XmlElement("from")]
        public string From { get; set; }

        [XmlElement("displayName")]
        public string DisplayName { get; set; }

        [XmlElement("to")]
        public string To { get; set; }

        [XmlElement("cc")]
        public string CC { get; set; }

        [XmlElement("subject")]
        public string Subject { get; set; }

        [XmlElement("body")]
        public string Body { get; set; }
    }
}
