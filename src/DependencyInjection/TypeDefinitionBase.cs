using System;
using System.Reflection;

using Petecat.Extension;

namespace Petecat.DependencyInjection
{
    public class TypeDefinitionBase : ITypeDefinition
    {
        public TypeDefinitionBase() { }

        public TypeDefinitionBase(Type type)
        {
            Info = type;
        }

        public TypeDefinitionBase(Type type, Type inference, bool sington, IAssemblyInfo assemblyInfo) : this(type)
        {
            Inference = inference;
            Sington = sington;
            AssemblyInfo = assemblyInfo;
        }

        public MemberInfo Info { get; protected set; }

        protected IConstructorMethodInfo[] _ConstructorMethods = null;

        public virtual IConstructorMethodInfo[] ConstructorMethods
        {
            get
            {
                if (_ConstructorMethods == null)
                {
                    var constructorMethods = new IConstructorMethodInfo[0];

                    foreach (var constructorInfo in (Info as Type).GetConstructors())
                    {
                        constructorMethods = constructorMethods.Append(new ConstructorMethodInfoBase(this, constructorInfo));
                    }

                    _ConstructorMethods = constructorMethods;
                }

                return _ConstructorMethods;
            }
        }

        protected IInstanceMethodInfo[] _InstanceMethods = null;

        public virtual IInstanceMethodInfo[] InstanceMethods
        {
            get
            {
                if (_InstanceMethods == null)
                {
                    var instanceMethods = new IInstanceMethodInfo[0];

                    foreach (var methodInfo in (Info as Type).GetMethods())
                    {
                        instanceMethods = instanceMethods.Append(new InstanceMethodInfoBase(this, methodInfo));
                    }

                    _InstanceMethods = instanceMethods;
                }

                return _InstanceMethods;
            }
        }

        protected IPropertyInfo[] _Properties = null;

        public virtual IPropertyInfo[] Properties
        {
            get
            {
                if (_Properties == null)
                {
                    var properties = new IPropertyInfo[0];

                    foreach (var propertyInfo in (Info as Type).GetProperties())
                    {
                        properties = properties.Append(new PropertyInfoBase(this, propertyInfo));
                    }

                    _Properties = properties;
                }

                return _Properties;
            }
        }

        public IAssemblyInfo AssemblyInfo { get; protected set; }

        public Type Inference { get; protected set; }

        public bool Sington { get; protected set; }

        private object _SingtonInstance = null;

        public object GetInstance(params object[] parameters)
        {
            if (Sington)
            {
                if (_SingtonInstance == null)
                {
                    _SingtonInstance = Activator.CreateInstance(Info as Type, parameters);
                }

                return _SingtonInstance;
            }
            else
            {
                return Activator.CreateInstance(Info as Type, parameters);
            }
        }
    }
}
