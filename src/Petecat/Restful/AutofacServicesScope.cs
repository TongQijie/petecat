using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Petecat.Restful
{
    internal class AutofacServicesScope : IServicesScope, IServicesLocator, IServiceProvider, IDisposable
    {
        private ILifetimeScope lifetimeScope;

        public AutofacServicesScope(ILifetimeScope lifetime)
        {
            this.lifetimeScope = lifetime;
        }

        public bool ContainService(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            return this.lifetimeScope.IsRegistered(serviceType);
        }

        public bool ContainService<TService>()
        {
            return this.lifetimeScope.IsRegistered<TService>();
        }

        public bool ContainService(Type serviceType, string subKey)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            if (string.IsNullOrEmpty(subKey))
            {
                return this.ContainService(serviceType);
            }
            return this.lifetimeScope.IsRegisteredWithName(subKey, serviceType);
        }

        public bool ContainService<TService>(string subKey)
        {
            if (string.IsNullOrEmpty(subKey))
            {
                return this.ContainService<TService>();
            }
            return this.lifetimeScope.IsRegisteredWithName<TService>(subKey);
        }

        public TService Resolve<TService>()
        {
            TService result = default(TService);
            if (this.ContainService<TService>())
            {
                result = this.lifetimeScope.Resolve<TService>();
            }
            return result;
        }

        public IEnumerable<TService> ResolveAll<TService>()
        {
            IEnumerable<TService> result;
            if (this.ContainService<TService>())
            {
                result = this.lifetimeScope.Resolve<IEnumerable<TService>>();
            }
            else
            {
                result = Enumerable.Empty<TService>();
            }
            return result;
        }

        public TService Resolve<TService>(string subKey)
        {
            bool flag = false;
            TService result = default(TService);
            if (string.IsNullOrEmpty(subKey) && this.ContainService<TService>())
            {
                flag = true;
                result = this.lifetimeScope.Resolve<TService>();
            }
            if (!flag && this.ContainService<TService>(subKey))
            {
                result = this.lifetimeScope.ResolveNamed<TService>(subKey);
            }
            return result;
        }

        public object GetService(Type serviceType, string subKey)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            bool flag = false;
            object result = null;
            if (string.IsNullOrEmpty(subKey) && this.ContainService(serviceType))
            {
                flag = true;
                result = this.lifetimeScope.Resolve(serviceType);
            }
            if (!flag && this.ContainService(serviceType, subKey))
            {
                result = this.lifetimeScope.ResolveNamed(subKey, serviceType);
            }
            return result;
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
                result = this.lifetimeScope.Resolve(serviceType);
            }
            return result;
        }

        public object GetAllServices(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            object result = null;
            if (this.ContainService(serviceType))
            {
                Type type = typeof(IEnumerable<>);
                type = type.MakeGenericType(new Type[]
				{
					serviceType
				});
                result = this.lifetimeScope.Resolve(type);
            }
            return result;
        }

        public TService GetInstance<TService>() where TService : class
        {
            return this.Resolve<TService>();
        }

        public void Dispose()
        {
            this.lifetimeScope.Dispose();
        }
    }
}
