using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace Petecat.WebServer
{
    public class WebResponse
    {
        public WebResponse(IntPtr socket, RequestData requestData)
        {
            _Socket = socket;
            _RequestData = requestData;
        }

        private IntPtr _Socket;

        private RequestData _RequestData = null;

        public int StatusCode { get; set; }

        public string StatusDescription { get; set; }

        private byte[] GetHeaders()
        {
            var basicHeaders = new StringBuilder();
            basicHeaders.AppendLine(_RequestData.Protocol + ' ' + StatusCode + ' ' + StatusDescription);
            basicHeaders.AppendLine("Date: " + DateTime.UtcNow.ToString("r", CultureInfo.InvariantCulture));
            basicHeaders.AppendLine("Server: Petecat Windows");
            basicHeaders.AppendLine("Connection: close");
            basicHeaders.AppendLine();
            return Encoding.UTF8.GetBytes(basicHeaders.ToString());
        }

        public void Send()
        {
            StatusCode = 200;
            StatusDescription = "ok";

            var d = GetHeaders();
            var n = send(_Socket, d, d.Length, 0);
        }

        [DllImport("Ws2_32.dll")]
        private static extern int send(IntPtr socket, byte[] buf, int len, int flags);
    }
}