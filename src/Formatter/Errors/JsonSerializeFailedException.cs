using System;

namespace Petecat.Formatter.Errors
{
    public class JsonSerializeFailedException : Exception
    {
        public JsonSerializeFailedException(string elementName, string description)
            : base(string.Format("element '{0}' fails to serialize.{1}description: {2}", elementName, Environment.NewLine, description))
        {
        }
    }
}
