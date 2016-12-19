using System;

namespace Petecat.WebServer
{
    public class WebRequest
    {
        public WebRequest(IntPtr socket, RequestData requestData)
        {
            _Socket = socket;
            _RequestData = requestData;
        }

        private IntPtr _Socket;

        private RequestData _RequestData = null;
    }
}