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
        ServiceResponse<ArticleInfo> GetArticleById(ServiceRequest request);

        [ServiceMethod(MethodName = "edit-article-by-id")]
        ServiceResponse EditArticleById(ServiceRequest<ArticleInfo> request);
    }
}