using Petecat.IoC;
using Petecat.Logging;
using Petecat.Extension;
using Petecat.Logging.Loggers;
using Petecat.Network.Sockets;

using System;
using Petecat.Console;

namespace Petecat.Service
{
    public class ServiceTcpApplication
    {
        private ITcpListenerObject _TcpListenerObject = null;

        public void Start(int port)
        {
            Initialize();

            _TcpListenerObject = SocketFactory.CreateTcpListenerObject();
            _TcpListenerObject.SocketConnected += _TcpListenerObject_SocketConnected;
            _TcpListenerObject.SocketDisconnected += _TcpListenerObject_SocketDisconnected;
            _TcpListenerObject.ReceivedData += _TcpListenerObject_ReceivedData;
            _TcpListenerObject.Listen(port);
        }

        private void _TcpListenerObject_SocketDisconnected(ISocketObject socketObject)
        {
            Connections = Connections.Remove(x => x.SocketObject.Equals(socketObject));

            foreach (var con in Connections)
            {
                ConsoleBridging.WriteLine("{0}:{1}", con.SocketObject.Address.ToString(), con.SocketObject.Port);
            }
        }

        private void _TcpListenerObject_ReceivedData(ISocketObject socketObject, byte[] data, int offset, int count)
        {
            var connection = Connections.FirstOrDefault(x => x.SocketObject.Equals(socketObject));
            if (connection != null)
            {
                connection.ReceiveData(data, offset, count);
            }
        }

        private void _TcpListenerObject_SocketConnected(ISocketObject socketObject)
        {
            var connection = new ServiceTcpConnection(socketObject as ITcpClientObject);
            connection.ServiceRequestArrival += connection_ServiceRequestArrival;
            Connections = Connections.Append(connection);

            foreach (var con in Connections)
            {
                ConsoleBridging.WriteLine("{0}:{1}", con.SocketObject.Address.ToString(), con.SocketObject.Port);
            }
        }

        private void connection_ServiceRequestArrival(ServiceTcpRequest request)
        {
            ConsoleBridging.WriteLine("Get data from {0}:{1}", request.Connection.SocketObject.Address.ToString(), request.Connection.SocketObject.Port);

            var response = new ServiceTcpResponse(request.Connection);

            try
            {
                response.Status = 0x66;
                response.ContentType = request.ContentType;
                response.Flush(ServiceManager.Instance.InvokeTcp(request));
            }
            catch (Exception e)
            {
                response.Status = 0x99;
                response.ContentType = null;
                response.Flush(e.Message);
            }
        }

        private ServiceTcpConnection[] Connections = new ServiceTcpConnection[0];

        private void Initialize()
        {
            LoggerManager.SetLogger(new FileLogger(LoggerManager.AppDomainLoggerName, "./log".FullPath()));

            try
            {
                AppDomainIoCContainer.Initialize();
                ServiceManager.Instance = new ServiceManager(AppDomainIoCContainer.Instance);
            }
            catch (Exception e)
            {
                LoggerManager.GetLogger().LogEvent("ServiceHttpApplication", LoggerLevel.Fatal, e);
                return;
            }
        }
    }
}
