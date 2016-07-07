using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petecat.Restful
{
    public interface IServicesContainerWithBizUnit : IServicesContainer, IServicesLocator, IServiceProvider
    {
        IBizUnit BizUnit
        {
            get;
        }
    }
}
