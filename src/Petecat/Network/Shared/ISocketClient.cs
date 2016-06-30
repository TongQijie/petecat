using System.Net;
namespace Petecat.Network.Shared
{
    public interface ISocketClient
    {
        byte[] Buffer { get; }

        IDatagramFinder DatagramFinder { get; }

        void Connect(string host, int port);

        void Connect(IPAddress address, int port);

        void Disconnect();

        void Send(IDatagram datagram);

        void Receive();
    }
}