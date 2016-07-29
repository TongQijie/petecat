using System;

namespace Petecat.Service.Attributes
{
    /// <summary>
    /// 标记此特性的方法为服务方法
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ServiceMethodAttribute : Attribute
    {
        public ServiceMethodAttribute()
        {
        }

        public ServiceMethodAttribute(string methodName, ServiceAccessMethod accessMethods)
        {
            MethodName = methodName;
            AccessMethods = accessMethods;
        }

        /// <summary>
        /// 获取或设置服务方法名
        /// </summary>
        public string MethodName { get; set; }

        public ServiceAccessMethod AccessMethods { get; set; }

        /// <summary>
        /// 获取或设置是否为默认服务方法
        /// </summary>
        public bool IsDefaultMethod { get; set; }
    }
}
