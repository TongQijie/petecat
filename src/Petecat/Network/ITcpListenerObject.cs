using System.Net;

namespace Petecat.Network
{
    public interface ITcpListenerObject : ISocketObject
    {
        void Listen(IPEndPoint hostEndPoint);

        event SocketConnectedHandlerDelegate SocketConnected;

        event SocketDisconnectedHandlerDelegate SocketDisconnected;
    }
}
