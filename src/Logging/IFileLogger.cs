namespace Petecat.Logging
{
    public interface IFileLogger : ILogger
    {
        string Folder { get; set; }

        Frequency Frequency { get; set; }
    }
}