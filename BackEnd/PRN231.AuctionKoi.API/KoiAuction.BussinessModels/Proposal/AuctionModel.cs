using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.BussinessModels.Proposal
{
    public class AuctionModel
    {
        public int AuctionId { get; set; }

        public string? AuctionName { get; set; }

        public DateTime? AuctionDate { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? MinIncrement { get; set; }

        public string? Status { get; set; }

        public string? Description { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? AutionMethod { get; set; }

        public string? AuctionCode { get; set; }

        public int? TimeSpan { get; set; }

        public int TypeId { get; set; }

     //   public AuctionTypeModel AuctionType { get; set; }
    }
    public class AuctionUpdateModel
    {
        public int AuctionId { get; set; }

        public string? AuctionName { get; set; }

        public DateTime? AuctionDate { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? MinIncrement { get; set; }

        public string? Status { get; set; }

        public string? Description { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? AutionMethod { get; set; }

        public string? AuctionCode { get; set; }

        public int? TimeSpan { get; set; }

        public int TypeId { get; set; }

      //  public AuctionTypeModel AuctionType { get; set; }
    }


}
