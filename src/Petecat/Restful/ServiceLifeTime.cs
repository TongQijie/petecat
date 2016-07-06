namespace Petecat.Restful
{
    /// <summary>
    /// Service life time.
    /// </summary>
    public enum ServiceLifeTime
    {
        /// <summary>
        /// Per Resolve One Same Instance in a Scope.
        /// </summary>
        Scope,
        /// <summary>
        /// Just exist one instance in all application life time.
        /// </summary>
        Singleton,
        /// <summary>
        /// Every time when you get a service a new instance will be create.
        /// </summary>
        Transient
    }
}
