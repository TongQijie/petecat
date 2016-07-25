using System.Xml.Serialization;

namespace Petecat.Threading.Configuration
{
    [XmlRoot("tasks")]
    public class TaskContainerConfig
    {
        [XmlElement("task")]
        public TaskObjectConfig[] TaskObjects { get; set; }
    }
}
