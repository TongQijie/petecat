using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petecat.Network.Ftp;
using System.Net;
using System.Text;

namespace Petecat.Test.Network.Ftp
{
    [TestClass]
    public class FtpClientTest
    {
        [TestMethod]
        public void DownloadFile()
        {
            var request = new FtpClientRequest(FtpVerb.DownloadFile, "ftp://192.168.0.105/hey.txt");

            using (var response = request.GetResponse())
            {
                var data = response.GetString(Encoding.Default);
            }
        }

        [TestMethod]
        public void ListDirectory()
        {
            var request = new FtpClientRequest(FtpVerb.ListDirectory, "ftp://192.168.0.105");

            using (var response = request.GetResponse())
            {
                var data = response.GetString(Encoding.Default);
            }
        }

        [TestMethod]
        public void UploadFile()
        {
            var request = new FtpClientRequest(FtpVerb.UploadFile, "ftp://192.168.0.105/uploadedFile1.txt");
            using (var stream = request.Request.GetRequestStream())
            {
                var data = Encoding.UTF8.GetBytes("uploaded file.dddd");
                stream.Write(data, 0, data.Length);
            }
        }
    }
}
