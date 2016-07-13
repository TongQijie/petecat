using Autofac;
using System.Collections.Generic;
namespace Petecat.Restful
{
    public interface IAutofacServicesRegistor
    {
        void Registor(ContainerBuilder builder, IEnumerable<IServiceDefinition> services);
    }
}
