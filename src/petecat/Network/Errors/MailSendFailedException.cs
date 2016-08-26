using System;
namespace Petecat.Network.Errors
{
    public class MailSendFailedException : Exception
    {
        public MailSendFailedException(string templateId, Exception innerException)
            : base(string.Format("mail template '{0}' fails to send.", templateId), innerException)
        {
        }
    }
}
