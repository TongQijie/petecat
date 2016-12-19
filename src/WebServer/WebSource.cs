using System;
using System.Net;
using System.Net.Sockets;

namespace Petecat.WebServer
{
    public class WebSource : IWebSource
    {
        public WebSource(IPAddress address, int port)
        {
            _BindAddress = new IPEndPoint(address, port);
        }

        private IPEndPoint _BindAddress = null;

        public Socket CreateSocket()
        {
            var socket = new Socket(_BindAddress.AddressFamily, SocketType.Stream, ProtocolType.IP);
            socket.Bind(_BindAddress);
            return socket;
        }

        public IWorker CreateWorker(Socket socket, ApplicationServer server)
        {
            return new Worker(socket, server);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
