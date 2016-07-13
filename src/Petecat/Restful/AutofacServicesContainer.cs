using Autofac;
using Autofac.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Petecat.Restful
{
    [AutoSetupService(typeof(IServicesContainer), Priority = 1), AutoSetupService(typeof(IServicesLocator), Priority = 1)]
    public class AutofacServicesContainer : IAutoSetupServicesContainer, IServicesContainer, IServicesLocator, IServiceProvider
    {
        private readonly ContainerBuilder mainBuilder = new ContainerBuilder();

        private readonly IContainer mainContainer;

        public int Priority
        {
            get
            {
                return 1;
            }
        }

        public AutofacServicesContainer(IServicesDefinitionContainer servicesDefinitionContainer)
        {
            new DefaultAutofacServicesRegistor().Registor(this.mainBuilder, servicesDefinitionContainer.ServicesDefinition);
            this.mainContainer = this.mainBuilder.Build(ContainerBuildOptions.None);
        }

        public bool ContainService(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            return this.mainContainer.IsRegistered(serviceType);
        }

        public bool ContainService<TService>()
        {
            return this.mainContainer.IsRegistered<TService>();
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
            return this.mainContainer.IsRegisteredWithName(subKey, serviceType);
        }

        public bool ContainService<TService>(string subKey)
        {
            if (string.IsNullOrEmpty(subKey))
            {
                return this.ContainService<TService>();
            }
            return this.mainContainer.IsRegisteredWithName<TService>(subKey);
        }

        public TService Resolve<TService>()
        {
            TService result = default(TService);
            if (this.ContainService<TService>())
            {
                result = this.mainContainer.Resolve<TService>();
            }
            return result;
        }

        public IEnumerable<TService> ResolveAll<TService>()
        {
            IEnumerable<TService> result;
            if (this.ContainService<TService>())
            {
                result = this.mainContainer.Resolve<IEnumerable<TService>>();
            }
            else
            {
                result = Enumerable.Empty<TService>();
            }
            return result;
        }

        public TService Resolve<TService>(string subKey)
        {
            TService result = default(TService);
            if (this.ContainService<TService>(subKey))
            {
                bool flag = false;
                if (string.IsNullOrEmpty(subKey) && this.ContainService<TService>())
                {
                    flag = true;
                    result = this.mainContainer.Resolve<TService>();
                }
                if (!flag)
                {
                    result = this.mainContainer.ResolveNamed<TService>(subKey);
                }
            }
            return result;
        }

        public TService GetInstance<TService>() where TService : class
        {
            return this.Resolve<TService>();
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
                result = this.mainContainer.Resolve(serviceType);
            }
            if (!flag && this.ContainService(serviceType, subKey))
            {
                result = this.mainContainer.ResolveNamed(subKey, serviceType);
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
                result = this.mainContainer.Resolve(serviceType);
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
                result = this.mainContainer.Resolve(type);
            }
            return result;
        }

        public IServicesScope CreateScope()
        {
            return new AutofacServicesScope(this.mainContainer.BeginLifetimeScope());
        }
    }
}
