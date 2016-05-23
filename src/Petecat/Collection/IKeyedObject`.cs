namespace Petecat.Collection
{
    public interface IKeyedObject<T>
    {
        T Key { get; }
    }
}