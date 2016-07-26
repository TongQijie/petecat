using System.Xml.Serialization;

namespace Petecat.Threading.Tasks
{
    public enum TaskObjectOperation
    {
        [XmlEnum("idle")]
        Idle,

        [XmlEnum("execute")]
        Execute,

        [XmlEnum("suspend")]
        Suspend,

        [XmlEnum("terminate")]
        Terminate,
    }
}
