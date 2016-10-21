﻿using Petecat.IoC;
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

        public ServiceTcpApplication()
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

                Connections = new ServiceTcpConnection[0];
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
                Connections = Connections.Append(new ServiceTcpConnection(socketObject));
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

                ConsoleBridging.WriteLine("execute succeeded.");
            }
            catch (Exception e)
            {
                response.Status = (byte)ServiceTcpResponseStatus.Failed;
                response.ContentType = null;
                response.Flush(e.Message);

                ConsoleBridging.WriteLine("execute failed.");
            }
        }

        private ServiceTcpConnection[] Connections = new ServiceTcpConnection[100];

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

            for (int i = 0; i < Connections.Length; i++)
            {
                Connections[i] = new ServiceTcpConnection() { IsDisposed = true };
                Connections[i].ServiceRequestArrival += connection_ServiceRequestArrival;
            }
        }
    }
}