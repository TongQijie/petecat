namespace Petecat.Logging
{
    public enum Severity
    {
        /// <summary>
        /// 调试级别，用于调试模式下的日志级别
        /// </summary>
        Debug = 0,

        /// <summary>
        /// 信息级别，用于日常运行日志输出
        /// </summary>
        Info = 1,

        /// <summary>
        /// 警告级别，用于运行异常报告，此类异常并不会对系统造成重要影响
        /// </summary>
        Warn = 2,

        /// <summary>
        /// 错误级别，用于运行异常报告，此类异常可能会终止系统子模块或业务的运行
        /// </summary>
        Error = 3,

        /// <summary>
        /// 致命级别，用于灾难性异常报告，此类异常可能会导致整个系统的瘫痪或终止，应紧急应对
        /// </summary>
        Fatal = 4,
    }
}
