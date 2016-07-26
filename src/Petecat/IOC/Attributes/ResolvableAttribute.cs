using System;

namespace Petecat.IOC.Attributes
{
    /// <summary>
    /// 标记此特性的类自动注册到IOC容器中
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class ResolvableAttribute : Attribute
    {
    }
}
