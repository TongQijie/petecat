using Petecat.HttpServer;
using Petecat.HttpServer.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebSockets;

namespace Petecat.ServiceHost
{
    [WebSocketInjectable(ServiceName = "wstest")]
    public class WebSocketService : IWebSocketExecutionHandler
    {
        public async Task Process(AspNetWebSocketContext context)
        {
            WebSocket socket = context.WebSocket;
            while (true)
            {
                if (socket.State == WebSocketState.Open)
                {
                    Thread.Sleep(1000);
                    var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes("Time: " + DateTime.Now.ToLongTimeString()));
                    await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else
                {
                    break;
                }
            }
        }
    }
}