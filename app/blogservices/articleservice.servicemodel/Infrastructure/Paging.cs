namespace ArticleService.ServiceModel.Infrastructure
{
    public class Paging
    {
        public Paging() { }

        public Paging(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; set; }

        public int TotalPages { get; set; }
        
        public int PageSize { get; set; }
    }
}
