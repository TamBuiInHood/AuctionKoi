using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Common.Utils.Filters
{
    public class UserAuctionFilters
    {
        [FromQuery(Name = "filter-fish-type")]
        public string[]? fishTypeNames { get; set; }
        [FromQuery(Name = "filter-auction-code")]
        public string? auctionCode { get; set; }
        [FromQuery(Name = "filter-user-code")]
        public string? userCode { get; set; }


        [FromQuery(Name = "filter-is-winner")]
        public string? isWinner { get; set; }

        [FromQuery(Name = "filter-create-date-from")]
        public DateTime? createDateFrom { get; set; }
        [FromQuery(Name = "filter-create-date-to")]
        public DateTime? createDateTo { get; set; }
    }
}
