using ArticleService.ServiceModel;
using ArticleService.ServiceModel.Infrastructure;
using ArticleService.ServiceProxy;
using System;
namespace articleservice.servicetest
{
    class Program
    {
        static void Main(string[] args)
        {
            var count = 10;
            while (count > 0)
            {
                ArticleServiceProxy.OperateArticle(new ServiceRequest<ArticleInfo>()
                {
                    ActionName = "Create",
                    Body = new ArticleInfo()
                    {
                        Title = "hello, World!",
                        Abstract = "hey, man",
                        Content = "this is a note about butterfly.",
                    },
                });
                var articles = ArticleServiceProxy.GetArticlesByPage(new ServiceRequest()
                {
                    Paging = new Paging(1, 10),
                });
                var article = ArticleServiceProxy.GetArticleById(new ServiceRequest()
                {
                    KeyValues = new KeyValuePair[] 
                    { 
                        new KeyValuePair("ArticleId", "hey"),
                    },
                });

                count--;
            }
        }
    }
}
