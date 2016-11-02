using System;
using System.Reflection;

using Petecat.Utility;

namespace Petecat.DependencyInjection
{
    public class DefaultTypeDefinition : ITypeDefinition
    {
        public DefaultTypeDefinition(Type type)
        {
            Info = type;
            Attributes.DependencyInjectableAttribute attribute;
            if (ReflectionUtility.TryGetCustomAttribute(type, null, out attribute))
            {
                Inference = attribute.Inference;
            }
            AssemblyInfo = new DefaultAssemblyInfo(type.Assembly);
        }

        public MemberInfo Info { get; private set; }

        private IConstructorMethodInfo[] _ConstructorMethods = null;

        public IConstructorMethodInfo[] ConstructorMethods
        {
            get { throw new System.NotImplementedException(); }
        }

        private IInstanceMethodInfo[] _InstanceMethods = null;

        public IInstanceMethodInfo[] InstanceMethods
        {
            get { throw new System.NotImplementedException(); }
        }

        private IPropertyDefinition[] _Properties = null;

        public IPropertyDefinition[] Properties
        {
            get { throw new System.NotImplementedException(); }
        }

        public IAssemblyInfo AssemblyInfo { get; private set; }

        public Type Inference { get; private set; }

        public object GetInstance(params object[] parameters)
        {
            return Activator.CreateInstance(Info as Type, parameters);
        }
    }
}
