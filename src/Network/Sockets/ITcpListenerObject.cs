using System.Net;

namespace Petecat.Network.Sockets
{
    public interface ITcpListenerObject : ISocketObject
    {
        void Listen(IPEndPoint hostEndPoint);

        event SocketConnectedHandlerDelegate SocketConnected;

        event SocketDisconnectedHandlerDelegate SocketDisconnected;
    }
}
