using System;
using System.Collections.Concurrent;
using System.Net.Sockets;

using Petecat.Extending;

namespace Petecat.WebServer
{
    public class ApplicationServer : MarshalByRefObject
    {
        public ApplicationServer(IWebSource webSource)
        {
            _WebSource = webSource;
        }

        private IWebApplication[] _Applications = new IWebApplication[0];

        public IWebApplication GetWebApplication(string url)
        {
            return _Applications.FirstOrDefault(x => x.Match(url));
        }

        public void AddWebApplication(string host, int port, string virtualPath, string fullPath)
        {
            var domain = AppDomain.CreateDomain(virtualPath);

            var type = typeof(WebApplicationBase);
            var app = domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName) as IWebApplication;
            app.SetAttributes(host, port, virtualPath, fullPath);

            _Applications = _Applications.Append(app);
        }

        public bool Alive { get; private set; }

        private Socket _Socket = null;

        private IWebSource _WebSource = null;

        public void Start()
        {
            _Socket = _WebSource.CreateSocket();
            _Socket.Listen(100);

            var args = new SocketAsyncEventArgs();
            args.Completed += OnAccept;
            _Socket.AcceptAsync(args);

            Alive = true;
        }

        public void Stop()
        {
            _WebSource.Dispose();
        }

        void OnAccept(object sender, SocketAsyncEventArgs e)
        {
            var accepted = e.AcceptSocket;
            e.AcceptSocket = null;

            if (e.SocketError != SocketError.Success)
            {
                CloseSocket(accepted);
                accepted = null;
            }

            try
            {
                if (Alive)
                {
                    _Socket.AcceptAsync(e);
                }
            }
            catch (Exception)
            {
                if (accepted != null)
                {
                    CloseSocket(accepted);
                }

                // TODO: log here
                return;
            }

            if (accepted == null)
            {
                return;
            }

            SetSocketOptions(accepted);
            StartRequest(accepted, 0);
        }

        void CloseSocket(Socket socket)
        {
            if (socket == null)
            {
                return;
            }

            // attempt a quick RST of the connection
            try
            {
                socket.LingerState = new LingerOption(true, 0);
            }
            catch
            {
                // ignore
            }

            try
            {
                socket.Close();
            }
            catch
            {
                // ignore
            }
        }

        void SetSocketOptions(Socket sock)
        {
            try
            {
                sock.LingerState = new LingerOption(true, 15);
            }
            catch
            {
                // Ignore exceptions here for systems that do not support these options.
            }
        }

        void StartRequest(Socket accepted, int reuses)
        {
            try
            {
                var worker = _WebSource.CreateWorker(accepted, this);
                _Workers.TryAdd(worker.Id, worker);
                worker.Run();
            }
            catch (Exception)
            {
                try
                {
                    if (accepted != null)
                    {
                        try
                        {
                            if (accepted.Connected)
                            {
                                accepted.Shutdown(SocketShutdown.Both);
                            }
                        }
                        catch
                        {
                            // ignore
                        }

                        accepted.Close();
                    }
                }
                catch
                {
                    // ignore
                }
            }
        }

        public IWorker GetWorker(Guid id)
        {
            IWorker worker;
            if (_Workers.TryGetValue(id, out worker))
            {
                return worker;
            }

            return null;
        }

        private ConcurrentDictionary<Guid, IWorker> _Workers = new ConcurrentDictionary<Guid, IWorker>();
    }
}