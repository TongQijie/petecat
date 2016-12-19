using System;
using System.Net.Sockets;

namespace Petecat.WebServer
{
    public class LingeringNetworkStream : NetworkStream
    {
        const int USECONDS_TO_LINGER = 2000000;
        const int MAX_USECONDS_TO_LINGER = 30000000;
        // We dont actually use the data from this buffer. So we cache it...
        static byte[] buffer;

        public LingeringNetworkStream(Socket sock, bool owns) : base(sock, owns)
        {
            EnableLingering = true;
            OwnsSocket = owns;
        }

        public bool OwnsSocket { get; private set; }

        public bool EnableLingering { get; set; }

        void LingeringClose()
        {
            int waited = 0;

            if (!Connected)
                return;

            try
            {
                Socket.Shutdown(SocketShutdown.Send);
                DateTime start = DateTime.UtcNow;
                while (waited < MAX_USECONDS_TO_LINGER)
                {
                    int nread = 0;
                    try
                    {
                        if (!Socket.Poll(USECONDS_TO_LINGER, SelectMode.SelectRead))
                            break;

                        if (buffer == null)
                            buffer = new byte[512];

                        nread = Socket.Receive(buffer, 0, buffer.Length, 0);
                    }
                    catch { }

                    if (nread == 0)
                        break;

                    waited += (int)(DateTime.UtcNow - start).TotalMilliseconds * 1000;
                }
            }
            catch
            {
                // ignore - we don't care, we're closing anyway
            }
        }

        public override void Close()
        {
            if (EnableLingering)
            {
                try
                {
                    LingeringClose();
                }
                finally
                {
                    base.Close();
                }
            }
            else
                base.Close();
        }

        public bool Connected
        {
            get { return Socket.Connected; }
        }
    }
}
