using System.Net;

namespace Petecat.Network
{
    public interface ITcpListenerObject : ISocketObject
    {
        void BeginListen(IPEndPoint hostEndPoint);

        event SocketConnectedHandlerDelegate SocketConnected;
    }
}
