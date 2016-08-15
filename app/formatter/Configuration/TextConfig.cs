using System.Xml.Serialization;

namespace Formatter.Configuration
{
    public class TextConfig
    {
        [XmlAttribute("replaceTab")]
        public bool ReplaceTabWithSpaces { get; set; }

        [XmlAttribute("spaces")]
        public int SpacesForTab { get; set; }

        [XmlAttribute("containsBOM")]
        public bool ContainsBOM { get; set; }

        [XmlAttribute("newline")]
        public string NewLineStyle { get; set; }
    }
}