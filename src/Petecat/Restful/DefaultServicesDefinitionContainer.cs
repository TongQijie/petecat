using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petecat.Restful
{
    /// <summary>
    /// Default services definition container.
    /// </summary>
    internal class DefaultServicesDefinitionContainer : IServicesDefinitionContainer
    {
        /// <summary>
        /// Services definition data.
        /// </summary>
        protected ConcurrentDictionary<Type, ConcurrentDictionary<string, IServiceDefinition>> data = new ConcurrentDictionary<Type, ConcurrentDictionary<string, IServiceDefinition>>();

        /// <summary>
        /// Gets services definition.
        /// </summary>
        public IEnumerable<IServiceDefinition> ServicesDefinition
        {
            get
            {
                return this.data.Values.SelectMany((ConcurrentDictionary<string, IServiceDefinition> item) => item.Values).ToArray<IServiceDefinition>();
            }
        }

        /// <summary>
        /// Register service with implement type and scope life time.
        /// </summary>
        /// <param name="service">Service type.</param>
        /// <param name="implement">Implement type.</param>
        public void RegisterService(Type service, Type implement)
        {
            this.RegisterService(service, implement, ServiceLifeTime.Scope);
        }

        /// <summary>
        /// Register service with implement type and scope life time.
        /// </summary>
        /// <typeparam name="TService">Service type.</typeparam>
        /// <typeparam name="TImplement">Implement type.</typeparam>
        public void RegisterService<TService, TImplement>()
        {
            this.RegisterService(typeof(TService), typeof(TImplement));
        }

        /// <summary>
        /// Register service with implement type and scope life time.
        /// </summary>
        /// <param name="service">Service type.</param>
        /// <param name="implement">Implement type.</param>
        /// <param name="subKey">Sub key.</param>
        public void RegisterService(Type service, Type implement, string subKey)
        {
            this.RegisterService(service, implement, subKey, ServiceLifeTime.Scope);
        }

        /// <summary>
        /// Register service with implement type and scope life time.
        /// </summary>
        /// <typeparam name="TService">Service type.</typeparam>
        /// <typeparam name="TImplement">Implement type.</typeparam>
        /// <param name="subKey">Sub key.</param>
        public void RegisterService<TService, TImplement>(string subKey)
        {
            this.RegisterService(typeof(TService), typeof(TImplement), subKey);
        }

        /// <summary>
        /// Register service with implement type and service life time.
        /// </summary>
        /// <param name="service">Service type.</param>
        /// <param name="implement">Implement type.</param>
        /// <param name="lifeTime">Service life time.</param>
        public void RegisterService(Type service, Type implement, ServiceLifeTime lifeTime)
        {
            this.RegisterService(service, implement, null, lifeTime);
        }

        /// <summary>
        /// Register service with implement type and service life time.
        /// </summary>
        /// <typeparam name="TService">Service type.</typeparam>
        /// <typeparam name="TImplement">Implement type.</typeparam>
        /// <param name="lifeTime">Service life time.</param>
        public void RegisterService<TService, TImplement>(ServiceLifeTime lifeTime)
        {
            this.RegisterService(typeof(TService), typeof(TImplement), lifeTime);
        }

        /// <summary>
        /// Register service with implement type and service life time.
        /// </summary>
        /// <param name="service">Service type.</param>
        /// <param name="implement">Implement type.</param>
        /// <param name="subKey">Sub key.</param>
        /// <param name="lifeTime">Service life time.</param>
        public void RegisterService(Type service, Type implement, string subKey, ServiceLifeTime lifeTime)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
            string realSubKey = string.IsNullOrEmpty(subKey) ? string.Empty : subKey;
            this.data.AddOrUpdate(service, delegate(Type type)
            {
                ConcurrentDictionary<string, IServiceDefinition> subDict = new ConcurrentDictionary<string, IServiceDefinition>();
                subDict.TryAdd(realSubKey, this.CreateServiceDefine(service, implement, null, subKey, lifeTime));
                return subDict;
            }, delegate(Type type, ConcurrentDictionary<string, IServiceDefinition> old)
            {
                old.AddOrUpdate(realSubKey, this.CreateServiceDefine(service, implement, null, subKey, lifeTime), (string k, IServiceDefinition o) => this.CreateServiceDefine(service, implement, null, subKey, lifeTime));
                return old;
            });
        }

        /// <summary>
        /// Register service with implement type and service life time.
        /// </summary>
        /// <typeparam name="TService">Service type.</typeparam>
        /// <typeparam name="TImplement">Implement type.</typeparam>
        /// <param name="subKey">Sub key.</param>
        /// <param name="lifeTime">Service life time.</param>
        public void RegisterService<TService, TImplement>(string subKey, ServiceLifeTime lifeTime)
        {
            this.RegisterService(typeof(TService), typeof(TImplement), subKey, lifeTime);
        }

        /// <summary>
        /// Register service with service factory and scope life time.
        /// </summary>
        /// <param name="service">Service type.</param>
        /// <param name="serviceFactory">Service factory.</param>
        public void RegisterService(Type service, Func<object> serviceFactory)
        {
            this.RegisterService(service, serviceFactory, ServiceLifeTime.Scope);
        }

        /// <summary>
        /// Register service with service factory and scope life time.
        /// </summary>
        /// <typeparam name="TService">Service type.</typeparam>
        /// <param name="serviceFactory">Service factory.</param>
        public void RegisterService<TService>(Func<object> serviceFactory)
        {
            this.RegisterService(typeof(TService), serviceFactory);
        }

        /// <summary>
        /// Register service with service factory and scope life time.
        /// </summary>
        /// <param name="service">Service type.</param>
        /// <param name="serviceFactory">Service factory.</param>
        /// <param name="subKey">Sub key.</param>
        public void RegisterService(Type service, Func<object> serviceFactory, string subKey)
        {
            this.RegisterService(service, serviceFactory, subKey, ServiceLifeTime.Scope);
        }

        /// <summary>
        /// Register service with service factory and scope life time.
        /// </summary>
        /// <typeparam name="TService">Service type.</typeparam>
        /// <param name="serviceFactory">Service factory.</param>
        /// <param name="subKey">Sub key.</param>
        public void RegisterService<TService>(Func<object> serviceFactory, string subKey)
        {
            this.RegisterService(typeof(TService), serviceFactory, subKey);
        }

        /// <summary>
        /// Register service with service factory and service life time.
        /// </summary>
        /// <param name="service">Service type.</param>
        /// <param name="serviceFactory">Service factory.</param>
        /// <param name="lifeTime">Service life time.</param>
        public void RegisterService(Type service, Func<object> serviceFactory, ServiceLifeTime lifeTime)
        {
            this.RegisterService(service, serviceFactory, null, lifeTime);
        }

        /// <summary>
        /// Register service with service factory and service life time.
        /// </summary>
        /// <typeparam name="TService">Service type.</typeparam>
        /// <param name="serviceFactory">Service factory.</param>
        /// <param name="lifeTime">Service life time.</param>
        public void RegisterService<TService>(Func<object> serviceFactory, ServiceLifeTime lifeTime)
        {
            this.RegisterService(typeof(TService), serviceFactory, lifeTime);
        }

        /// <summary>
        /// Register service with service factory and service life time.
        /// </summary>
        /// <param name="service">Service type.</param>
        /// <param name="serviceFactory">Service factory.</param>
        /// <param name="subKey">Sub key.</param>
        /// <param name="lifeTime">Service life time.</param>
        public void RegisterService(Type service, Func<object> serviceFactory, string subKey, ServiceLifeTime lifeTime)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
            string realSubKey = string.IsNullOrEmpty(subKey) ? string.Empty : subKey;
            this.data.AddOrUpdate(service, delegate(Type type)
            {
                ConcurrentDictionary<string, IServiceDefinition> subDict = new ConcurrentDictionary<string, IServiceDefinition>();
                subDict.TryAdd(realSubKey, this.CreateServiceDefine(service, null, serviceFactory, subKey, lifeTime));
                return subDict;
            }, delegate(Type type, ConcurrentDictionary<string, IServiceDefinition> old)
            {
                old.AddOrUpdate(realSubKey, this.CreateServiceDefine(service, null, serviceFactory, subKey, lifeTime), (string k, IServiceDefinition o) => this.CreateServiceDefine(service, null, serviceFactory, subKey, lifeTime));
                return old;
            });
        }

        /// <summary>
        /// Register service with service factory and service life time.
        /// </summary>
        /// <typeparam name="TService">Service type.</typeparam>
        /// <param name="serviceFactory">Service factory.</param>
        /// <param name="subKey">Sub key.</param>
        /// <param name="lifeTime">Service life time.</param>
        public void RegisterService<TService>(Func<object> serviceFactory, string subKey, ServiceLifeTime lifeTime)
        {
            this.RegisterService(typeof(TService), serviceFactory, subKey, lifeTime);
        }

        /// <summary>
        /// Create service define.
        /// </summary>
        /// <param name="service">Service type.</param>
        /// <param name="implement">Implement type.</param>
        /// <param name="serviceFactory">Service factory.</param>
        /// <param name="subKey">Sub key.</param>
        /// <param name="lifeTime">Service life time.</param>
        /// <returns>Service define.</returns>
        private IServiceDefinition CreateServiceDefine(Type service, Type implement, Func<object> serviceFactory, string subKey, ServiceLifeTime lifeTime)
        {
            if (implement == null && serviceFactory == null)
            {
                throw new ArgumentNullException("implement and serviceFactory");
            }
            return new ServiceDefinition
            {
                Service = service,
                Implement = implement,
                ServiceFactory = serviceFactory,
                SubKey = subKey,
                LifeTime = lifeTime
            };
        }
    }
}
