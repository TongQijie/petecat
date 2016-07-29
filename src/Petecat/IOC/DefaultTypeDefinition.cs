using System;
using System.Linq;
using System.Reflection;

namespace Petecat.IoC
{
    public class DefaultTypeDefinition : ITypeDefinition
    {
        public DefaultTypeDefinition(Type type)
        {
            Info = type;
            AssemblyInfo = new AssemblyInfo(type.Assembly);
        }

        public string Key { get { return (Info as Type).FullName; } }

        public MemberInfo Info { get; private set; }

        public AssemblyInfo AssemblyInfo { get; private set; }

        private IConstructorMethodDefinition[] _Constructors = null;

        public IConstructorMethodDefinition[] Constructors
        {
            get
            {
                if (_Constructors == null)
                {
                    var constructors = new IConstructorMethodDefinition[0];

                    (Info as Type).GetConstructors().ToList().ForEach(x =>
                    {
                        constructors = constructors.Concat(new IConstructorMethodDefinition[] { new DefaultConstructorMethodDefinition(x) }).ToArray();
                    });

                    _Constructors = constructors;
                }

                return _Constructors;
            }
        }

        private IInstanceMethodDefinition[] _InstanceMethods = null;

        public IInstanceMethodDefinition[] InstanceMethods
        {
            get
            {
                if (_InstanceMethods == null)
                {
                    var instanceMethods = new IInstanceMethodDefinition[0];

                    (Info as Type).GetMethods().ToList().ForEach(x =>
                    {
                        instanceMethods = instanceMethods.Concat(new IInstanceMethodDefinition[] { new DefaultInstanceMethodDefinition(x) }).ToArray();
                    });

                    _InstanceMethods = instanceMethods;
                }

                return _InstanceMethods;
            }
        }

        private IPropertyDefinition[] _Properties = null;

        public IPropertyDefinition[] Properties
        {
            get
            {
                if (_Properties == null)
                {
                    var properties = new IPropertyDefinition[0];

                    (Info as Type).GetProperties().ToList().ForEach(x =>
                    {
                        properties = properties.Concat(new IPropertyDefinition[] { new DefaultPropertyDefinition(x) }).ToArray();
                    });

                    _Properties = properties;
                }

                return _Properties;
            }
        }

        public object GetInstance(params object[] arguments)
        {
            return Activator.CreateInstance(Info as Type, arguments);
        }

        public bool IsImplementInterface(Type interfaceType)
        {
            return interfaceType.IsInterface && (Info as Type).IsClass && (Info as Type).GetInterfaces().ToList().Exists(x => x.FullName == interfaceType.FullName);
        }
    }
}