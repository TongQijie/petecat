using System.Collections.Generic;
using System.Reflection;

namespace Petecat.IoC
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
