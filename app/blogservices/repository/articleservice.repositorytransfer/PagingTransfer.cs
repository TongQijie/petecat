using ArticleService.RepositoryModel;
using ArticleService.ServiceModel.Infrastructure;
namespace ArticleService.RepositoryTransfer
{
    public static class PagingTransfer
    {
        public static Paging BuildPaging(PagingSource pagingSource)
        {
            if (pagingSource == null)
            {
                return null;
            }

            return new Paging()
            {
                PageNumber = pagingSource.PageNumber,
                PageSize = pagingSource.PageSize,
                TotalPages = pagingSource.TotalPages,
            };
        }

        public static PagingSource BuildPagingSource(Paging paging)
        {
            if (paging == null)
            {
                return null;
            }

            return new PagingSource()
            {
                PageNumber = paging.PageNumber,
                PageSize = paging.PageSize,
                TotalPages = paging.TotalPages,
            };
        }
    }
}
