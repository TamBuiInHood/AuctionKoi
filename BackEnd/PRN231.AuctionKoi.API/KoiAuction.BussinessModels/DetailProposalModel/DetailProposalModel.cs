using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.BussinessModels.DetailProposalModel
{
    public class DetailProposalModel
    {
        [Key]
        public int FishId { get; set; }

        public string? FishCode { get; set; }

        public string? FishName { get; set; }

        public string? Gender { get; set; }

        public int? Age { get; set; }

        public double? Length { get; set; }

        public double? Weight { get; set; }

        public int? Rating { get; set; }

        public string? Status { get; set; }

        public DateOnly? CreateDate { get; set; }

        public DateOnly? UpdateDate { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public string? VideoUrl { get; set; }

        public string? Color { get; set; }

        public double? InitialPrice { get; set; }

        public double? FinalPrice { get; set; }

        public int? Index { get; set; }
        public int? TimeSpan { get; set; }
        public int? MinIncrement { get; set; }


        public int FishTypeId { get; set; }

        public string? FishTypeName { get; set; }

        public int FarmId { get; set; }

        public string? FarmName { get; set; }

        public int? AuctionId { get; set; }

        public string? AuctionName { get; set; }

        public double? AuctionFee { get; set; }
    }
}
