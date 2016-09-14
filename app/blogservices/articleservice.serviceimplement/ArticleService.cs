using ArticleService.BusinessInterface;
using ArticleService.ServiceInterface;
using ArticleService.ServiceModel;
using ArticleService.ServiceModel.Infrastructure;
using Petecat.Service.Attributes;

namespace ArticleService.ServiceImplement
{
    [AutoService(typeof(IArticleService))]
    public class ArticleService : ServiceBase, IArticleService
    {
        private IArticleServiceImpl _ArticleServiceImpl;

        public ArticleService(IArticleServiceImpl articleServiceImpl)
        {
            _ArticleServiceImpl = articleServiceImpl;
        }

        public string Hi()
        {
            return "hi, welcome to access article service.";
        }

        public ServiceResponse<ArticleInfo[]> GetArticlesByPage(ServiceRequest request)
        {
            return Sandbox(request, _ArticleServiceImpl.GetArticlesByPage);
        }

        public ServiceResponse<ArticleInfo> GetArticleById(ServiceRequest request)
        {
            return Sandbox(request, _ArticleServiceImpl.GetArticleById);
        }

        public ServiceResponse OperateArticle(ServiceRequest<ArticleInfo> request)
        {
            return Sandbox(request, _ArticleServiceImpl.OperateArticle);
        }
    }
}
