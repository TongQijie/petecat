using System;
using System.Net.Sockets;

namespace Petecat.WebServer
{
    public interface IWebSource : IDisposable
    {
        Socket CreateSocket();

        IWorker CreateWorker(Socket socket, ApplicationServer server);
    }
}
