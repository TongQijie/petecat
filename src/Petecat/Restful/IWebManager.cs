namespace Petecat.Restful
{
    /// <summary>
    /// Web app manager interface.
    /// </summary>
    public interface IWebAppManager
    {
        /// <summary>
        /// Do some work when application start. 
        /// </summary>
        void Start();

        /// <summary>
        /// Do same work when request start. 
        /// </summary>
        void BeginRequest();

        /// <summary>
        /// Do same work when request end. 
        /// </summary>
        void EndRequest();
    }
}
