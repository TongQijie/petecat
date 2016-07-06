namespace Petecat.Restful
{
    public interface IContainerAdapter
    {
        T TryResolve<T>();

        T Resolve<T>();
    }
}