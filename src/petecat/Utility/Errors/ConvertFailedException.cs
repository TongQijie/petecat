using System;

namespace Petecat.Utility.Errors
{
    public class ConvertFailedException : Exception
    {
        public ConvertFailedException(string sourceValue, string targetType)
            : base(string.Format("value '{0}' cannot be converted to type '{1}'", sourceValue, targetType))
        {
        }
    }
}
