using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petecat.Restful
{
    [AutoSetupService(typeof(IServicesContainerWithBizUnit))]
    internal class DefaultServicesContainerWithBizUnit : IServicesContainerWithBizUnit, IServicesContainer, IServicesLocator, IServiceProvider
    {
        private readonly IServicesContainer container;

        private readonly IBizUnit bizunit;

        public IBizUnit BizUnit
        {
            get
            {
                return this.bizunit;
            }
        }

        public DefaultServicesContainerWithBizUnit(IServicesContainer container)
        {
            this.container = container;
            this.bizunit = container.Resolve<IBizUnit>();
        }

        public IServicesScope CreateScope()
        {
            return new DefaultServicesScopeWithBizUnit(this.container.CreateScope(), this.bizunit);
        }

        public bool ContainService(Type serviceType)
        {
            return this.container.ContainService(serviceType);
        }

        public bool ContainService<TService>()
        {
            return this.container.ContainService<TService>();
        }

        public bool ContainService(Type serviceType, string subKey)
        {
            return this.container.ContainService(serviceType, subKey);
        }

        public bool ContainService<TService>(string subKey)
        {
            return this.container.ContainService<TService>(subKey);
        }

        public object GetAllServices(Type serviceType)
        {
            return this.container.GetAllServices(serviceType);
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            object result = null;
            if (this.ContainService(serviceType))
            {
                bool resolvedService = false;
                string bizunitSubkey = this.GetBizunitSubKey();
                if (!string.IsNullOrWhiteSpace(bizunitSubkey))
                {
                    if (this.ContainService(serviceType, bizunitSubkey))
                    {
                        resolvedService = true;
                        result = this.GetService(serviceType, bizunitSubkey);
                    }
                }
                if (!resolvedService)
                {
                    result = this.container.GetService(serviceType);
                }
            }
            return result;
        }

        public object GetService(Type serviceType, string subKey)
        {
            return this.container.GetService(serviceType, subKey);
        }

        public TService Resolve<TService>()
        {
            TService result = default(TService);
            if (this.ContainService<TService>())
            {
                bool resolvedService = false;
                string bizunitSubkey = this.GetBizunitSubKey();
                if (!string.IsNullOrWhiteSpace(bizunitSubkey))
                {
                    if (this.ContainService<TService>(bizunitSubkey))
                    {
                        resolvedService = true;
                        result = this.Resolve<TService>(bizunitSubkey);
                    }
                }
                if (!resolvedService)
                {
                    result = this.container.Resolve<TService>();
                }
            }
            return result;
        }

        public IEnumerable<TService> ResolveAll<TService>()
        {
            return this.container.ResolveAll<TService>();
        }

        public TService Resolve<TService>(string subKey)
        {
            return this.container.Resolve<TService>(subKey);
        }

        public TService GetInstance<TService>() where TService : class
        {
            return this.Resolve<TService>();
        }

        private string GetBizunitSubKey()
        {
            string result = null;
            if (this.bizunit != null)
            {
                result = this.bizunit.CountryCode;
            }
            return result;
        }
    }
}
