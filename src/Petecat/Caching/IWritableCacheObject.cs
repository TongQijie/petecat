using System;

namespace Petecat.Caching
{
    public interface IWritableCacheObject : ICacheObject
    {
        void Flush();
    }
}
