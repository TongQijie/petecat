using System.Runtime.Serialization;

namespace ArticleService.ServiceModel.Infrastructure
{
    [DataContract]
    public class Paging
    {
        public Paging() { }

        public Paging(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        [DataMember(Name = "pageNumber")]
        public int PageNumber { get; set; }

        [DataMember(Name = "totalPages")]
        public int TotalPages { get; set; }
        
        [DataMember(Name = "pageSize")]
        public int PageSize { get; set; }
    }
}
