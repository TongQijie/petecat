using ArticleService.RepositoryModel;
using ArticleService.ServiceModel;
using System.Collections.Generic;
using Petecat.Extension;

namespace ArticleService.RepositoryTransfer
{
    public static class ArticleInfoTransfer
    {
        public static ArticleInfo[] BuildArticleInfos(IEnumerable<ArticleInfoSource> articleInfoSources)
        {
            if (articleInfoSources == null)
            {
                return null;
            }

            var articleInfos = new ArticleInfo[0];
            foreach (var articleInfoSource in articleInfoSources)
            {
                var articleInfo = BuildArticleInfo(articleInfoSource);
                if (articleInfo != null)
                {
                    articleInfos = articleInfos.Append(articleInfo);
                }
            }

            return articleInfos;
        }

        public static ArticleInfo BuildArticleInfo(ArticleInfoSource articleInfoSource)
        {
            if (articleInfoSource == null)
            {
                return null;
            }

            return new ArticleInfo()
            {
                Abstract = articleInfoSource.Abstract,
                Content = articleInfoSource.Content,
                CreationDate = articleInfoSource.CreationDate,
                Id = articleInfoSource.Id,
                Title = articleInfoSource.Title,
            };
        }

        public static ArticleInfoSource BuildArticleInfoSource(ArticleInfo articleInfo)
        {
            if (articleInfo == null)
            {
                return null;
            }

            return new ArticleInfoSource()
            {
                Abstract = articleInfo.Abstract,
                Content = articleInfo.Content,
                CreationDate = articleInfo.CreationDate,
                Id = articleInfo.Id,
                Title = articleInfo.Title,
            };
        }
    }
}
