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
            return _Cache.ReadAll().OrderByDescending(x => x.CreationDate)
                .Skip((pagingSource.PageNumber - 1) * pagingSource.PageSize).Take(pagingSource.PageSize).ToArray();
        }

        public ArticleInfoSource GetArticleById(string id)
        {
            return _Cache.Read(id);
        }

        public bool EditArticleById(ArticleInfoSource articleInfoSource)
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
