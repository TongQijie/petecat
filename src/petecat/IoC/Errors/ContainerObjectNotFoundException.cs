using System;

namespace Petecat.IoC.Errors
{
    public class ContainerObjectNotFoundException : Exception
    {
        public ContainerObjectNotFoundException(string objectName)
            : base(string.Format("container object '{0}' does not exist.", objectName))
        {
        }
    }
}
