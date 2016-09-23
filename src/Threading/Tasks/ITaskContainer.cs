namespace Petecat.Threading.Tasks
{
    public interface ITaskContainer
    {
        ITaskObject GetOrAdd(ITaskObject taskObject);

        void StartAll();

        void StopAll();

        void Start(string name);

        void Suspend(string name);

        void Stop(string name);

        void Resume(string name);
    }
}
