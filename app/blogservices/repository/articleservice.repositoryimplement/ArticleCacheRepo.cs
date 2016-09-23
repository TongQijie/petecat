using System.IO;
using Petecat.Extension;
using Petecat.Caching;
using ArticleService.RepositoryModel;
using System.Text;
using System.Linq;
using Petecat.Data.Formatters;
using Petecat.Threading.Watcher;
using System.Collections.Generic;
using System;

namespace ArticleService.RepositoryImplement
{
    public class ArticleCacheRepo
    {
        public ArticleCacheRepo(string path)
        {
            Path = path;
        }

        public string Path { get; private set; }

        public void Initialize()
        {
            if (!Path.HasValue() || !Directory.Exists(Path))
            {
                // log
                return;
            }

            var fileInfos = new DirectoryInfo(Path).GetFiles("*.json");

            foreach (var fileInfo in fileInfos)
            {
                CacheObjectManager.Instance.Add<ArticleInfoSource>(fileInfo.Name, fileInfo.FullName, Encoding.UTF8,
                    ObjectFormatterFactory.GetFormatter(ObjectFormatterType.DataContractJson), true);
            }

            FolderWatcherManager.Instance.GetOrAdd(Path)
                .SetFileCreatedHandler((f, n) =>
                {
                    CacheObjectManager.Instance.Add<ArticleInfoSource>(n, System.IO.Path.Combine(f.FullPath, n), Encoding.UTF8,
                        ObjectFormatterFactory.GetFormatter(ObjectFormatterType.DataContractJson), true);
                })
                .SetFileDeletedHandler((f, n) =>
                {

                }).Start();
        }

        public List<ArticleInfoSource> ReadMany(Predicate<ArticleInfoSource> predicate)
        {
            return CacheObjectManager.Instance.CacheObjects.Select(x => x.GetValue() as ArticleInfoSource)
                .Where(x => predicate(x)).ToList();
        }

        public ArticleInfoSource ReadSingle(Predicate<ArticleInfoSource> predicate)
        {
            return CacheObjectManager.Instance.CacheObjects.Select(x => x.GetValue() as ArticleInfoSource)
                .FirstOrDefault(x => predicate(x));
        }

        public bool Exists(Predicate<ArticleInfoSource> predicate)
        {
            return CacheObjectManager.Instance.CacheObjects.Select(x => x.GetValue() as ArticleInfoSource)
                .ToList().Exists(predicate);
        }

        public void Update(ArticleInfoSource articleInfoSource)
        {
            ObjectFormatterFactory.GetFormatter(ObjectFormatterType.DataContractJson).WriteObject(articleInfoSource,
                System.IO.Path.Combine(Path, articleInfoSource.Id) + ".json");
        }

        public void Insert(ArticleInfoSource articleInfoSource)
        {
            articleInfoSource.Id = System.IO.Path.GetRandomFileName().Replace(".", "");
            ObjectFormatterFactory.GetFormatter(ObjectFormatterType.DataContractJson).WriteObject(articleInfoSource,
                System.IO.Path.Combine(Path, articleInfoSource.Id) + ".json");
        }
    }
}
