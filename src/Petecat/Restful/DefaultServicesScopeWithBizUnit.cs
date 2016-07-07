using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petecat.Restful
{
    internal class DefaultServicesScopeWithBizUnit : IServicesScope, IServicesLocator, IServiceProvider, IDisposable
    {
        private readonly IServicesScope scope;

        private readonly IBizUnit bizunit;

        public DefaultServicesScopeWithBizUnit(IServicesScope scope, IBizUnit bizunit)
        {
            this.scope = scope;
            this.bizunit = bizunit;
        }

        public bool ContainService(Type serviceType)
        {
            return this.scope.ContainService(serviceType);
        }

        public bool ContainService<TService>()
        {
            return this.scope.ContainService<TService>();
        }

        public bool ContainService(Type serviceType, string subKey)
        {
            return this.scope.ContainService(serviceType, subKey);
        }

        public bool ContainService<TService>(string subKey)
        {
            return this.scope.ContainService<TService>(subKey);
        }

        public object GetAllServices(Type serviceType)
        {
            return this.scope.GetAllServices(serviceType);
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
                    result = this.scope.GetService(serviceType);
                }
            }
            return result;
        }

        public object GetService(Type serviceType, string subKey)
        {
            return this.scope.GetService(serviceType, subKey);
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
                    result = this.scope.Resolve<TService>();
                }
            }
            return result;
        }

        public IEnumerable<TService> ResolveAll<TService>()
        {
            return this.scope.ResolveAll<TService>();
        }

        public TService Resolve<TService>(string subKey)
        {
            return this.scope.Resolve<TService>(subKey);
        }

        public TService GetInstance<TService>() where TService : class
        {
            return this.Resolve<TService>();
        }

        public void Dispose()
        {
            this.scope.Dispose();
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
