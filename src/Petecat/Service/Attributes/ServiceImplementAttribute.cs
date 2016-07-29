using System;

using Petecat.IoC.Attributes;

namespace Petecat.Service.Attributes
{
    /// <summary>
    /// 标记此特性的类为服务实现类
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceImplementAttribute : ResolvableAttribute
    {
        /// <summary>
        /// 获取或设置服务名称
        /// </summary>
        public string ServiceName { get; set; }
    }
}
