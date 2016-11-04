namespace Petecat.HttpServer.Configuration
{
    public interface IHttpApplicationConfiguration
    {
        KeyValueConfigurationItem[] HttpApplicationRoutingConfiguration { get; }

        KeyValueConfigurationItem[] StaticResourceMappingConfiguration { get; }
    }
}
