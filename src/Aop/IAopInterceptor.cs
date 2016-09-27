namespace Petecat.Aop
{
    public interface IAopInterceptor
    {
        void Intercept(IAopInvocation invocation);
    }
}
