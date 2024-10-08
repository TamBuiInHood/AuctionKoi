using KoiAuction.Common.Constants;
using Microsoft.AspNetCore.Mvc;

namespace KoiAuction.Common.Utils
{
    public class PaginationParameter
    {
        [FromQuery(Name = "page-index")]
        public int PageIndex { get; set; } = PageDefault.PAGE_INDEX;
        [FromQuery(Name = "page-size")]
        public int PageSize { get; set; } = PageDefault.PAGE_SIZE;
        [FromQuery(Name = "search-key")]
        public string? Search { get; set; }
        [FromQuery(Name = "sort-by")]
        public string? SortBy { get; set; }
        [FromQuery(Name = "direction")]
        public string? Direction { get; set; }

    }
}
