using Autofac;
using Autofac.Builder;
using System;
using System.Collections.Generic;
namespace Petecat.Restful
{
    [AutoSetupService(typeof(IAutofacServicesRegistor))]
    public class DefaultAutofacServicesRegistor : IAutofacServicesRegistor
    {
        public void Registor(ContainerBuilder builder, IEnumerable<IServiceDefinition> services)
        {
            if (builder != null && !services.IsNullOrEmpty<IServiceDefinition>())
            {
                services.ForEach(delegate(IServiceDefinition service)
                {
                    this.RegisterServiceToBuilder(builder, service);
                });
            }
        }

        private void RegisterServiceToBuilder(ContainerBuilder builder, IServiceDefinition service)
        {
            if (service.ServiceFactory != null)
            {
                this.RegisterServiceWithFactoryToBuilder(builder, service);
                if (!string.IsNullOrEmpty(service.SubKey))
                {
                    this.RegisterServiceWithFactoryWithSubkeyToBuilder(builder, service);
                    return;
                }
            }
            else
            {
                this.RegisterServiceWithImplementToBuilder(builder, service);
                if (!string.IsNullOrEmpty(service.SubKey))
                {
                    this.RegisterServiceWithImplementWithSubKeyToBuilder(builder, service);
                }
            }
        }

        private void RegisterServiceWithImplementToBuilder(ContainerBuilder builder, IServiceDefinition service)
        {
            IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registrationBuilder = builder.RegisterType(service.Implement);
            IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registrationBuilder2 = registrationBuilder.As(new Type[]
			{
				service.Service
			});
            switch (service.LifeTime)
            {
                case ServiceLifeTime.Singleton:
                    registrationBuilder2.SingleInstance();
                    return;
                case ServiceLifeTime.Transient:
                    registrationBuilder2.InstancePerDependency();
                    return;
            }
            registrationBuilder2.InstancePerLifetimeScope();
        }

        private void RegisterServiceWithImplementWithSubKeyToBuilder(ContainerBuilder builder, IServiceDefinition service)
        {
            IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registrationBuilder = builder.RegisterType(service.Implement);
            IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registrationBuilder2 = registrationBuilder.Named(service.SubKey, service.Service);
            switch (service.LifeTime)
            {
                case ServiceLifeTime.Singleton:
                    registrationBuilder2.SingleInstance();
                    return;
                case ServiceLifeTime.Transient:
                    registrationBuilder2.InstancePerDependency();
                    return;
            }
            registrationBuilder2.InstancePerLifetimeScope();
        }

        private void RegisterServiceWithFactoryToBuilder(ContainerBuilder builder, IServiceDefinition service)
        {
            IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> registrationBuilder = builder.Register((IComponentContext c) => service.ServiceFactory());
            IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> registrationBuilder2 = registrationBuilder.As(new Type[]
			{
				service.Service
			});
            switch (service.LifeTime)
            {
                case ServiceLifeTime.Singleton:
                    registrationBuilder2.SingleInstance();
                    return;
                case ServiceLifeTime.Transient:
                    registrationBuilder2.InstancePerDependency();
                    return;
            }
            registrationBuilder2.InstancePerLifetimeScope();
        }

        private void RegisterServiceWithFactoryWithSubkeyToBuilder(ContainerBuilder builder, IServiceDefinition service)
        {
            IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> registrationBuilder = builder.Register((IComponentContext c) => service.ServiceFactory());
            IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> registrationBuilder2 = registrationBuilder.Named(service.SubKey, service.Service);
            switch (service.LifeTime)
            {
                case ServiceLifeTime.Singleton:
                    registrationBuilder2.SingleInstance();
                    return;
                case ServiceLifeTime.Transient:
                    registrationBuilder2.InstancePerDependency();
                    return;
            }
            registrationBuilder2.InstancePerLifetimeScope();
        }
    }
}
