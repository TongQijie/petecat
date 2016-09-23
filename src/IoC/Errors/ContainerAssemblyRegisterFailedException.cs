using System;

namespace Petecat.IoC.Errors
{
    public class ContainerAssemblyRegisterFailedException : Exception
    {
        public ContainerAssemblyRegisterFailedException(string assemblyName, Exception innerException)
            : base(string.Format("assembly '{0}' fails to register in container.", assemblyName), innerException)
        {
        }
    }
}
