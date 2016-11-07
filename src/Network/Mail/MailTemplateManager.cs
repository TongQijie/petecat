using Petecat.Caching;
using Petecat.Data.Formatters;
using Petecat.Network.Configuration;

using System.Linq;
using System.IO;
using System;

namespace Petecat.Network.Mail
{
    public class MailTemplateManager
    {
        public const string CacheObjectName = "Global_MailTemplates";

        private static MailTemplateManager _Instance = null;

        public static MailTemplateManager Instance { get { return _Instance ?? (_Instance = new MailTemplateManager()); } }

        public MailTemplateManager Initialize(string configPath)
        {
            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException(configPath);
            }

            CacheObjectManager.Instance.Add<MailTemplateCollectionConfig>(CacheObjectName, 
                                                                          configPath,
                                                                          ObjectFormatterFactory.GetFormatter(ObjectFormatterType.Xml), 
                                                                          true);

            return this;
        }

        public MailTemplateConfig GetMailTemplate(string templateId)
        {
            var cacheObject = CacheObjectManager.Instance.GetObject(CacheObjectName);
            if (cacheObject == null)
            {
                throw new Errors.MailTemplateManagerNotInitializedException();
            }

            var mailTemplatesConfig = cacheObject.GetValue() as MailTemplateCollectionConfig;
            if (mailTemplatesConfig == null)
            {
                throw new Errors.MailTemplateNotFoundException(templateId);
            }

            var mailTemplateConfig = mailTemplatesConfig.Templates.FirstOrDefault(x => x.TemplateId.Equals(templateId, StringComparison.OrdinalIgnoreCase));
            if (mailTemplateConfig == null)
            {
                throw new Errors.MailTemplateNotFoundException(templateId);
            }

            return mailTemplateConfig;
        }
    }
}
