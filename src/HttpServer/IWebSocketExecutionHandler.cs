using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Web.WebSockets;
namespace Petecat.HttpServer
{
    public interface IWebSocketExecutionHandler
    {
        Task Process(AspNetWebSocketContext context);
    }
}
