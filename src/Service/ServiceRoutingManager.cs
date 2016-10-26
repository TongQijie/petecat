using Petecat.Extension;
using Petecat.Configuring;
using Petecat.Service.Configuration;

using System;

namespace Petecat.Service
{
    public class ServiceRoutingManager
    {
        private static ServiceRoutingManager _Instance = null;

        public static ServiceRoutingManager Instance { get { return _Instance ?? (_Instance = new ServiceRoutingManager()); } }

        public string GetRoutingRule(string name)
        {
            var serviceRoutingConfig = ConfigurationFactory.GetManager().GetValue<ServiceRoutingConfig>("Global_ServiceRouting");
            if (serviceRoutingConfig == null || serviceRoutingConfig.Rules == null || serviceRoutingConfig.Rules.Length == 0)
            {
                return null;
            }

            var routingRule = serviceRoutingConfig.Rules.FirstOrDefault(x => string.Equals(name, x.Name, StringComparison.OrdinalIgnoreCase));
            if (routingRule == null)
            {
                return null;
            }

            return routingRule.Value;
        }
    }
}
