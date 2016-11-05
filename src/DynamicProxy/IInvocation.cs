using System;
using System.Reflection;

namespace Petecat.DynamicProxy
{
    public interface IInvocation
    {
        void Process();

        object Owner { get; }

        MethodInfo MethodInfo { get; }

        object[] ParameterValues { get; }

        object ReturnValue { get; }

        Type TargetType { get; }

        Type InterceptorType { get; }
    }
}
