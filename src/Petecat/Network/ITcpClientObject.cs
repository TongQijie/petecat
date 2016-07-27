using System.Net;

namespace Petecat.Network
{
    public interface ITcpClientObject : ISocketObject
    {
        void Connect(IPAddress address, int port);

        void Disconnect();

        void BeginReceive();

        void BeginSend(byte[] data, int offset, int count);
    }
}
