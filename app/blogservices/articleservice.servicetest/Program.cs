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
            var articles = ArticleServiceProxy.GetArticlesByPage(new ServiceRequest()
            {
                Paging = new Paging(1, 10),
            });
            var article = ArticleServiceProxy.GetArticleById(new ServiceRequest()
            {
                KeyValues = new KeyValuePair[] 
                { 
                    new KeyValuePair("ArticleId", "hello-world"),
                },
            });
            ArticleServiceProxy.EditArticleById(new ServiceRequest<ArticleInfo>()
            {
                Body = new ArticleInfo()
                {
                    Id = Environment.TickCount.ToString(),
                    CreationDate = DateTime.Now,
                    Title = "hello, World!",
                    Abstract = "hey, man",
                    Content = "this is a note about butterfly.",
                },
            });
        }
    }
}
