using System;
namespace Petecat.Network.Errors
{
    public class MailTemplateFromMissingException : Exception
    {
        public MailTemplateFromMissingException(string templateId)
            : base(string.Format("mail template '{0}' from is missing.", templateId))
        {
        }
    }
}
