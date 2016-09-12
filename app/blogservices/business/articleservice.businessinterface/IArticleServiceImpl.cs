using ArticleService.ServiceModel;
using ArticleService.ServiceModel.Infrastructure;

namespace ArticleService.BusinessInterface
{
    public interface IArticleServiceImpl
    {
        ServiceResponse<ArticleInfo[]> GetArticlesByPage(ServiceRequest request);

        ServiceResponse<ArticleInfo> GetArticleById(ServiceRequest request);

        ServiceResponse EditArticleById(ServiceRequest<ArticleInfo> request);
    }
}