using Petecat.Service.Attributes;
namespace Petecat.ServiceHost
{
    [ServiceInterface(ServiceName = "article-service")]
    public interface IArticleService
    {
        [ServiceMethod(MethodName = "get-articles")]
        Article[] GetArticles(int pageNumber);

        [ServiceMethod(MethodName = "fetch-article")]
        Article FetchArticle(string id);
    }
}