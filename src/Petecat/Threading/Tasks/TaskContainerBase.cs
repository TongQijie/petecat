using Petecat.Caching;
using Petecat.Collection;
using Petecat.Data.Formatters;
using Petecat.IOC;
using Petecat.Threading.Watcher;
using System.IO;
using System.Text;

namespace Petecat.Threading.Tasks
{
    public class TaskContainerBase : ITaskContainer
    {
        private ThreadSafeKeyedObjectCollection<string, ITaskObject> _TaskObjects = new ThreadSafeKeyedObjectCollection<string, ITaskObject>();

        private IContainer _Container = null;

        public void Initialize(IContainer container, string taskObjectConfigFile, string taskSwitchConfigFile)
        {
            _Container = container;
            _Container.RegisterContainerObjects(taskObjectConfigFile);

            CacheObjectManager.Instance.AddXml<Configuration.TaskSwitchContainerConfig>("TaskSwitchContainer", taskSwitchConfigFile, false);

            foreach (var taskSwitchConfig in CacheObjectManager.Instance.GetValue<Configuration.TaskSwitchContainerConfig>("TaskSwitchContainer").Switches)
            {
                if (taskSwitchConfig.Immediate)
                {
                    Operate(taskSwitchConfig);
                }
            }

            var fileInfo = new FileInfo(taskSwitchConfigFile);

            FolderWatcherManager.Instance.GetOrAdd(fileInfo.Directory.FullName)
                .SetFileChangedHandler(fileInfo.Name, (w) =>
                {
                    CacheObjectManager.Instance.Get("TaskSwitchContainer").IsDirty = true;

                    foreach (var taskSwitchConfig in CacheObjectManager.Instance.GetValue<Configuration.TaskSwitchContainerConfig>("TaskSwitchContainer").Switches)
                    {
                        Operate(taskSwitchConfig);
                    }
                }).Start();
        }

        public ITaskObject GetOrAdd(ITaskObject taskObject)
        {
            if (_TaskObjects.ContainsKey(taskObject.Name))
            {
                return _TaskObjects.Get(taskObject.Name, null);
            }
            else
            {
                return _TaskObjects.Add(taskObject);
            }
        }

        public void StartAll()
        {
            foreach (var taskObject in _TaskObjects.Values)
            {
                if (taskObject.CanExecute)
                {
                    taskObject.Execute();
                }
            }
        }

        public void StopAll()
        {
            foreach (var taskObject in _TaskObjects.Values)
            {
                if (taskObject.CanExecute)
                {
                    taskObject.Terminate();
                }
            }
        }

        public void Start(string name)
        {
            var taskObject = _TaskObjects.Get(name, null);
            if (taskObject != null && taskObject.CanExecute)
            {
                taskObject.Execute();
            }
        }

        public void Suspend(string name)
        {
            var taskObject = _TaskObjects.Get(name, null);
            if (taskObject != null && taskObject.CanSuspend)
            {
                taskObject.Suspend();
            }
        }

        public void Stop(string name)
        {
            var taskObject = _TaskObjects.Get(name, null);
            if (taskObject != null && taskObject.CanTerminate)
            {
                taskObject.Terminate();
            }
        }

        public void Resume(string name)
        {
            var taskObject = _TaskObjects.Get(name, null);
            if (taskObject != null && taskObject.CanResume)
            {
                taskObject.Execute();
            }
        }

        private void Operate(Configuration.TaskSwitchConfig taskSwitchConfig)
        {
            var taskObject = GetOrAdd(AppDomainContainer.Instance.Resolve<ITaskObject>(taskSwitchConfig.Name));
            if (taskSwitchConfig.Operation == TaskObjectOperation.Execute)
            {
                taskObject.Execute();
            }
            else if (taskSwitchConfig.Operation == TaskObjectOperation.Terminate)
            {
                taskObject.Terminate();
            }
            else if (taskSwitchConfig.Operation == TaskObjectOperation.Suspend)
            {
                taskObject.Suspend();
            }
        }
    }
}
