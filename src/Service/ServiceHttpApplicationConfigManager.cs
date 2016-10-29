using Petecat.Configuring;
using Petecat.Service.Configuration;
using Petecat.Extension;
using System;
namespace Petecat.Service
{
    public class ServiceHttpApplicationConfigManager
    {
        private static ServiceHttpApplicationConfigManager _Instance = null;

        public static ServiceHttpApplicationConfigManager Instance { get { return _Instance ?? (_Instance = new ServiceHttpApplicationConfigManager()); } }

        public string GetStaticResourceContentMapping(string key)
        {
            var serviceHttpApplicationConfig = ConfigurationFactory.GetManager().GetValue<ServiceHttpApplicationConfig>("Global_ServiceHttpApplication");
            if (serviceHttpApplicationConfig == null 
                || serviceHttpApplicationConfig.StaticResourceContentMapping == null
                || serviceHttpApplicationConfig.StaticResourceContentMapping.KeyValues == null
                || serviceHttpApplicationConfig.StaticResourceContentMapping.KeyValues.Length == 0)
            {
                return null;
            }

            var contentMapping = serviceHttpApplicationConfig.StaticResourceContentMapping.KeyValues.FirstOrDefault(x => string.Equals(key, x.Key, StringComparison.OrdinalIgnoreCase));
            if (contentMapping == null)
            {
                return null;
            }

            return contentMapping.Value;
        }

        public string GetServiceHttpRouting(string key)
        {
            var serviceHttpApplicationConfig = ConfigurationFactory.GetManager().GetValue<ServiceHttpApplicationConfig>("Global_ServiceHttpApplication");
            if (serviceHttpApplicationConfig == null
                || serviceHttpApplicationConfig.StaticResourceContentMapping == null
                || serviceHttpApplicationConfig.StaticResourceContentMapping.KeyValues == null
                || serviceHttpApplicationConfig.StaticResourceContentMapping.KeyValues.Length == 0)
            {
                return null;
            }

            var httpRouting = serviceHttpApplicationConfig.ServiceHttpRouting.KeyValues.FirstOrDefault(x => string.Equals(key, x.Key, StringComparison.OrdinalIgnoreCase));
            if (httpRouting == null)
            {
                return null;
            }

            return httpRouting.Value;
        }
    }
}
