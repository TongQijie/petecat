using Petecat.Collection;

namespace Petecat.Caching
{
    public interface ICacheObject : IKeyedObject<string>
    {
        bool IsDirty { get; set; }

        object GetValue();
    }
}
