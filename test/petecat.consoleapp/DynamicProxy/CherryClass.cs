using Petecat.DynamicProxy;
using System;
namespace Petecat.ConsoleApp.DynamicProxy
{
    public class CherryClass : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            if (invocation.ParameterValues.Length > 0 && (int)invocation.ParameterValues[0] < 2)
            {
                throw new Exception("parameter value is wrong");
            }

            invocation.Process();

            if (invocation.ReturnValue != null && (int)invocation.ReturnValue < 2)
            {
                throw new Exception("return value is wrong");
            }
        }
    }
}
