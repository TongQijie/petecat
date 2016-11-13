using Petecat.Network.Sockets;
using System.Text;

namespace Petecat.ConsoleApp.Network.Socket
{
    public class TcpListenerObjectTest
    {
        public void Run()
        {
            var listener = SocketFactory.CreateTcpListenerObject();
            listener.ReceivedData += Listener_ReceivedData;
            listener.Listen(18081);

            Console.ConsoleBridging.ReadAnyKey();
        }

        private void Listener_ReceivedData(ISocketObject socketObject, byte[] data, int offset, int count)
        {
            Console.ConsoleBridging.WriteLine(Encoding.UTF8.GetString(data, offset, count));
        }
    }
}
