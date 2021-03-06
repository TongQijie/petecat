﻿using Petecat.Formatter.Internal;

namespace Petecat.Formatter.Json
{
    public abstract class JsonObject
    {
        /// <summary>
        /// fill object content, including internal elements or value
        /// </summary>
        /// <param name="stream">object data stream</param>
        /// <param name="seperators">bytes that indicate external object can continue to fill next object.</param>
        /// <param name="terminators">bytes that indicate current filling object is the last object in external object.</param>
        /// <returns>if true, indicate current filling object is the last object in external object, else false</returns>
        public virtual bool Fill(IStream stream, byte[] seperators, byte[] terminators)
        {
            return false;
        }
    }
}
