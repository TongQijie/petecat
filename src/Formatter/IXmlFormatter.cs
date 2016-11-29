namespace Petecat.Formatter
{
    public interface IXmlFormatter : IFormatter
    {
        bool OmitNamespaces { get; set; }
    }
}
