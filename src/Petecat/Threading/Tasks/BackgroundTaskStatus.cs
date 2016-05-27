namespace Petecat.Threading.Tasks
{
    public enum BackgroundTaskStatus
    {
        /// <summary>
        /// 等待状态
        /// </summary>
        Sleep,

        /// <summary>
        /// 正在执行状态
        /// </summary>
        Executing,

        /// <summary>
        /// 等待挂起状态
        /// </summary>
        Suspending,

        /// <summary>
        /// 挂起状态
        /// </summary>
        Suspended,
    }
}
