using Petecat.Threading.Tasks;
using Petecat.Console.Outputs;
using Petecat.Console;
using Petecat.IOC;
using Petecat.Extension;
using Petecat.Utility;

using Petecat.Threading.Watcher;
using Petecat.Caching;
using System.Xml.Serialization;
using Petecat.Data.Formatters;
using System.Text;
using System;

namespace Petecat.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //var container = new DefaultContainer();
            //container.Register("./configuration/container.config");

            //var apple = container.Resolve("apple");
            //var banana = container.Resolve("banana");
            //var another = container.Resolve("another-apple");

            //AppDomainContainer.Initialize(AppConfigUtility.GetAppConfig("containerAssemblies", string.Empty).FullPath());

            //FolderWatcherManager.Instance.GetOrAdd("./configuration".FullPath())
            //    .SetFileChangedHandler("container.config", (w) =>
            //    {
            //        CommonUtility.WriteLine(w.FullPath + " changed.");
            //    })
            //    .SetFileChangedHandler("databases.config", (w) => 
            //    {
            //        CommonUtility.WriteLine(w.FullPath + " changed.");
            //    }).Start("*.config");

            //CacheObjectManager.Instance.Add("AppSettings", () =>
            //{
            //    return new XmlFormatter().ReadObject<AppSettings>("./configuration/AppSettings.config".FullPath(), Encoding.UTF8);
            //});

            //FolderWatcherManager.Instance.GetOrAdd("./configuration".FullPath())
            //    .SetFileChangedHandler("AppSettings.config", (w) =>
            //    {
            //        CacheObjectManager.Instance.Get("AppSettings").IsDirty = true;
            //    }).Start();

            CacheObjectManager.Instance.AddXml<AppSettings>("AppSettings", "./configuration/AppSettings.config".FullPath(), true);

            while (true)
            {
                CommonUtility.WriteLine(new XmlFormatter().WriteString(CacheObjectManager.Instance.GetValue<AppSettings>("AppSettings")));

                CommonUtility.ReadAnyKey();
            }

            CommonUtility.ReadAnyKey();

            FolderWatcherManager.Instance.GetOrAdd("./configuration").Stop();
        }
    }

    [XmlRoot("appSettings")]
    public class AppSettings
    {
        [XmlElement("enableHttps")]
        public bool EnableHttps { get; set; }

        [XmlElement("httpsConfig")]
        public HttpsConfig HttpsConfig { get; set; }
    }

    public class HttpsConfig
    {
        [XmlAttribute("url")]
        public string Url { get; set; }

        [XmlAttribute("port")]
        public int Port { get; set; }
    }
}