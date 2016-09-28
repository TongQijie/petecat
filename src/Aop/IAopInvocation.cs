using System.Reflection;

namespace Petecat.Aop
{
    public interface IAopInvocation
    {
        void Process();

        object Owner { get; }

        MethodInfo Method { get; }

        object[] ParameterValues { get; }

        object ReturnValue { get; }
    }
}
