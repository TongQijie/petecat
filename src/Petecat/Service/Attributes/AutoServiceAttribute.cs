using Petecat.IOC.Attributes;
using System;

namespace Petecat.Service.Attributes
{
    /// <summary>
    /// 标记此特性的类支持自动解析类，一般用于服务接口实现类
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AutoServiceAttribute : AutoResolvableAttribute
    {
        public AutoServiceAttribute(Type specifiedType) 
            : base(specifiedType)
        {
        }
    }
}
