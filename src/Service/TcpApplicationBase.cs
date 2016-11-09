using System;
using Petecat.IoC;
using Petecat.Console;
using Petecat.Logging;
using Petecat.Extension;
using Petecat.Network.Sockets;
using Petecat.DependencyInjection;

namespace Petecat.Service
{
    public class TcpApplicationBase
    {
        private ITcpListenerObject _TcpListenerObject = null;

        public TcpApplicationBase()
        {
            Initialize();
        }

        public void Start(int port)
        {
            if (_TcpListenerObject == null)
            {
                _TcpListenerObject = SocketFactory.CreateTcpListenerObject();
                _TcpListenerObject.SocketConnected += _TcpListenerObject_SocketConnected;
                _TcpListenerObject.SocketDisconnected += _TcpListenerObject_SocketDisconnected;
                _TcpListenerObject.ReceivedData += _TcpListenerObject_ReceivedData;
                _TcpListenerObject.Listen(port);
            }
        }

        public void Stop()
        {
            if (_TcpListenerObject != null)
            {
                _TcpListenerObject.SocketConnected -= _TcpListenerObject_SocketConnected;
                _TcpListenerObject.SocketDisconnected -= _TcpListenerObject_SocketDisconnected;
                _TcpListenerObject.ReceivedData -= _TcpListenerObject_ReceivedData;
                _TcpListenerObject.Dispose();
                _TcpListenerObject = null;

                Connections = new TcpConnectionBase[0];
            }
        }

        private void _TcpListenerObject_SocketConnected(ISocketObject socketObject)
        {
            var connection = Connections.FirstOrDefault(x => x.IsDisposed);
            if (connection != null)
            {
                connection.Reset(socketObject);
            }
            else
            {
                Connections = Connections.Append(new TcpConnectionBase(socketObject));
            }
        }

        private void _TcpListenerObject_SocketDisconnected(ISocketObject socketObject)
        {
            var connection = Connections.FirstOrDefault(x => x.SocketObject != null && x.SocketObject.Equals(socketObject));
            if (connection != null)
            {
                connection.IsDisposed = true;
            }
        }

        private void _TcpListenerObject_ReceivedData(ISocketObject socketObject, byte[] data, int offset, int count)
        {
            var connection = Connections.FirstOrDefault(x => !x.IsDisposed && x.SocketObject != null && x.SocketObject.Equals(socketObject));
            if (connection != null)
            {
                connection.ReceiveData(data, offset, count);
            }
            else
            {
                ConsoleBridging.WriteLine("connection not found.");
            }
        }

        private void connection_ServiceRequestArrival(ServiceTcpRequest request)
        {
            var response = new ServiceTcpResponse(request.Connection);

            try
            {
                response.Status = (byte)ServiceTcpResponseStatus.Succeeded;
                response.ContentType = request.ContentType;
                response.Flush(ServiceManager.Instance.InvokeTcp(request));
            }
            catch (Exception e)
            {
                response.Status = (byte)ServiceTcpResponseStatus.Failed;
                response.ContentType = null;
                response.Flush(e.Message);
            }
        }

        private TcpConnectionBase[] Connections = new TcpConnectionBase[100];

        private void Initialize()
        {
            try
            {
                AppDomainIoCContainer.Initialize();
                ServiceManager.Instance = new ServiceManager(AppDomainIoCContainer.Instance);
            }
            catch (Exception e)
            {
                DependencyInjector.GetObject<IFileLogger>().LogEvent("ServiceHttpApplication", Severity.Fatal, e);
                return;
            }

            for (int i = 0; i < Connections.Length; i++)
            {
                Connections[i] = new TcpConnectionBase() { IsDisposed = true };
                Connections[i].ServiceRequestArrival += connection_ServiceRequestArrival;
            }
        }
    }
}
