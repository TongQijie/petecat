using ArticleService.ServiceModel;
using ArticleService.ServiceModel.Infrastructure;
using Petecat.Service.Client;
namespace ArticleService.ServiceProxy
{
    public static class ArticleServiceProxy
    {
        public static ServiceResponse<ArticleInfo[]> GetArticlesByPage(ServiceRequest request)
        {
            return new ServiceClientBase("get-articles-by-page").Call<ServiceResponse<ArticleInfo[]>>(request);
        }

        public static ServiceResponse<ArticleInfo> GetArticleById(ServiceRequest request)
        {
            return new ServiceClientBase("get-article-by-id").Call<ServiceResponse<ArticleInfo>>(request);
        }

        public static ServiceResponse OperateArticle(ServiceRequest<ArticleInfo> request)
        {
            return new ServiceClientBase("operate-article").Call<ServiceResponse>(request);
        }
    }
}
