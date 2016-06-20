using System.Xml.Serialization;
using System.Collections.Generic;

namespace Petecat.Threading.Configuration
{
    public class BackgroundTask
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("active")]
        public bool Active { get; set; }

        [XmlAttribute("provider")]
        public string Provider { get; set; }

        [XmlArray("arguments")]
        [XmlArrayItem("arg")]
        public BackgroundTaskArgument[] Arguments { get; set; }

        public Dictionary<string, object> GetArguments()
        {
            if (Arguments == null || Arguments.Length == 0)
            {
                return null;
            }

            var arguments = new Dictionary<string, object>();
            foreach (var argument in Arguments)
            {
                arguments.Add(argument.Name, argument.Value);
            }

            return arguments;
        }
    }
}
