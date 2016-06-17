using System.Xml.Serialization;

namespace Petecat.Threading.Configuration
{
    [XmlRoot("tasks")]
    public class BackgroundTaskCollection
    {
        [XmlElement("task")]
        public BackgroundTask[] BackgroundTasks { get; set; }
    }
}