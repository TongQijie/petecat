using Petecat.WebServer;
using System;
using System.Net;

namespace Petecat.ConsoleApp.WebServer
{
    public class ApplicationServerTest
    {
        public void Run()
        {
            var server = new ApplicationServer(new WebSource(IPAddress.Parse("192.168.0.106"), 8080));
            server.Start();

            server.AddWebApplication("example.com", 8080, "/", "");

            Console.ReadKey();
        }
    }
}
