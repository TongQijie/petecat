using ArticleService.RepositoryInterface;
using ArticleService.RepositoryModel;
using Petecat.IoC.Attributes;
using System;
using Petecat.Extension;
using System.Linq;
using System.IO;
using Petecat.Logging;

namespace ArticleService.RepositoryImplement
{
    [AutoResolvable(typeof(IArticleServiceRepo))]
    public class ArticleServiceRepo : IArticleServiceRepo
    {
        private ArticleCacheRepo _Cache = null;

        public ArticleServiceRepo()
        {
            if (_Cache == null)
            {
                if (!Directory.Exists("./repo".FullPath()))
                {
                    Directory.CreateDirectory("./repo".FullPath());
                }

                _Cache = new ArticleCacheRepo("./repo".FullPath());
                _Cache.Initialize();
            }
        }

        public ArticleInfoSource[] GetArticlesByPage(PagingSource pagingSource)
        {
            return _Cache.ReadMany(x => !x.Deleted).OrderByDescending(x => x.CreationDate)
                .Skip((pagingSource.PageNumber - 1) * pagingSource.PageSize).Take(pagingSource.PageSize).ToArray();
        }

        public ArticleInfoSource GetArticleById(string id)
        {
            return _Cache.ReadSingle(x => !x.Deleted && string.Equals(x.Id, id, StringComparison.OrdinalIgnoreCase));
        }

        public bool CreateArticle(ArticleInfoSource articleInfoSource)
        {
            if (_Cache.Exists(x => !x.Deleted && string.Equals(x.Id, articleInfoSource.Id, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            articleInfoSource.CreationDate = DateTime.Now;
            articleInfoSource.ModifiedDate = DateTime.Now;
            articleInfoSource.Deleted = false;

            return Write(articleInfoSource);
        }

        public bool ModifyArticle(ArticleInfoSource articleInfoSource)
        {
            var article = _Cache.ReadSingle(x => !x.Deleted && string.Equals(x.Id, articleInfoSource.Id, StringComparison.OrdinalIgnoreCase));
            if (article == null)
            {
                return false;
            }

            articleInfoSource.ModifiedDate = DateTime.Now;
            return Write(articleInfoSource);
        }

        public bool DeleteArticle(ArticleInfoSource articleInfoSource)
        {
            articleInfoSource.Deleted = true;
            return Write(articleInfoSource);
        }

        private bool Write(ArticleInfoSource articleInfoSource)
        {
            try
            {
                _Cache.Write(articleInfoSource);
                return true;
            }
            catch (Exception e)
            {
                LoggerManager.GetLogger().LogEvent("ArticleServiceRepo", LoggerLevel.Error, e);
            }

            return false;
        }
    }
}
