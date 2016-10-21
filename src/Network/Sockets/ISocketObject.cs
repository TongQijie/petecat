using System;
using System.Net;
using System.Net.Sockets;

namespace Petecat.Network.Sockets
{
    public interface ISocketObject : IDisposable
    {
        Socket InternalSocket { get; }

        event SocketReceivedDataHandlerDelegate ReceivedData;
        
        void BeginSend(byte[] data, int offset, int count);

        void Send(byte[] data, int offset, int count);

        IPAddress Address { get; }

        int Port { get; }
    }
}
