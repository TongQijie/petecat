namespace Petecat.Jobs
{
    public interface IJobDispatcher
    {
        IJob[] Jobs { get; }

        void Setup(IJob job);

        void StartAll();

        void StopAll();

        void Start(string name);

        void Stop(string name);

        void Suspend(string name);
    }
}