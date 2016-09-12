using ArticleService.RepositoryModel;
namespace ArticleService.RepositoryInterface
{
    public interface IArticleServiceRepo
    {
        ArticleInfoSource[] GetArticlesByPage(PagingSource pagingSource);

        ArticleInfoSource GetArticleById(string id);

        bool EditArticleById(ArticleInfoSource articleInfoSource);
    }
}
