using System;

namespace Petecat.Caching
{
    public interface ICacheItem
    {
        string Key { get; }

        bool IsDirty { get; set; }

        DateTime CreationDate { get; }

        DateTime ModifiedDate { get; }

        object GetValue();
    }
}
