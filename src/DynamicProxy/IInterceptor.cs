namespace Petecat.DynamicProxy
{
    public interface IInterceptor
    {
        void Intercept(IInvocation invocation);
    }
}
