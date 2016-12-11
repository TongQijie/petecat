using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using System.IO;

using Petecat.Network.Http;
using System.Text.RegularExpressions;

namespace Crawler
{
    class Program
    {
        static Program()
        {
        }

        static List<Playlist> Playlists = new List<Playlist>();

        public static void Main(string[] args)
        {
            for (int i = 1; i <= 100; i++)
            {
                Console.WriteLine("Getting page {0}... {1}/100", i, i);

                GetOnePage(i);
            }

            Console.WriteLine("Waiting to finish ...");

            using (var outputStream = new StreamWriter("playlists.html", false, Encoding.UTF8))
            {
                outputStream.WriteLine("<ul>");
                foreach (var playlist in Playlists.OrderByDescending(x => x.PlayCount))
                {
                    outputStream.WriteLine("<li><a href=\"{0}\">{1}</a>。播放次数:{2}</li>", playlist.Url, playlist.Title, playlist.PlayCount);
                }
                outputStream.WriteLine("</ul>");
            }

            Console.WriteLine("done");

            Console.ReadKey();
        }

        static void GetOnePage(int pageNumber)
        {
            var queryString = new Dictionary<string, string>();
            queryString.Add("order", "hot");
            queryString.Add("cat", "全部");
            queryString.Add("limit", "35");
            queryString.Add("offset", ((pageNumber - 1) * 35).ToString());

            var request = new HttpClientRequest(HttpVerb.GET, "http://music.163.com/discover/playlist", queryString);

            var html = "";
            using (var response = request.GetResponse())
            {
                html = response.GetString(Encoding.UTF8);
            }

            if (string.IsNullOrWhiteSpace(html))
            {
                return;
            }

            var document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);

            var playlistNodes = document.DocumentNode.SelectNodes("//ul[@id='m-pl-container']/li");
            if (playlistNodes == null || playlistNodes.Count == 0)
            {
                return;
            }

            foreach (var playlistNode in playlistNodes)
            {
                var anchor = playlistNode.SelectSingleNode("./div[1]/a[@class='msk']");
                var span = playlistNode.SelectSingleNode("./div[1]/div[1]/span[@class='nb']");

                if (anchor != null && span != null)
                {
                    Playlists.Add(new Playlist()
                    {
                        Title = anchor.Attributes["title"].Value,
                        Href = anchor.Attributes["href"].Value,
                        PlayTimes = span.InnerText,
                    });
                }
            }
        }

        class Playlist
        {
            public string Title { get; set; }

            public string Href { get; set; }

            public string PlayTimes { get; set; }

            public int PlayCount
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(PlayTimes))
                    {
                        return 0;
                    }

                    var matchedValue = new Regex(@"^\d*", RegexOptions.IgnoreCase).Match(PlayTimes);
                    if (string.IsNullOrWhiteSpace(matchedValue.Value))
                    {
                        return 0;
                    }

                    if (PlayTimes.Contains("万"))
                    {
                        return Convert.ToInt32(matchedValue.Value) * 10000;
                    }
                    else 
                    {
                        return Convert.ToInt32(matchedValue.Value);
                    }
                }
            }

            public string Url
            {
                get
                {
                    if (string.IsNullOrEmpty(Href))
                    {
                        return string.Empty;
                    }
                    else 
                    {
                        return string.Format("http://music.163.com{0}", Href);
                    }
                }
            }
        }
    }
}
