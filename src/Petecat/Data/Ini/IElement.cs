namespace Petecat.Data.Ini
{
    public interface IElement
    {
        T ReadObject<T>();

        void WriteObject(object instance);

        string Format();
    }
}
