#if !MONO

using System.Web.WebSockets;
using System.Threading.Tasks;

namespace Petecat.HttpServer
{
    public interface IWebSocketExecutionHandler
    {
        Task Process(AspNetWebSocketContext context);
    }
}

#endif