using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petecat.Service.Errors
{
    public class ServiceHttpMethodNotSupportException : Exception
    {
        public ServiceHttpMethodNotSupportException(string httpMethod)
            : base(string.Format("http method '{0}' is not supported by now.", httpMethod))
        {
        }
    }
}
