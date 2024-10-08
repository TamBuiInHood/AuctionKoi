namespace KoiAuction.MVCWebApp.Models
{
    public class PaginatedViewModel<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public string Search { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}
