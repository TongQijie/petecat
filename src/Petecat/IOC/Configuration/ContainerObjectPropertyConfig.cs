using Petecat.Data.Formatters;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Petecat.IoC.Configuration
{
    public class ContainerObjectPropertyConfig : ContainerObjectValueElementConfig
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}
