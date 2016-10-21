using Petecat.Service.Client;
using Petecat.Service.Datagram;
namespace Petecat.ConsoleApp
{
    public class ServiceTcpClientObjectTest
    {
        public void GetResponse()
        {
            using (var client = new ServiceTcpClientObject("127.0.0.1", 12000))
            {
                var datagram = new ServiceTcpRequestDatagram(null, null, null, null);
                datagram.Wrap();
                var response = client.GetResponse(datagram.Bytes, 30000);
            }
        }
    }
}
