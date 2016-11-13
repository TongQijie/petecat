using Petecat.DependencyInjection;
using Petecat.DynamicProxy;
using Petecat.DynamicProxy.Attribute;
using Petecat.DynamicProxy.Errors;
using Petecat.Utility;
using System;
namespace Petecat.ConsoleApp.DynamicProxy
{
    public class DurianClass : BananaClass
    {
        //public DurianClass(AppleClass apple) : base(apple)
        //{
        //}

        public override int F(int a, int b)
        {
            var invocationBase = new InvocationBase();
            invocationBase.TargetType = typeof(BananaClass);
            invocationBase.ParameterValues = new object[] { a, b };
            invocationBase.MethodInfo = typeof(BananaClass).GetMethod("F", new Type[] { typeof(int), typeof(int) });
            invocationBase.InterceptorType = typeof(AppleClass);

            var interceptor = DependencyInjector.GetObject(invocationBase.InterceptorType) as IInterceptor;
            if (interceptor == null)
            {
                throw new InterceptorNotFoundException(invocationBase.InterceptorType);
            }
            interceptor.Intercept(invocationBase);

            return (int)invocationBase.ReturnValue;
        }
    }
}
