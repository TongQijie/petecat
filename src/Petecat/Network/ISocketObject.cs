using System;
using System.Net;
using System.Net.Sockets;

namespace Petecat.Network
{
    public interface ISocketObject : IDisposable
    {
        Socket Socket { get; }

        event SocketReceivedDataHandlerDelegate ReceivedData;
        
        void Send(byte[] data, int offset, int count);

        IPAddress Address { get; }

        int Port { get; }
    }
}
