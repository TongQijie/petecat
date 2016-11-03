namespace Petecat.Caching
{
    public interface IFileCacheItem : ICacheItem
    {
        string Path { get; }
    }
}
