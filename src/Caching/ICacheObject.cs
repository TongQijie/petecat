using Petecat.Collection;

namespace Petecat.Caching
{
    public interface ICacheObject : IKeyedObject<string>
    {
        string Path { get; set; }

        bool IsDirty { get; set; }

        object GetValue();
    }
}
