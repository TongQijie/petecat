using System;

namespace Petecat.WebServer
{
    public interface IWorker
    {
        Guid Id { get; }

        void Run();

        void Close();
    }
}
