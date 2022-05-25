namespace intelometry_app.Filters
{
    public class PaginationFilter
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public PaginationFilter()
        {
            Page = 1;
            PageSize = 10;
        }

        public PaginationFilter(int page, int pageSize)
        {
            Page = page < 1 ? 1 : page;
            PageSize = pageSize > 30 ? 30 : pageSize;
        }
    }
}
