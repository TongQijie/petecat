using System;

namespace Petecat.IoC.Attributes
{
    /// <summary>
    /// 标记此特性的类支持IOC容器自动解析
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class AutoResolvableAttribute : ResolvableAttribute
    {
        public AutoResolvableAttribute(Type specifiedType)
        {
            SpecifiedType = specifiedType;
        }

        public Type SpecifiedType { get; set; }
    }
}
