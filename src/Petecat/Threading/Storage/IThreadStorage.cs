namespace Petecat.Threading.Storage
{
    public interface IThreadStorage
    {
        T Get<T>(string name);

        T Get<T>(string name, T defaultValue);

        void Set(string name, object value);

        void Remove(string name);
    }
}
