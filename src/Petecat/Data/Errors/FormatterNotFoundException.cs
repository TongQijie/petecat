using System;

namespace Petecat.Data.Errors
{
    public class FormatterNotFoundException : Exception
    {
        public FormatterNotFoundException()
            : base(string.Format("formatter not found."))
        {
        }

        public FormatterNotFoundException(string formatterString)
            : base(string.Format("formatter({0}) not found.", formatterString))
        {
        }
    }
}