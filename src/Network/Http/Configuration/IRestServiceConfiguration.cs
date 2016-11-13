namespace Petecat.Network.Http.Configuration
{
    public interface IRestServiceConfiguration
    {
        RestServiceResourceConfiguration[] ResourceConfiguration { get; }
    }
}
