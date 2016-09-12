using ArticleService.BusinessInterface;
using ArticleService.RepositoryInterface;
using ArticleService.RepositoryTransfer;
using ArticleService.ServiceModel;
using ArticleService.ServiceModel.Infrastructure;
using Petecat.IoC.Attributes;
namespace ArticleService.BusinessImplement
{
    [AutoResolvable(typeof(IArticleServiceImpl))]
    public class ArticleServiceImpl : IArticleServiceImpl
    {
        private IArticleServiceRepo _ArticleServiceRepo;

        public ArticleServiceImpl(IArticleServiceRepo articleServiceRepo)
        {
            _ArticleServiceRepo = articleServiceRepo;
        }

        public ServiceResponse<ArticleInfo[]> GetArticlesByPage(ServiceRequest request)
        {
            var response = new ServiceResponse<ArticleInfo[]>();

            var pagingSource = PagingTransfer.BuildPagingSource(request.Paging);
            var articleInfoSources =_ArticleServiceRepo.GetArticlesByPage(pagingSource);
            if (articleInfoSources != null)
            {
                response.Body = ArticleInfoTransfer.BuildArticleInfos(articleInfoSources);
                response.Paging = PagingTransfer.BuildPaging(pagingSource);
            }

            return response;
        }

        public ServiceResponse<ArticleInfo> GetArticleById(ServiceRequest request)
        {
            var response = new ServiceResponse<ArticleInfo>();

            var articleInfoSource = _ArticleServiceRepo.GetArticleById(request.GetValue<string>("ArticleId", null));
            if (articleInfoSource != null)
            {
                response.Body = ArticleInfoTransfer.BuildArticleInfo(articleInfoSource);
            }

            return response;
        }

        public ServiceResponse EditArticleById(ServiceRequest<ArticleInfo> request)
        {
            var response = new ServiceResponse();

            _ArticleServiceRepo.EditArticleById(ArticleInfoTransfer.BuildArticleInfoSource(request.Body));

            return response;
        }
    }
}
