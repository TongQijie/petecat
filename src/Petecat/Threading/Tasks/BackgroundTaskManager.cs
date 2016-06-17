using System;
using System.Collections.Generic;
using System.Linq;

using Petecat.Configuration;
using Petecat.Utility;

namespace Petecat.Threading.Tasks
{
    public class BackgroundTaskManager
    {
        public BackgroundTaskManager(string backgroundTaskConfigFile)
        {
            FileConfigurationManager = new FileConfigurationManagerBase(true);
            FileConfigurationManager.LoadFromXml<Configuration.BackgroundTaskCollection>(backgroundTaskConfigFile, "BackgroundTasks");
            FileConfigurationManager.ConfigurationItemChanged += FileConfigurationManager_ConfigurationItemChanged;
        }

        void FileConfigurationManager_ConfigurationItemChanged(IConfigurationManager configurationManager, string key)
        {
            var backgroundTasks = configurationManager.Get<Configuration.BackgroundTaskCollection>(key, null);
            if (backgroundTasks != null)
            {
                Restart();
            }
            else
            {
                Stop();
            }
        }

        private FileConfigurationManagerBase FileConfigurationManager { get; set; }

        private List<AbstractBackgroundTask> _BackgroundTasks = null;

        public List<AbstractBackgroundTask> BackgroundTasks { get { return _BackgroundTasks ?? (_BackgroundTasks = new List<AbstractBackgroundTask>()); } }

        public bool IsAlive { get; private set; }

        public void Start()
        {
            IsAlive = true;

            var backgroundTaskConfigs = FileConfigurationManager.Get<Configuration.BackgroundTaskCollection>("BackgroundTasks", null);
            if (backgroundTaskConfigs == null)
            {
                return;
            }

            foreach (var backgroundTaskConfig in backgroundTaskConfigs.BackgroundTasks.Where(x => x.Active))
            {
                var backgroundTask = ReflectionUtility.GetInstance<AbstractBackgroundTask>(backgroundTaskConfig.Provider);
                if (backgroundTask != null)
                {
                    if (!BackgroundTasks.Exists(x => x.Key.Equals(backgroundTask.Key, StringComparison.OrdinalIgnoreCase)))
                    {
                        BackgroundTasks.Add(backgroundTask);
                        backgroundTask.Execute();
                    }
                }
            }
        }

        public void Stop()
        {
            foreach (var backgroundTask in BackgroundTasks)
            {
                backgroundTask.Dispose();
            }
            BackgroundTasks.Clear();

            IsAlive = false;
        }

        public void Restart()
        {
            Stop();

            Start();
        }
    }
}