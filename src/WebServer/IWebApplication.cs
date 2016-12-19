using System;
using System.Net.Sockets;

namespace Petecat.WebServer
{
    public interface IWebApplication
    {
        string Path { get; }

        string VirtualPath { get; }

        AppDomain Domain { get; }

        ApplicationServer Server { get; set; }

        void Unload();

        bool IsHttpHandler(string verb, string uri);

        void ProcessRequest(Guid id, IntPtr socket, string verb, string path, string pathInfo, string queryString, string protocol, byte[] buffer);

        bool Match(string url);

        void SetAttributes(string host, int port, string virtualPath, string fullPath);
    }
}