using System;
namespace Petecat.Aop
{
    public interface IAopTypeGenerator
    {
        Type Generate(Type baseClass);
    }
}
