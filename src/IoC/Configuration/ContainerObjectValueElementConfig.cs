using Petecat.Data.Formatters;

using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

namespace Petecat.IoC.Configuration
{
    public class ContainerObjectValueElementConfig
    {
        [XmlText]
        public string StringValue { get; set; }

        [XmlAnyElement]
        public XmlElement ElementValue { get; set; }

        public bool IsReferenceObject
        {
            get
            {
                return Regex.IsMatch((StringValue ?? "").Trim(), @"^\x24\x7B\S+\x7D$");
            }
        }

        public bool IsDirectObject
        {
            get
            {
                return ElementValue != null && ElementValue.Name == "object";
            }
        }

        public bool IsValueCollection
        {
            get
            {
                return ElementValue != null && ElementValue.Name == "list";
            }
        }

        public bool IsObjectValue
        {
            get
            {
                return IsDirectObject || IsReferenceObject;
            }
        }

        public string ObjectName
        {
            get
            {
                if (IsReferenceObject)
                {
                    return StringValue.Trim().Substring(2, StringValue.Trim().Length - 3);
                }
                else if (IsDirectObject)
                {
                    var objectValue = new XmlFormatter().ReadObject<ContainerObjectConfig>(ElementValue.OuterXml);
                    return objectValue.Name;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
