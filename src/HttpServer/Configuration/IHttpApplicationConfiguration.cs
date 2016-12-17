namespace Petecat.HttpServer.Configuration
{
    public interface IHttpApplicationConfiguration
    {
        KeyValueConfigurationItem[] HttpApplicationRoutingConfiguration { get; }

        KeyValueConfigurationItem[] StaticResourceMappingConfiguration { get; }

        KeyValueConfigurationItem[] RewriteRuleConfiguration { get; }

        KeyValueConfigurationItem[] ResponseHeaders { get; }
    }
}
