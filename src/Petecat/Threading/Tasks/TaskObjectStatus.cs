﻿namespace Petecat.Threading.Tasks
{
    public enum TaskObjectStatus
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

        /// <summary>
        /// 正在还原状态
        /// </summary>
        Resuming,

        /// <summary>
        /// 正在终止状态
        /// </summary>
        Terminating,
    }
}
