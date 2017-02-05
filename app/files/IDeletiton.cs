namespace Files
{
    public interface IDeletiton
    {
        void Execute(string folder, string[] folders, string[] files);
    }
}