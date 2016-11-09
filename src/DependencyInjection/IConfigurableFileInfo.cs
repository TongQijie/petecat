namespace Petecat.DependencyInjection
{
    public interface IConfigurableFileInfo
    {
        string Path { get; }

        IInstanceInfo[] GetInstanceInfos();
    }
}