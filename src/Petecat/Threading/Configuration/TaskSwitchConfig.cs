using Petecat.Threading.Tasks;
using System.Xml.Serialization;

namespace Petecat.Threading.Configuration
{
    public class TaskSwitchConfig
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("immediate")]
        public bool Immediate { get; set; }

        private TaskObjectOperation _Operation = TaskObjectOperation.Idle;

        [XmlAttribute("operation")]
        public TaskObjectOperation Operation
        {
            get { return _Operation; }
            set { _Operation = value; }
        }
    }
}
