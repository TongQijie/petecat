using System.Net;

namespace Petecat.Network
{
    public delegate void SocketReceivedDataHandlerDelegate(ISocketObject socketObject, byte[] data, int offset, int count);
}
