using Petecat.DependencyInjection;
using Petecat.DependencyInjection.Containers;
using System;
using System.IO;

namespace Petecat.WebServer
{
    public class WebApplicationBase : MarshalByRefObject, IWebApplication
    {
        public void SetAttributes(string host, int port, string virtualPath, string fullPath)
        {
            _Host = host;
            _Port = port;
            _VirtualPath = virtualPath;
            _Path = fullPath;
        }

        private string _Host = null;

        private int _Port = 0;

        public AppDomain Domain { get { return AppDomain.CurrentDomain; } }

        private string _Path = null;

        public string Path { get { return _Path; } }

        public ApplicationServer Server { get; set; }

        private string _VirtualPath = null;

        public string VirtualPath { get { return _VirtualPath; } }

        public bool IsHttpHandler(string verb, string uri)
        {
            throw new NotImplementedException();
        }

        public bool IsStarted { get; private set; }

        public void ProcessRequest(Guid id, IntPtr socket, string verb, string path, string pathInfo, string queryString, string protocol, byte[] buffer)
        {
            //Directory.SetCurrentDirectory(Path);

            //if (!IsStarted)
            //{
            //    DependencyInjector.Setup(new BaseDirectoryAssemblyContainer())
            //        .RegisterAssemblies<AssemblyInfoBase>();
            //    IsStarted = true;
            //}

            var requestData = new RequestData(verb, path, queryString, protocol);
            requestData.InputBuffer = buffer;
            requestData.PathInfo = pathInfo;

            var context = new WebContext(new WebRequest(socket, requestData), new WebResponse(socket, requestData));

            //var handler = DependencyInjector.GetObject<IWebHandlerFactory>().GetHandler(context);
            //if (handler != null)
            //{
            //    handler.ProcessRequest(context);
            //}

            context.Response.Send();

            Server.GetWorker(id).Close();
        }

        public void Unload()
        {
            throw new NotImplementedException();
        }

        public bool Match(string url)
        {
            return url.StartsWith(VirtualPath);
        }
    }
}