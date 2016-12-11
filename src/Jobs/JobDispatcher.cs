using System;
using System.Linq;
using System.Collections.Concurrent;

using Petecat.Extending;
using Petecat.DependencyInjection.Attribute;

namespace Petecat.Jobs
{
    [DependencyInjectable(Inference = typeof(IJobDispatcher), Singleton = true)]
    public class JobDispatcher : IJobDispatcher
    {
        private ConcurrentDictionary<string, IJob> _Jobs = new ConcurrentDictionary<string, IJob>();

        public IJob[] Jobs { get { return _Jobs.Values.ToArray(); } }

        public void Setup(IJob job)
        {
            _Jobs.TryAdd(job.Name, job);
        }

        public void StartAll()
        {
            Jobs.Each(x => x.Execute());
        }

        public void StopAll()
        {
            Jobs.Each(x => x.Terminate());
        }

        public void Start(string name)
        {
            if (!_Jobs.ContainsKey(name))
            {
                throw new Exception(string.Format("job '{0}' does not exists.", name));
            }

            IJob job;
            if (_Jobs.TryGetValue(name, out job))
            {
                job.Execute();
            }
        }

        public void Stop(string name)
        {
            if (!_Jobs.ContainsKey(name))
            {
                throw new Exception(string.Format("job '{0}' does not exists.", name));
            }

            IJob job;
            if (_Jobs.TryGetValue(name, out job))
            {
                job.Terminate();
            }
        }

        public void Suspend(string name)
        {
            if (!_Jobs.ContainsKey(name))
            {
                throw new Exception(string.Format("job '{0}' does not exists.", name));
            }

            IJob job;
            if (_Jobs.TryGetValue(name, out job))
            {
                job.Suspend();
            }
        }
    }
}