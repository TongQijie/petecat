using System;

namespace Petecat.Data
{
    public interface IComparer
    {
        int Compare(params Func<int>[] conditions);
    }
}
