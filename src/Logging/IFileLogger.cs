namespace Petecat.Logging
{
    public interface IFileLogger : ILogger
    {
        string Folder { get; }
    }
}