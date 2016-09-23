using System.Net;

namespace Petecat.Network.Ftp
{
    public class FtpClientRequest
    {
        private FtpClientRequest(FtpVerb ftpVerb)
        {
            FtpVerb = ftpVerb;
        }

        public FtpClientRequest(FtpVerb ftpVerb, string uri)
            : this(ftpVerb)
        {
            Request = WebRequest.Create(uri) as FtpWebRequest;

            if (FtpVerb == FtpVerb.DownloadFile)
            {
                Request.Method = "RETR";
            }
            else if (FtpVerb == FtpVerb.UploadFile)
            {
                Request.Method = "STOR";
            }
            else if (FtpVerb == FtpVerb.ListDirectory)
            {
                Request.Method = "NLST";
            }
        }

        public FtpWebRequest Request { get; private set; }

        public FtpVerb FtpVerb { get; private set; }

        public FtpClientResponse GetResponse()
        {
            try
            {
                return new FtpClientResponse(Request.GetResponse() as FtpWebResponse);
            }
            catch (WebException e)
            {
                return new FtpClientResponse(e.Response as FtpWebResponse);
            }
        }
    }
}
