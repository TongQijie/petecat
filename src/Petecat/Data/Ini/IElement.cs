namespace Petecat.Data.Ini
{
    public interface IElement : Collection.IKeyedObject<string>
    {
        T ReadObject<T>();

        void WriteObject(object instance);

        string Format();
    }
}
