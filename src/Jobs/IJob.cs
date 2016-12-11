namespace Petecat.Jobs
{
    public interface IJob
    {
        string Name { get; }

        string Description { get; }

        void Execute();

        void Suspend();

        void Terminate();

        JobStatus Status { get; }
    }
}