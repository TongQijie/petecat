using System.Net;

namespace Petecat.Network.Sockets
{
    public delegate void SocketReceivedDataHandlerDelegate(ISocketObject socketObject, byte[] data, int offset, int count);
}
