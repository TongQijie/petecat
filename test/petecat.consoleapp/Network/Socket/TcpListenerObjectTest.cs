using Petecat.Network.Sockets;
using System.Text;
using System;

namespace Petecat.ConsoleApp.Network.Socket
{
    public class TcpListenerObjectTest
    {
        public void Run()
        {
            var listener = SocketFactory.CreateTcpListenerObject();
            listener.ReceivedData += Listener_ReceivedData;
            listener.Listen(18081);

            Console.ReadKey();
        }

        private void Listener_ReceivedData(ISocketObject socketObject, byte[] data, int offset, int count)
        {
            Console.WriteLine(Encoding.UTF8.GetString(data, offset, count));
        }
    }
}
