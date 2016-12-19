using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Petecat.WebServer
{
    public class Worker : MarshalByRefObject, IWorker
    {
        public Worker(Socket socket, ApplicationServer server)
        {
            Id = Guid.NewGuid();

            _LingeringNetworkStream = new LingeringNetworkStream(socket, false);
            _Stream = _LingeringNetworkStream;

            _Socket = socket;
            _Server = server;
            _LocalIPEndPoint = (IPEndPoint)socket.LocalEndPoint;
            _RemoteIPEndPoint = (IPEndPoint)socket.RemoteEndPoint;
        }

        public Guid Id { get; private set; }

        private LingeringNetworkStream _LingeringNetworkStream = null;

        private Stream _Stream = null;

        private Socket _Socket = null;

        private ApplicationServer _Server = null;

        private IPEndPoint _LocalIPEndPoint = null;

        private IPEndPoint _RemoteIPEndPoint = null;

        private InitialWorkerRequest _InitialWorkerRequest = null;

        public void Run()
        {
            _InitialWorkerRequest = new InitialWorkerRequest(_Stream);
            byte[] buffer = InitialWorkerRequest.AllocateBuffer();
            _Stream.BeginRead(buffer, 0, buffer.Length, ReadCallback, buffer);
        }

        void ReadCallback(IAsyncResult ares)
        {
            var buffer = (byte[])ares.AsyncState;
            try
            {
                int nread = _Stream.EndRead(ares);
                // See if we got at least 1 line
                _InitialWorkerRequest.SetBuffer(buffer, nread);
                _InitialWorkerRequest.ReadRequestData();

                var requestData = _InitialWorkerRequest.RequestData;
                _InitialWorkerRequest.FreeBuffer();

                var app = _Server.GetWebApplication(requestData.Path);
                app.Server = _Server;
                app.ProcessRequest(Id, _Socket.Handle, requestData.Verb, requestData.Path, requestData.PathInfo, requestData.QueryString, requestData.Protocol, requestData.InputBuffer);
            }
            catch (Exception e)
            {
                InitialWorkerRequest.FreeBuffer(buffer);
                //HandleInitialException(e);
            }
        }

        public void Close()
        {
            if (!_LingeringNetworkStream.Connected)
            {
                _Stream.Close();
                if (_Stream != _LingeringNetworkStream)
                {
                    _LingeringNetworkStream.Close();
                }
                else if (!_LingeringNetworkStream.OwnsSocket)
                {
                    try
                    {
                        if (_Socket.Connected && !(_Socket.Poll(0, SelectMode.SelectRead) && _Socket.Available == 0))
                        {
                            _Socket.Shutdown(SocketShutdown.Both);
                        }
                    }
                    catch
                    {
                        // ignore
                    }

                    try
                    {
                        _Socket.Close();
                    }
                    catch
                    {
                        // ignore
                    }
                }

                return;
            }

            _LingeringNetworkStream.EnableLingering = false;
            _Stream.Close();
            if (_Stream != _LingeringNetworkStream)
            {
                _LingeringNetworkStream.Close();
            }

            try
            {
                _Socket.Close();
            }
            catch
            {
                // ignore
            }
            //server.ReuseSocket(sock, reuses + 1);
        }
    }
}
