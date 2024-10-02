using KoiAuction.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Service.Responses
{
    public class ServiceResult<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public object Errors { get; set; }

        public bool IsSuccess { get; set; }
    }

    public class AuctionData
    {
        public List<Auction> List { get; set; }
    }


}

