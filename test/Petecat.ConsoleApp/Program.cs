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
using Petecat.Threading.Configuration;

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

            //CacheObjectManager.Instance.AddXml<AppSettings>("AppSettings", "./configuration/AppSettings.config".FullPath(), true);

            //while (true)
            //{
            //    CommonUtility.WriteLine(new XmlFormatter().WriteString(CacheObjectManager.Instance.GetValue<AppSettings>("AppSettings")));

            //    CommonUtility.ReadAnyKey();
            //}

            AppDomainContainer.Initialize();
            var taskContainer = new TaskContainerBase();
            taskContainer.Initialize(AppDomainContainer.Instance, "./configuration/TaskObjects.config".FullPath(), "./configuration/TaskSwitchContainer.config".FullPath());

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

            CommonUtility.ReadAnyKey();

            //FolderWatcherManager.Instance.GetOrAdd("./configuration").Stop();
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