using System.Text;

namespace Petecat.WebServer
{
    public class RequestData
    {
        public string Verb { get; set; }

        public string Path { get; set; }

        public string PathInfo { get; set; }

        public string QueryString { get; set; }

        public string Protocol { get; set; }

        public byte[] InputBuffer { get; set; }

        public RequestData(string verb, string path, string queryString, string protocol)
        {
            Verb = verb;
            Path = path;
            QueryString = queryString;
            Protocol = protocol;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Verb: {0}\n", Verb);
            sb.AppendFormat("Path: {0}\n", Path);
            sb.AppendFormat("PathInfo: {0}\n", PathInfo);
            sb.AppendFormat("QueryString: {0}\n", QueryString);
            return sb.ToString();
        }
    }
}
