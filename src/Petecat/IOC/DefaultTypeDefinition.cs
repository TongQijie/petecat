using System;
using System.Linq;
using System.Reflection;

namespace Petecat.IOC
{
    public class DefaultTypeDefinition : ITypeDefinition
    {
        public DefaultTypeDefinition(Type type)
        {
            Info = type;
        }

        public string Key { get { return (Info as Type).FullName; } }

        public MemberInfo Info { get; private set; }

        private IConstructorDefinition[] _Constructors = null;

        public IConstructorDefinition[] Constructors
        {
            get
            {
                if (_Constructors == null)
                {
                    var constructors = new IConstructorDefinition[0];

                    (Info as Type).GetConstructors().ToList().ForEach(x =>
                    {
                        constructors = constructors.Concat(new IConstructorDefinition[] { new DefaultConstructorDefinition(x) }).ToArray();
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