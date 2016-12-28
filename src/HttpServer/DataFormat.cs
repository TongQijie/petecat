namespace Petecat.HttpServer
{
    public enum DataFormat
    {
        /// <summary>
        /// support any data format.
        /// </summary>
        Any = 0,

        /// <summary>
        /// application/json
        /// </summary>
        Json = 1,

        /// <summary>
        /// application/xml
        /// text/xml
        /// </summary>
        Xml = 2,

        /// <summary>
        /// text/plain
        /// </summary>
        Text = 3,
    }
}
