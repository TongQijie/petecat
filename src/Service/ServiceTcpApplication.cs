using Petecat.IoC;
using Petecat.Logging;
using Petecat.Extension;
using Petecat.Logging.Loggers;
using Petecat.Network.Sockets;

using System;

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
            _TcpListenerObject.SocketDisposed += _TcpListenerObject_SocketDisposed;
            _TcpListenerObject.Listen(port);
        }

        private void _TcpListenerObject_SocketConnected(ISocketObject socketObject)
        {
            var connection = new ServiceTcpConnection(64 * 1024, socketObject as ITcpClientObject);
            connection.ServiceRequestArrival += connection_ServiceRequestArrival;
            Connections = Connections.Append(connection);
        }

        private void _TcpListenerObject_SocketDisposed(ISocketObject socketObject)
        {
            Connections = Connections.Remove(x => x.TcpClientObject.Equals(socketObject));
        }

        private void connection_ServiceRequestArrival(ServiceTcpRequest request)
        {
            var response = new ServiceTcpResponse(request.Connection);

            try
            {
                response.Status = 0x66;
                response.ContentType = request.ContentType;
                ServiceManager.Instance.InvokeTcp(request, response);
            }
            catch (Exception e)
            {
                response.Status = 0x99;
                response.ContentType = null;
                response.WriteObject(e.Message);
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
