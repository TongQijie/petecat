using System;
using System.Net;
using System.Net.Sockets;

namespace Petecat.Network.Sockets
{
    public interface ISocketObject : IDisposable
    {
        Socket InternalSocket { get; }

        event SocketReceivedDataHandlerDelegate ReceivedData;

        event SocketDisposedHandlerDelegate SocketDisposed;
        
        void BeginSend(byte[] data, int offset, int count);

        IPAddress Address { get; }

        int Port { get; }
    }
}
