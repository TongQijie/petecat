using System;

namespace ArticleService.RepositoryModel
{
    public class ArticleInfoSource
    {
        public string Id { get; set; }

        public DateTime CreationDate { get; set; }

        public string Title { get; set; }

        public string Abstract { get; set; }

        public string Content { get; set; }
    }
}
