using System.Xml.Serialization;

namespace Petecat.Threading.Configuration
{
    [XmlRoot("switches")]
    public class TaskSwitchContainerConfig
    {
        [XmlElement("switch")]
        public TaskSwitchConfig[] Switches { get; set; }
    }
}
