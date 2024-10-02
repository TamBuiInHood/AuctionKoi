using System;
using System.Collections.Generic;

namespace KoiAuction.Repository.Entities;

public partial class Auction
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

    public virtual ICollection<DetailProposal> DetailProposals { get; set; } = new List<DetailProposal>();

    public virtual AuctionType? Type { get; set; } // Cho phép Type là null

}
