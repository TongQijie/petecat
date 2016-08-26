using System;
namespace Petecat.Network.Errors
{
    public class MailTemplateNotFoundException : Exception
    {
        public MailTemplateNotFoundException(string templateId)
            : base(string.Format("mail template '{0}' does not exsit.", templateId))
        {
        }
    }
}
