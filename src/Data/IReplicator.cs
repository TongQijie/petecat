namespace Petecat.Data
{
    public interface IReplicator
    {
        object ShallowCopy(object obj);

        T ShallowCopy<T>(object obj);

        object DeepCopy(object obj);

        T DeepCopy<T>(object obj);
    }
}
