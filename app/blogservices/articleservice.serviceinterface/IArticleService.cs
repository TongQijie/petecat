using ArticleService.ServiceModel;
using ArticleService.ServiceModel.Infrastructure;
using Petecat.Service.Attributes;

namespace ArticleService.ServiceInterface
{
    [ServiceInterface(ServiceName = "article-service")]
    public interface IArticleService
    {
        [ServiceMethod(MethodName = "hi", IsDefaultMethod = true)]
        string Hi();

        [ServiceMethod(MethodName = "get-articles-by-page")]
        ServiceResponse<ArticleInfo[]> GetArticlesByPage(ServiceRequest request);

        [ServiceMethod(MethodName = "get-article-by-id")]
        ServiceResponse<ArticleInfo[]> GetArticleById(ServiceRequest request);

        [ServiceMethod(MethodName = "operate-article")]
        ServiceResponse OperateArticle(ServiceRequest<ArticleInfo> request);
    }
}