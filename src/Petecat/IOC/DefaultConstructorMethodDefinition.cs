using System.Collections.Generic;
using System.Reflection;

namespace Petecat.IOC
{
    public class DefaultConstructorMethodDefinition : AbstractMethodDefinition, IConstructorMethodDefinition
    {
        public DefaultConstructorMethodDefinition(ConstructorInfo constructorInfo)
            : base(constructorInfo)
        {
            Info = constructorInfo;
        }
    }
}
