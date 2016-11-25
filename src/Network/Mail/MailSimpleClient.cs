using Petecat.Network.Configuration;
using Petecat.Extending;

using System;
using System.Text;
using System.Net.Mail;

namespace Petecat.Network.Mail
{
    public class MailSimpleClient : IDisposable
    {
        public MailSimpleClient(Configuration.MailTemplateConfig templateConfig)
        {
            TemplateConfig = templateConfig;

            Client = new SmtpClient(templateConfig.Server, templateConfig.Port);
            Message = new MailMessage();

            if (!templateConfig.From.HasValue())
            {
                throw new Errors.MailTemplateFromMissingException(templateConfig.TemplateId);
            }
            Message.From = new MailAddress(templateConfig.From, templateConfig.DisplayName, Encoding.UTF8);

            if (!templateConfig.To.HasValue())
            {
                throw new Errors.MailTemplateToMissingException(templateConfig.TemplateId);
            }
            foreach (var to in templateConfig.To.SplitByChar(';'))
            {
                Message.To.Add(new MailAddress(to));
            }

            if (templateConfig.CC.HasValue())
            {
                foreach (var cc in templateConfig.CC.SplitByChar(';'))
                {
                    Message.CC.Add(new MailAddress(cc));
                }
            }

            Message.Body = templateConfig.Body;
            Message.BodyEncoding = Encoding.UTF8;
            Message.Subject = templateConfig.Subject;
            Message.SubjectEncoding = Encoding.UTF8;
        }

        public MailTemplateConfig TemplateConfig { get; private set; }

        public SmtpClient Client { get; private set; }

        public MailMessage Message { get; private set; }

        public MailSimpleClient SetSubject(params string[] parameters)
        {
            Message.Subject = string.Format(TemplateConfig.Subject, parameters);
            return this;
        }

        public MailSimpleClient SetBody(params string[] parameters)
        {
            Message.Body = string.Format(TemplateConfig.Body, parameters);
            return this;
        }

        public void Send()
        {
            try
            {
                Client.Send(Message);
            }
            catch (Exception e)
            {
                throw new Errors.MailSendFailedException(TemplateConfig.TemplateId, e);
            }
        }

        public void Dispose()
        {
            if (Message != null)
            {
                Message.Dispose();
                Message = null;
            }

            if (Client != null)
            {
                Client.Dispose();
                Client = null;
            }
        }
    }
}
