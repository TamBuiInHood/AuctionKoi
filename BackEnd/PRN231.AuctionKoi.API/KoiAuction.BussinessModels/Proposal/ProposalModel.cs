using System.ComponentModel.DataAnnotations;

namespace KoiAuction.BussinessModels.Proposal
{
    public class ProposalModel
    {
        [Key]
        public int FarmId { get; set; }
        public string? FarmCode { get; set; }

        public string? FarmName { get; set; }

        public string? Location { get; set; }

        public string? AvatarUrl { get; set; }

        public DateTime? CreateDate { get; set; }

        public string? Status { get; set; }

        public string? Description { get; set; }

        public string? Owner { get; set; }

        public DateTime? UpdateDate { get; set; }
        public bool? IsDeleted { get; set; }
        public int? UserId { get; set; }

        public string? UserFullName { get; set; }

        //public List<DetailProposal>? DetailProposals { get; set; }
    }
}
