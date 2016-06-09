using System;
using System.Runtime.Serialization;

namespace Petecat.Test.Network.Http
{
    [DataContract(Namespace = "http://wcf.gettingstarted")]
    public class GlobalSession
    {
        [DataMember]
        public string SessionId { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public DateTime? ExpiredDate { get; set; }
    }
}
