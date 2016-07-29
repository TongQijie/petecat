using Petecat.IoC.Attributes;

using System;

namespace Petecat.Service.Attributes
{
    /// <summary>
    /// 标记此特性的类为服务接口类
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class ServiceInterfaceAttribute : ResolvableAttribute
    {
        /// <summary>
        /// 获取或设置服务名称
        /// </summary>
        public string ServiceName { get; set; }
    }
}
