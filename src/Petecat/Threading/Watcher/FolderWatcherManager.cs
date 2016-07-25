using Petecat.Collection;

namespace Petecat.Threading.Watcher
{
    public class FolderWatcherManager
    {
        private static FolderWatcherManager _Instance = null;

        public static FolderWatcherManager Instance { get { return _Instance ?? (_Instance = new FolderWatcherManager()); } }

        private ThreadSafeKeyedObjectCollection<string, FolderWatcher> _FolderWatchers = new ThreadSafeKeyedObjectCollection<string, FolderWatcher>();

        public FolderWatcher GetOrAdd(string directory)
        {
            if (_FolderWatchers.ContainsKey(directory))
            {
                return _FolderWatchers.Get(directory, null);
            }
            else
            {
                return _FolderWatchers.Add(new FolderWatcher(directory));
            }
        }
    }
}
