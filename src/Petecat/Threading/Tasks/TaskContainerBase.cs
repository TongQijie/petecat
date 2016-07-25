using Petecat.Collection;
using Petecat.Data.Formatters;

using System.Text;

namespace Petecat.Threading.Tasks
{
    public class TaskContainerBase : ITaskContainer
    {
        private ThreadSafeKeyedObjectCollection<string, ITaskObject> _TaskObjects = new ThreadSafeKeyedObjectCollection<string, ITaskObject>();

        public void Initialize(string path)
        {
            var taskContainerConfig = new XmlFormatter().ReadObject<Configuration.TaskContainerConfig>(path, Encoding.UTF8);

            if (taskContainerConfig.TaskObjects != null && taskContainerConfig.TaskObjects.Length > 0)
            {
                
            }
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
    }
}
