using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petecat.Restful
{
    [AutoSetupService(typeof(IServicesLocator)), AutoSetupService(typeof(IServicesContainer))]
    public class DefaultServicesContainer : DefaultServicesScope, IAutoSetupServicesContainer, IServicesContainer, IServicesLocator, IServiceProvider
    {
        public int Priority
        {
            get
            {
                return 0;
            }
        }

        public DefaultServicesContainer(IServicesDefinitionContainer servicesDefinitionContainer, IActivator activator)
            : base(servicesDefinitionContainer, activator)
        {
        }

        public IServicesScope CreateScope()
        {
            return new DefaultServicesScope(this.registeredServices, this.singletonServiceInstances, this.activator);
        }
    }
}
