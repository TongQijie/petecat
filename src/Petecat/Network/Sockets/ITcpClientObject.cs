using System.Net;

namespace Petecat.Network.Sockets
{
    public interface ITcpClientObject : ISocketObject
    {
        void Connect(IPAddress address, int port);

        void Disconnect();
    }
}
