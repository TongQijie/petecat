//#define Tcp_Listener
#define Tcp_Client

using Petecat.Threading.Tasks;
using Petecat.Console.Outputs;
using Petecat.Console;
using Petecat.IoC;
using Petecat.Extension;
using Petecat.Utility;

using Petecat.Threading.Watcher;
using Petecat.Caching;
using System.Xml.Serialization;
using Petecat.Data.Formatters;
using System.Text;
using System;
using Petecat.Threading.Configuration;
using Petecat.Threading;
using Petecat.Network;
using System.Net;

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

            AppDomainContainer.Initialize();

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

            //CacheObjectManager.Instance.AddXml<AppSettings>("AppSettings", "./configuration/AppSettings.config".FullPath(), true);

            //while (true)
            //{
            //    CommonUtility.WriteLine(new XmlFormatter().WriteString(CacheObjectManager.Instance.GetValue<AppSettings>("AppSettings")));

            //    CommonUtility.ReadAnyKey();
            //}

            //AppDomainContainer.Initialize();
            //var taskContainer = new TaskContainerBase();
            //taskContainer.Initialize(AppDomainContainer.Instance, "./configuration/TaskObjects.config".FullPath(), "./configuration/TaskSwitchContainer.config".FullPath());

            //AppDomainContainer.Initialize("./configuration/ContainerAssemblies.config".FullPath()).Register("./configuration/TaskObjects.config".FullPath());

            //CacheObjectManager.Instance.AddXml<TaskSwitchContainerConfig>("TaskSwitchContainer", "./configuration/TaskSwitchContainer.config".FullPath(), false);

            //{
            //    var taskSwitchContainerConfig = CacheObjectManager.Instance.GetValue<TaskSwitchContainerConfig>("TaskSwitchContainer");
            //    foreach (var taskSwitchConfig in taskSwitchContainerConfig.Switches)
            //    {
            //        if (taskSwitchConfig.Immediate)
            //        {
            //            var taskObject = AppDomainContainer.Instance.Resolve<ITaskObject>(taskSwitchConfig.Name);
            //            if (taskSwitchConfig.Operation == TaskObjectOperation.Execute)
            //            {
            //                taskObject.Execute();
            //            }
            //            else if(taskSwitchConfig.Operation == TaskObjectOperation.Terminate)
            //            {
            //                taskObject.Terminate();
            //            }
            //            else if (taskSwitchConfig.Operation == TaskObjectOperation.Suspend)
            //            {
            //                taskObject.Suspend();
            //            }
            //        }
            //    }
            //}

            //FolderWatcherManager.Instance.GetOrAdd("./configuration".FullPath())
            //    .SetFileChangedHandler("TaskSwitchContainer.config", (w) =>
            //    {
            //        CacheObjectManager.Instance.Get("TaskSwitchContainer").IsDirty = true;

            //        var taskSwitchContainerConfig = CacheObjectManager.Instance.GetValue<TaskSwitchContainerConfig>("TaskSwitchContainer");
            //        foreach (var taskSwitchConfig in taskSwitchContainerConfig.Switches)
            //        {
            //            var taskObject = AppDomainContainer.Instance.Resolve<ITaskObject>(taskSwitchConfig.Name);
            //            if (taskSwitchConfig.Operation == TaskObjectOperation.Execute)
            //            {
            //                taskObject.Execute();
            //            }
            //            else if (taskSwitchConfig.Operation == TaskObjectOperation.Terminate)
            //            {
            //                taskObject.Terminate();
            //            }
            //            else if (taskSwitchConfig.Operation == TaskObjectOperation.Suspend)
            //            {
            //                taskObject.Suspend();
            //            }
            //        }
            //    }).Start();

            //using(var threadObject = new ThreadObject(() =>
            //{
            //    while (true)
            //    {
            //        Console.ConsoleBridging.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff"));

            //        ThreadBridging.Sleep(2000);
            //    }
            //}).Start())
            //{
            //    ConsoleBridging.ReadAnyKey();
            //}



#if Tcp_Listener

            var tcpListener = SocketObject.CreateTcpListenerObject();
            tcpListener.ReceivedData += tcpListener_ReceivedData;
            tcpListener.SocketConnected += tcpListener_SocketConnected;
            tcpListener.BeginListen(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10000));
            ConsoleBridging.ReadAnyKey();

#elif Tcp_Client

            //var tcpClient = SocketObject.CreateTcpClientObject();
            //tcpClient.Connect(IPAddress.Parse("127.0.0.1"), 10000);
            //tcpClient.BeginReceive();
            
            //var text = "";
            //while ((text = ConsoleBridging.ReadLine()) != string.Empty)
            //{
            //    var data = Encoding.UTF8.GetBytes(text);

            //    tcpClient.BeginSend(data, 0, data.Length);
            //}

#endif


            //FolderWatcherManager.Instance.GetOrAdd("./configuration").Stop();
        }

        static void tcpListener_SocketConnected(ISocketObject socketObject)
        {
            ConsoleBridging.WriteLine("Connected: " + socketObject.Address.ToString() + ":" + socketObject.Port.ToString());
        }

        static void tcpListener_ReceivedData(ISocketObject socketObject, byte[] data, int offset, int count)
        {
            ConsoleBridging.WriteLine("Received: " + Encoding.UTF8.GetString(data, 0, count));
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