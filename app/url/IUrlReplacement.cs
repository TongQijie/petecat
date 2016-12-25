namespace Petecat.App.Url
{
    public interface IUrlReplacement
    {
        void Execute(string folder, string value, string replacement);
    }
}
