using System;
namespace Petecat.Service
{
    [Flags]
    public enum ServiceAccessMethod : int
    {
        HttpGet = 0,

        HttpPost = 1,
    }
}
