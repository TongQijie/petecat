using System;
namespace Petecat.Network.Errors
{
    public class MailTemplateToMissingException : Exception
    {
        public MailTemplateToMissingException(string templateId)
            : base(string.Format("mail template '{0}' to is missing.", templateId))
        {
        }
    }
}
