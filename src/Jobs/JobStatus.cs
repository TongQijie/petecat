namespace Petecat.Jobs
{
    public enum JobStatus
    {
        /// <summary>
        /// Job is not running.
        /// </summary>
        Stopped = 0,

        /// <summary>
        /// Job is running.
        /// </summary>
        Executing = 1,

        /// <summary>
        /// Job is stopped, but can resume.
        /// </summary>
        Suspended = 2,

        /// <summary>
        /// transition status: Executing -> Suspending
        /// </summary>
        Suspending = 3,

        /// <summary>
        /// transition status: Suspending -> Executing
        /// </summary>
        Resuming = 4,

        /// <summary>
        /// transition status: Executing -> Sleep
        /// </summary>
        Terminating = 5,
    }
}