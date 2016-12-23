using Petecat.HttpServer;
using Petecat.HttpServer.Attribute;

namespace Petecat.ServiceHost
{
    [RestServiceInjectable(ServiceName = "article", Singleton = true)]
    public class ArticleService
    {
        [RestServiceMethod(MethodName = "get-articles-by-page", HttpVerb = HttpVerb.Get)]
        public Article[] GetArticles(int pageNumber)
        {
            var headers = RestServiceHttpHandler.Request.Headers;

            var cookies = RestServiceHttpHandler.Request.Cookies;

            return new Article[]
            {
                new Article() { Name = "Nexus S", Snippet = "Fast just got faster with Nexus S." },
                new Article() { Name = "Motorola XOOM™ with Wi-Fi", Snippet = "The Next, Next Generation tablet." },
                new Article() { Name = "MOTOROLA XOOM™", Snippet = "The Next, Next Generation tablet." },
            };
        }

        [RestServiceMethod(MethodName = "get-article-by-id", HttpVerb = HttpVerb.Get)]
        public Article FetchArticle(string id)
        {
            return new Article() { Name = id, Snippet = "this is item detail...." };
        }

        [RestServiceMethod(MethodName = "post-article", HttpVerb = HttpVerb.Post)]
        public Article PostArticle([RestServiceParameter(Source = RestServiceParameterSource.Body)] Article article, 
            [RestServiceParameter(Source = RestServiceParameterSource.QueryString, Alias = "alias")] string id)
        {
            return article;
        }
    }
}