using Petecat.Service.Attributes;
namespace Petecat.ServiceHost
{
    [AutoService(typeof(IArticleService))]
    public class ArticleService : IArticleService
    {
        public Article[] GetArticles(int pageNumber)
        {
            return new Article[]
            {
                new Article() { Name = "Nexus S", Snippet = "Fast just got faster with Nexus S." },
                new Article() { Name = "Motorola XOOM™ with Wi-Fi", Snippet = "The Next, Next Generation tablet." },
                new Article() { Name = "MOTOROLA XOOM™", Snippet = "The Next, Next Generation tablet." },
            };
        }

        public Article FetchArticle(string id)
        {
            return new Article() { Name = id, Snippet = "this is item detail...." };
        }
    }
}