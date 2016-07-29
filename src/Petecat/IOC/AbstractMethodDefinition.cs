using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Petecat.IoC
{
    public abstract class AbstractMethodDefinition : IMethodDefinition
    {
        public AbstractMethodDefinition(MethodBase methodBase)
        {
            var parameters = methodBase.GetParameters();
            var arguments = new MethodArgument[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                arguments[i] = new MethodArgument()
                {
                    Index = i,
                    Name = parameters[i].Name,
                    ArgumentType = parameters[i].ParameterType,
                };
            }
            MethodArguments = arguments;
        }

        public IEnumerable<MethodArgument> MethodArguments { get; protected set; }

        public MemberInfo Info { get; protected set; }

        public virtual bool IsMatch(MethodArgument[] arguments)
        {
            foreach (var parameterInfo in (Info as MethodBase).GetParameters())
            {
                var argument = arguments.FirstOrDefault(x => x.Name.Equals(parameterInfo.Name, StringComparison.OrdinalIgnoreCase)
                    || x.Index == parameterInfo.Position);
                if (argument == null)
                {
                    return false;
                }

                try
                {
                    Convert.ChangeType(argument.ArgumentValue, parameterInfo.ParameterType);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public virtual bool TryGetArgumentValues(MethodArgument[] arguments, out object[] argumentValues)
        {
            argumentValues = new object[0];

            var methodBase = Info as MethodBase;

            if (arguments.Length != methodBase.GetParameters().Length)
            {
                return false;
            }

            foreach (var parameterInfo in methodBase.GetParameters().OrderBy(x => x.Position))
            {
                var argument = arguments.FirstOrDefault(x => x.Name.Equals(parameterInfo.Name, StringComparison.OrdinalIgnoreCase)
                    || x.Index == parameterInfo.Position);
                if (argument == null)
                {
                    return false;
                }

                try
                {
                    var argumentValue = Convert.ChangeType(argument.ArgumentValue, parameterInfo.ParameterType);
                    argumentValues = argumentValues.Concat(new object[] { argumentValue }).ToArray();
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
