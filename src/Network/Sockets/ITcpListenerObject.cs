using System.Net;

namespace Petecat.Network.Sockets
{
    public interface ITcpListenerObject : ISocketObject
    {
        void Listen(IPEndPoint hostEndPoint);

        void Listen(int port);

        event SocketConnectedHandlerDelegate SocketConnected;

        event SocketDisconnectedHandlerDelegate SocketDisconnected;
    }
}
