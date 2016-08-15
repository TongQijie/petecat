using System.Xml.Serialization;
namespace Formatter.Configuration
{
    [XmlRoot("format")]
    public class FormatterConfig
    {
        [XmlElement("text")]
        public TextConfig TextConfig { get; set; }
    }
}
