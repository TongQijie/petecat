using System.Net.Sockets;

namespace Petecat.Network
{
    public static class SocketFactory
    {
        public static ITcpClientObject CreateTcpClientObject()
        {
            return new SocketObject(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)) as ITcpClientObject;
        }

        public static ITcpListenerObject CreateTcpListenerObject()
        {
            return new SocketObject(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)) as ITcpListenerObject;
        }
    }
}
