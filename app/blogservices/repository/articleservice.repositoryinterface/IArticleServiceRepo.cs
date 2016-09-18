using ArticleService.RepositoryModel;
namespace ArticleService.RepositoryInterface
{
    public interface IArticleServiceRepo
    {
        ArticleInfoSource[] GetArticlesByPage(PagingSource pagingSource);

        ArticleInfoSource[] GetArticleById(string id);

        bool CreateArticle(ArticleInfoSource articleInfoSource);

        bool ModifyArticle(ArticleInfoSource articleInfoSource);

        bool DeleteArticle(ArticleInfoSource articleInfoSource);
    }
}
