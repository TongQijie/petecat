namespace Petecat.HttpServer.Configuration
{
    public interface IHttpApplicationConfiguration
    {
        KeyValueItemConfiguration[] HttpApplicationRoutingConfiguration { get; }

        KeyValueItemConfiguration[] StaticResourceMappingConfiguration { get; }

        KeyValueItemConfiguration[] ResponseHeaders { get; }

        RewriteRuleConfiguration[] RewriteRules { get; }

        HttpRedirectConfiguration[] HttpRedirects { get; }
    }
}
