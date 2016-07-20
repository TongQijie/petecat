using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Petecat.IOC.Configuration
{
    public class ContainerPropertyConfig
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlText]
        public string StringValue { get; set; }

        public bool IsObjectValue
        {
            get
            {
                return Regex.IsMatch((StringValue ?? "").Trim(), @"^\x24\x7B\S+\x7D$");
            }
        }

        public string ObjectName
        {
            get
            {
                if (IsObjectValue)
                {
                    return StringValue.Trim().Substring(2, StringValue.Trim().Length - 3);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
