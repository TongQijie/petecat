using System;
using System.Reflection;

namespace Petecat.DependencyInjection
{
    public class DefaultTypeDefinition : ITypeDefinition
    {
        public DefaultTypeDefinition(Type type)
        {
            Info = type;
        }

        public MemberInfo Info { get; set; }

        public IConstructorMethodDefinition[] ConstructorMethods
        {
            get { throw new System.NotImplementedException(); }
        }

        public IInstanceMethodDefinition[] InstanceMethods
        {
            get { throw new System.NotImplementedException(); }
        }

        public IPropertyDefinition[] Properties
        {
            get { throw new System.NotImplementedException(); }
        }

        public IAssemblyInfo AssemblyInfo
        {
            get { throw new System.NotImplementedException(); }
        }

        public object GetInstance(params object[] parameters)
        {
            throw new System.NotImplementedException();
        }

        
    }
}
