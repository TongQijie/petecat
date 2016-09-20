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

        public ArticleInfoSource[] GetArticleById(string id)
        {
            var article = _Cache.ReadSingle(x => !x.Deleted && string.Equals(x.Id, id, StringComparison.OrdinalIgnoreCase));
            if (article == null)
            {
                return null;
            }

            var previous = _Cache.ReadMany(x => !x.Deleted && x.CreationDate > article.CreationDate).OrderBy(x => x.CreationDate).FirstOrDefault();
            var next = _Cache.ReadMany(x => !x.Deleted && x.CreationDate < article.CreationDate).OrderByDescending(x => x.CreationDate).FirstOrDefault();

            return new ArticleInfoSource[3]
            {
                previous,
                article,
                next,
            };
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
            return Insert(articleInfoSource);
        }

        public bool ModifyArticle(ArticleInfoSource articleInfoSource)
        {
            var article = _Cache.ReadSingle(x => !x.Deleted && string.Equals(x.Id, articleInfoSource.Id, StringComparison.OrdinalIgnoreCase));
            if (article == null)
            {
                return false;
            }

            article.ModifiedDate = DateTime.Now;
            article.Title = articleInfoSource.Title;
            article.Abstract = articleInfoSource.Abstract;
            article.Content = articleInfoSource.Content;
            article.Signature = articleInfoSource.Signature;
            return Update(article);
        }

        public bool DeleteArticle(ArticleInfoSource articleInfoSource)
        {
            var article = _Cache.ReadSingle(x => !x.Deleted && string.Equals(x.Id, articleInfoSource.Id, StringComparison.OrdinalIgnoreCase));
            if (article == null)
            {
                return false;
            }

            article.ModifiedDate = DateTime.Now;
            article.Deleted = true;
            return Update(article);
        }

        private bool Update(ArticleInfoSource articleInfoSource)
        {
            try
            {
                _Cache.Update(articleInfoSource);
                return true;
            }
            catch (Exception e)
            {
                LoggerManager.GetLogger().LogEvent("ArticleServiceRepo", LoggerLevel.Error, e);
            }

            return false;
        }

        private bool Insert(ArticleInfoSource articleInfoSource)
        {
            try
            {
                _Cache.Insert(articleInfoSource);
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
