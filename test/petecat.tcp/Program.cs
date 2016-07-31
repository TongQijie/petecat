using System.Net;

using Petecat.Console;
using Petecat.Network;
using System.Text;

namespace petecat.tcp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (ConsoleBridging.ReadLine().StartsWith("s", System.StringComparison.OrdinalIgnoreCase))
            {
                var listener = SocketFactory.CreateTcpListenerObject();
                listener.ReceivedData += Listener_ReceivedData;
                listener.SocketConnected += Listener_SocketConnected;
                listener.SocketDisconnected += Listener_SocketDisconnected;
                listener.Listen(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10000));
                ConsoleBridging.ReadAnyKey();
            }
            else
            {
                var client = SocketFactory.CreateTcpClientObject();
                client.ReceivedData += Listener_ReceivedData;
                client.Connect(IPAddress.Parse("127.0.0.1"), 10000);

                var text = "";
                while ((text = ConsoleBridging.ReadLine()) != string.Empty)
                {
                    var data = Encoding.UTF8.GetBytes(text);
                    client.Send(data, 0, data.Length);
                }

                client.Disconnect();

                // just for branch test
            }
        }

        private static void Listener_SocketDisconnected(ISocketObject socketObject)
        {
            ConsoleBridging.WriteLine("disconnected {0}:{1}", socketObject.Address, socketObject.Port);
        }

        private static void Listener_SocketConnected(ISocketObject socketObject)
        {
            ConsoleBridging.WriteLine("connected {0}:{1}", socketObject.Address, socketObject.Port);
        }

        private static void Listener_ReceivedData(ISocketObject socketObject, byte[] data, int offset, int count)
        {
            ConsoleBridging.WriteLine("received from {0}:{1}\n{2}", socketObject.Address, socketObject.Port, Encoding.UTF8.GetString(data, offset, count));
        }
    }
}
