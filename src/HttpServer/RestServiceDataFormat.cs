namespace Petecat.HttpServer
{
    public enum RestServiceDataFormat
    {
        /// <summary>
        /// support any data format.
        /// </summary>
        Any = 0,

        /// <summary>
        /// only support json format
        /// </summary>
        Json = 1,

        /// <summary>
        /// only support xml format
        /// </summary>
        Xml = 2,
    }
}
