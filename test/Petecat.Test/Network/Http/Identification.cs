using System.Runtime.Serialization;

namespace Petecat.Test.Network.Http
{
    [DataContract(Namespace = "http://wcf.gettingstarted")]
    public class Identification
    {
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}
